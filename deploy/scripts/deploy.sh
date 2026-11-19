#!/usr/bin/env bash
# deploy/scripts/deploy.sh · runs ON THE VM (called by .github/workflows/cd.yml over SSH)
#
#   bash /opt/stockscribe/scripts/deploy.sh sha-<git sha> [--skip-migrate]
#
#   1. pull the images for that tag
#   2. first run only: get a Let's Encrypt certificate (certbot standalone on :80)
#   3. run the EF Core migration bundle against Neon (skip with --skip-migrate)
#   4. docker compose up -d
#   5. wait for https://$DOMAIN/health/ready; if it never passes, start the previous tag again
#
# Migrations are NOT rolled back. Keep them backward compatible (add first, remove in a
# later release) so the previous image still works on the new schema.
set -euo pipefail

APP_DIR=/opt/stockscribe
cd "$APP_DIR"

TAG="${1:-}"
if [[ ! "$TAG" =~ ^sha-[0-9a-f]{7,40}$ ]]; then
  echo "usage: deploy.sh sha-<git sha> [--skip-migrate]" >&2
  exit 2
fi
SKIP_MIGRATE=false
[[ "${2:-}" == "--skip-migrate" ]] && SKIP_MIGRATE=true

if [[ ! -f .env ]]; then
  echo "error: $APP_DIR/.env is missing (template: deploy/.env.example)" >&2
  exit 1
fi

# Read one key from .env without sourcing it (connection strings contain ';' and spaces).
env_value() {
  sed -n "s/^$1=//p" .env | tail -n 1 | sed -e "s/^'\(.*\)'\$/\1/" -e 's/^"\(.*\)"$/\1/'
}
DOMAIN="$(env_value DOMAIN)"
EMAIL="$(env_value LETSENCRYPT_EMAIL)"
if [[ -z "$DOMAIN" || -z "$EMAIL" ]]; then
  echo "error: DOMAIN and LETSENCRYPT_EMAIL must be set in .env" >&2
  exit 1
fi

export IMAGE_TAG="$TAG"
compose() { docker compose -f docker-compose.prod.yml --profile migrate "$@"; }
log() { printf '\n==> %s\n' "$*"; }

# Files arrive by tar from the runner; make sure the nginx entrypoint hook stays executable.
chmod +x nginx/*.envsh scripts/*.sh

mkdir -p .releases
PREVIOUS="$(cat .releases/current 2>/dev/null || true)"
log "Deploying $TAG (previous: ${PREVIOUS:-none})"

log "Pulling images"
# --ignore-pull-failures: a manual rollback without a GHCR login can still use local images.
compose pull --quiet --ignore-pull-failures

if ! compose run --rm --no-deps --entrypoint test certbot -f "/etc/letsencrypt/live/$DOMAIN/fullchain.pem"; then
  log "No certificate for $DOMAIN yet, requesting one from Let's Encrypt"
  compose stop proxy 2>/dev/null || true
  compose run --rm --no-deps -p 80:80 --entrypoint certbot certbot \
    certonly --standalone --non-interactive --agree-tos --no-eff-email \
    -m "$EMAIL" -d "$DOMAIN"
fi

if [[ "$SKIP_MIGRATE" == false ]]; then
  log "Running database migrations"
  compose run --rm migrate
fi

log "Starting containers"
compose up -d --remove-orphans api web proxy certbot

wait_ready() {
  local code
  for i in $(seq 1 30); do
    code="$(curl -s -o /dev/null -w '%{http_code}' --max-time 10 \
      --resolve "$DOMAIN:443:127.0.0.1" "https://$DOMAIN/health/ready" || true)"
    echo "  attempt $i: $code"
    [[ "$code" == "200" ]] && return 0
    sleep 3
  done
  return 1
}

log "Waiting for https://$DOMAIN/health/ready"
if wait_ready; then
  if [[ -n "$PREVIOUS" && "$PREVIOUS" != "$TAG" ]]; then
    echo "$PREVIOUS" > .releases/previous
  fi
  echo "$TAG" > .releases/current
  echo "$(date -u +%FT%TZ) $TAG" >> .releases/history
  # Old images (>10 days) go; recent ones stay so a rollback needs no download.
  docker image prune -af --filter "until=240h" > /dev/null
  log "Deployed $TAG"
  exit 0
fi

log "Health check failed"
compose logs --no-color --tail 50 api web proxy || true
if [[ -n "$PREVIOUS" && "$PREVIOUS" != "$TAG" ]]; then
  log "Rolling back to $PREVIOUS"
  export IMAGE_TAG="$PREVIOUS"
  compose up -d --remove-orphans api web proxy certbot
fi
exit 1
