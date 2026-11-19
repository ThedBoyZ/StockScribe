#!/usr/bin/env bash
# deploy/scripts/rollback.sh · runs ON THE VM
#
#   bash /opt/stockscribe/scripts/rollback.sh
#
# Starts the previous release again (from .releases/previous) without running migrations.
# Afterwards "previous" points at the release you left, so running it twice goes back forward.
set -euo pipefail

cd /opt/stockscribe
PREVIOUS="$(cat .releases/previous 2>/dev/null || true)"
if [[ -z "$PREVIOUS" ]]; then
  echo "No previous release recorded in /opt/stockscribe/.releases/previous" >&2
  exit 1
fi

echo "Current: $(cat .releases/current 2>/dev/null || echo none) → rolling back to: $PREVIOUS"
exec bash scripts/deploy.sh "$PREVIOUS" --skip-migrate
