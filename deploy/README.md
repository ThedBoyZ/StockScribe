# deploy/

ไฟล์ที่รัน**บน VM** (AWS EC2 t3.micro · Sydney · [ADR-0006](../docs/adr/0006-hosting-aws-ec2-sydney.md)) · GitHub Actions ([cd.yml](../.github/workflows/cd.yml)) copy โฟลเดอร์นี้ไปไว้ที่ `/opt/stockscribe` ทุกครั้งที่ deploy

| ไฟล์ | หน้าที่ |
| --- | --- |
| `docker-compose.prod.yml` | stack จริง: `proxy` (TLS) → `web` → `api` · `migrate` (รันครั้งเดียวต่อ deploy) · `certbot` (ต่ออายุใบรับรอง) · ใช้ image จาก GHCR ไม่ build บน VM |
| `nginx/templates/` | front proxy: TLS, โดเมน, security headers, HTTP → HTTPS |
| `nginx/90-reload-certs.envsh` | reload nginx ทุก 6 ชม. ให้เห็นใบรับรองที่ต่ออายุแล้ว |
| `scripts/deploy.sh` | pull → ขอใบรับรองครั้งแรก → migration → `compose up` → เช็ก `/health/ready` → rollback อัตโนมัติถ้าไม่ผ่าน |
| `scripts/rollback.sh` | กลับไป release ก่อนหน้า (ไม่รัน migration) |
| `scripts/bootstrap-vm.sh` | ติดตั้ง Docker, log rotation, swap 2 GB บน VM ใหม่ (ครั้งเดียว) |
| `.env.example` | แม่แบบ `/opt/stockscribe/.env` (secret ของ DB อยู่บน VM เท่านั้น) |

## Nginx 2 ชั้น 2 หน้าที่

```
Browser ──HTTPS──► proxy (deploy/nginx/, :443) ──HTTP──► web (apps/web/nginx.conf, :80) ──► api:8080
                   TLS · domain · security headers         SPA static · /api, /health → api
```

- **`apps/web/nginx.conf`** อยู่ใน image `web` ทำงานเหมือนกันทั้งในเครื่องและบน server
- **`deploy/nginx/`** มีเฉพาะบน VM ในเครื่องไม่มี TLS จึงเปิด `http://localhost:8080` ตรงไปที่ web

## ตั้งค่าครั้งแรก

1. **EC2:** Ubuntu 24.04 x86_64 · t3.micro · gp3 20 GB · Credit specification = Standard · Elastic IP
   Security group: 80, 443 จากทุกที่ · 22 จากทุกที่ (รับเฉพาะ key, ให้ GitHub Actions เข้าได้)
2. **VM:** `bash bootstrap-vm.sh` (หรือวางคำสั่งในไฟล์) แล้ว logout/login ใหม่
3. **DuckDNS:** สร้าง subdomain แล้วตั้ง IP เป็น Elastic IP
4. **`.env`:** `nano /opt/stockscribe/.env` ตาม `.env.example` แล้ว `chmod 600 /opt/stockscribe/.env`
5. **Atlas:** Network Access เพิ่ม `<Elastic IP>/32`
6. **Deploy key** (บนเครื่อง dev): สร้าง key คู่ใหม่ → `.pub` ต่อท้าย `~/.ssh/authorized_keys` บน VM → private key ไปที่ GitHub Secret
7. **GitHub** (Settings → Secrets and variables → Actions):
   - secrets: `EC2_HOST`, `EC2_SSH_KEY`, `EC2_KNOWN_HOSTS` (`ssh-keyscan -t ed25519 <IP>`)
   - variables: `DOMAIN`
8. merge เข้า main → CI ผ่าน → CD deploy · หรือ Actions → **CD** → **Run workflow**

deploy ครั้งแรกจะขอใบรับรอง Let's Encrypt ให้เอง (DuckDNS ต้องชี้ IP ถูกแล้ว และพอร์ต 80 ต้องเปิด)

## ใช้งานประจำ (บน VM)

```bash
cd /opt/stockscribe
cat .releases/current .releases/history          # release ที่รันอยู่ / ประวัติ
IMAGE_TAG=$(cat .releases/current) docker compose -f docker-compose.prod.yml logs -f api
bash scripts/rollback.sh                          # กลับไป release ก่อนหน้า
```

Uptime monitor ให้ใช้ `/health/live` เท่านั้น · `/health/ready` แตะ DB ทุกครั้ง ถ้า poll ตลอดเวลา Neon จะไม่ได้หลับ
