# deploy/

Files that run **on the VM** (Google Compute Engine e2-micro). Filled in by the deploy slice (`infra/cd-gce`); empty for now.

| Folder | What goes here |
|---|---|
| `nginx/` | The public front proxy on the VM: TLS with Let's Encrypt (certbot), the domain name and security headers. It forwards every request to the `web` container. |
| `scripts/` | `deploy.sh` (pull images, run the migration bundle, `docker compose up -d`, check `/health/ready`) and `rollback.sh` (go back to the previous image tag). |

## Two nginx configs, two jobs

```
Browser ──HTTPS──► deploy/nginx/ (VM, :443) ──HTTP──► apps/web/nginx.conf (web container, :80)
                   TLS · domain · security headers      static SPA files · /api and /health → api:8080
```

- **`apps/web/nginx.conf`** is baked into the `web` image and runs the same way locally (`docker compose up`) and in production.
- **`deploy/nginx/`** exists only on the VM. Locally there is no TLS, so you open `http://localhost:8080` and reach the web container directly.
