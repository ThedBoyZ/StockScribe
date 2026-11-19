#!/usr/bin/env bash
# deploy/scripts/bootstrap-vm.sh · one-time setup of a fresh Ubuntu 24.04 VM (run as `ubuntu`)
#
#   Docker Engine + Compose plugin (official apt repo) · log rotation · 2 GB swap · /opt/stockscribe
#
# Safe to run again. Log out and back in afterwards so the docker group applies.
set -euo pipefail

# ---- Docker ----
if ! command -v docker > /dev/null; then
  sudo apt-get update
  sudo apt-get install -y ca-certificates curl
  sudo install -m 0755 -d /etc/apt/keyrings
  sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
  sudo chmod a+r /etc/apt/keyrings/docker.asc
  echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" \
    | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
  sudo apt-get update
  sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
fi
sudo usermod -aG docker "$USER"

# Container logs: 3 × 10 MB each, so the 20 GB disk does not fill up.
echo '{"log-driver":"json-file","log-opts":{"max-size":"10m","max-file":"3"}}' \
  | sudo tee /etc/docker/daemon.json > /dev/null
sudo systemctl restart docker

# ---- Swap (t3.micro has 1 GB RAM) ----
if [[ ! -f /swapfile ]]; then
  sudo fallocate -l 2G /swapfile
  sudo chmod 600 /swapfile
  sudo mkswap /swapfile
  sudo swapon /swapfile
  echo '/swapfile none swap sw 0 0' | sudo tee -a /etc/fstab > /dev/null
fi
echo 'vm.swappiness=10' | sudo tee /etc/sysctl.d/99-swappiness.conf > /dev/null
sudo sysctl --system > /dev/null

# ---- App directory (deploy files arrive here from GitHub Actions; .env is created by hand) ----
sudo install -d -o "$USER" -g "$USER" -m 750 /opt/stockscribe

echo "Done. Log out and back in, then: docker run --rm hello-world && free -h"
