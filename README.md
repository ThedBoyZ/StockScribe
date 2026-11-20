# StockScribe

ระบบจัดซื้อและสต็อกสำหรับซูเปอร์มาร์เก็ตหลายสาขา — ใบขอซื้อ → ใบสั่งซื้อ → รับของ → สต็อกรายล็อตพร้อมแจ้งเตือนวันหมดอายุ

[![CI](https://github.com/ThedBoyZ/StockScribe/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/ThedBoyZ/StockScribe/actions/workflows/ci.yml)
![Status](https://img.shields.io/badge/phase-Dev%20%C2%B7%20slice%201-orange)
[![Live](https://img.shields.io/badge/demo-live-brightgreen)](https://stockscribe.duckdns.org)
![Docs](https://img.shields.io/badge/release-docs--v0.3-informational)
![License](https://img.shields.io/badge/license-MIT-green)

> **สถานะ:** เฟส BA, UX/UI และ SA เสร็จแล้ว · **Walking skeleton (Slice 0) ขึ้น production แล้ว** พร้อม CI/CD ไป AWS EC2 · กำลังทำ **Slice 1 · Demo login + seed**

| 🔗 | ลิงก์ |
| --- | --- |
| 🌐 Live Demo | [stockscribe.duckdns.org](https://stockscribe.duckdns.org) · [สถานะระบบ](https://stockscribe.duckdns.org/health/ready) |
| 🎨 ดีไซน์ (Figma) | [StockScribe — UI v1.0](ใส่ลิงก์) |
| 📋 Project Board | [StockScribe MVP](https://github.com/users/ThedBoyZ/projects/2) |
| 📄 เอกสาร | [BA](docs/ba/README.md) · [UX/UI](docs/ux/README.md) · [SA](docs/sa/README.md) · [ADR](docs/adr/README.md) |
| 📦 Releases | [Releases](https://github.com/ThedBoyZ/StockScribe/releases) |

---

## ปัญหาที่แก้

ร้านมินิมาร์ท 3 สาขาสั่งของผ่าน LINE + Excel ทำให้

- ใช้เวลารวมรายการและทำใบสั่งซื้อ **2–3 ชั่วโมงต่อสัปดาห์**
- **สั่งผิดหน่วย** จนของล้นโกดัง (เกือบ 10,000 บาทในครั้งเดียว)
- **รับของไม่ครบแต่จ่ายเงินเต็ม** เพราะไม่มีเลขที่เอกสาร
- **ของหมดอายุทิ้ง** 8,000–10,000 บาทต่อเดือน

ปัญหาทั้งหมดได้จากการสัมภาษณ์ stakeholder 3 role อ่านต่อที่ [Problem Statement](docs/ba/problem-statement.md)

## เป้าหมาย

| ตัวชี้วัด | ตอนนี้ | เป้าหมาย |
| --- | --- | --- |
| เวลาทำใบสั่งซื้อต่อสัปดาห์ | 2–3 ชม. | ≤ 30 นาที |
| มูลค่าของหมดอายุทิ้ง | 8,000–10,000 บาท/เดือน | ลดลง ≥ 50% |
| การสั่งผิดหน่วย | ~1 ครั้ง/เดือน | 0 |

## ฟีเจอร์หลัก (MVP)

ลำดับพัฒนาตาม [SA README](docs/sa/README.md#ลำดับ-dev-walking-skeleton-ก่อน)

| Story | ฟีเจอร์ | สถานะ |
| --- | --- | --- |
| [#2](https://github.com/ThedBoyZ/StockScribe/issues/2) | อนุมัติ PO ตามวงเงิน | ⏳ Ready · Slice 2 |
| [#1](https://github.com/ThedBoyZ/StockScribe/issues/1) | รับของไม่ครบตาม PO | ⏳ Ready · Slice 3 |
| [#3](https://github.com/ThedBoyZ/StockScribe/issues/3) | แจ้งเตือนสินค้าใกล้หมดอายุ | ⏳ Ready · Slice 4 |
| [#4](https://github.com/ThedBoyZ/StockScribe/issues/4) | เลือกหน่วยใน PR และแปลงเป็นหน่วยซื้อ | ⏳ Ready · Slice 5 |
| [#5](https://github.com/ThedBoyZ/StockScribe/issues/5) | รวม PR หลายสาขาเป็น PO แยกตามผู้ขาย | ⏳ Ready · Slice 5 |
| [US-007](docs/ba/user-stories/US-007-send-po-to-supplier.md) | ส่ง PO ให้ผู้ขายเป็น PDF | 📝 Draft · Slice 5 |
| [#6](https://github.com/ThedBoyZ/StockScribe/issues/6) | สร้าง PR อัตโนมัติจากสินค้าต่ำกว่า reorder point | ⏳ Ready · Slice 6 |

## Business Flow

![To-Be flow](docs/ba/business-flow/images/to-be.png)

## สถาปัตยกรรม

```
มือถือ / เดสก์ท็อป ──HTTPS──► Nginx ──► Nuxt 4 (SPA, static)
                                  └──► ASP.NET Core API ──► PostgreSQL (ธุรกรรม)
                                                        └──► MongoDB (audit log, แจ้งเตือน)
```

รายละเอียดใน [architecture.md](docs/sa/architecture.md) · เหตุผลของแต่ละการตัดสินใจใน [ADR](docs/adr/README.md)

## Tech Stack

| ชั้น | เทคโนโลยี |
| --- | --- |
| Frontend | Nuxt 4 · Vue 3 · TypeScript · Composition API · Tailwind · Pinia |
| Backend | ASP.NET Core (.NET 10) · Entity Framework Core · REST API · Serilog |
| Database | PostgreSQL (Neon) · MongoDB (Atlas) |
| Testing | xUnit · Testcontainers · Playwright |
| DevOps | Docker · GitHub Actions (CI/CD) · GHCR · AWS EC2 · Nginx · Let's Encrypt |

## กระบวนการทำงาน

```
BA ──► UX/UI ──► SA ──► Dev ──► QA ──► DevOps ──► Release
 ✅      ✅      ✅     🚧              ✅
```

| เฟส | เอกสาร | สถานะ | Release |
| --- | --- | --- | --- |
| Discovery (BA) | [docs/ba](docs/ba/README.md) | ✅ | [docs-v0.1](https://github.com/ThedBoyZ/StockScribe/releases/tag/docs-v0.1) |
| UX/UI + AC v1.1 | [docs/ux](docs/ux/README.md) | ✅ | [docs-v0.2](https://github.com/ThedBoyZ/StockScribe/releases/tag/docs-v0.2) |
| System Design (SA + ADR) | [docs/sa](docs/sa/README.md) · [docs/adr](docs/adr/README.md) | ✅ | [docs-v0.3](https://github.com/ThedBoyZ/StockScribe/releases/tag/docs-v0.3) |
| Development | [apps/](apps) | 🚧 Slice 0 ✅ · Slice 1 | v0.4 |
| QA | [docs/qa](docs/qa) | ⏳ | |
| Deploy (CI/CD) | [deploy/](deploy/README.md) · [ADR-0006](docs/adr/0006-hosting-aws-ec2-sydney.md) | ✅ AWS EC2 · GitHub Actions | |

## โครงสร้าง Repo

```
apps/
  web/        → Nuxt 4 frontend (SPA)
  api/        → .NET 10 API + tests
deploy/       → Nginx บน VM, สคริปต์ deploy/rollback
docs/
  ba/         → problem statement, สัมภาษณ์, user stories
  ux/         → ดีไซน์, screen inventory, annotations
  sa/         → architecture, ERD, state machines, RBAC, API spec
  adr/        → บันทึกการตัดสินใจทางเทคนิค
  qa/ devops/ releases/
docker-compose.yml  → รันทั้งระบบในเครื่อง
```

## เริ่มต้นใช้งาน (Local)

ต้องมี Docker เท่านั้น

```bash
cp .env.example .env        # ไม่บังคับ · ถ้าไม่มีจะใช้ค่า default
docker compose up --build
# เปิด http://localhost:8080 · สถานะระบบ http://localhost:8080/health/ready
```

compose จะรัน PostgreSQL และ MongoDB → `migrate` (EF Core migration แล้วจบ) → `api` → `web` (nginx เสิร์ฟ SPA และส่ง `/api`, `/health` ต่อให้ API)

> ถ้าพอร์ต 8080 มีโปรแกรมอื่นใช้อยู่ ให้ตั้ง `WEB_PORT=8081` ใน `.env`

### พัฒนาแบบ hot reload

ต้องมี .NET 10 SDK และ Node 24

```bash
docker compose up -d postgres mongo                                    # DB อย่างเดียว (5433, 27017)
cd apps/api && dotnet run --project StockScribe.Api --launch-profile http   # API → :5089
cd apps/web && npm install && npm run dev                              # เว็บ → http://localhost:3000
```

สร้างตารางครั้งแรก: `cd apps/api && ASPNETCORE_ENVIRONMENT=Development dotnet ef database update --project StockScribe.Api`

### Tests

```bash
cd apps/api
dotnet test StockScribe.slnx                                     # ทั้งหมด (integration ต้องเปิด Docker)
dotnet test StockScribe.slnx --filter "Category!=Integration"   # unit อย่างเดียว
```

integration test ใช้ Testcontainers สร้าง PostgreSQL และ MongoDB ชั่วคราวเอง ไม่ใช้ DB ใน compose

## การมีส่วนร่วม

ดู [CONTRIBUTING.md](CONTRIBUTING.md) สำหรับ branch, commit และขั้นตอน PR

---

_ชื่อร้าน ผู้ขาย และข้อมูลในบันทึกสัมภาษณ์เป็นข้อมูลสมมติเพื่อการฝึกกระบวนการทำงาน_
