# StockScribe

ระบบจัดซื้อและสต็อกสำหรับซูเปอร์มาร์เก็ตหลายสาขา — ใบขอซื้อ → ใบสั่งซื้อ → รับของ → สต็อกรายล็อตพร้อมแจ้งเตือนวันหมดอายุ

![Status](https://img.shields.io/badge/phase-UX%20%2F%20UI-blue)
![Docs](https://img.shields.io/badge/release-docs--v0.1-informational)
![License](https://img.shields.io/badge/license-MIT-green)

> **สถานะ:** เฟส Discovery (BA) เสร็จแล้ว · กำลังทำเฟส UX/UI

| 🔗 | ลิงก์ |
| --- | --- |
| 🌐 Live Demo | _เร็วๆ นี้_ |
| 📋 Project Board | [StockFlow MVP](https://github.com/users/ThedBoyZ/projects/2) |
| 📄 เอกสาร BA | [docs/ba](docs/ba/README.md) |
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

| Story | ฟีเจอร์ | สถานะ |
| --- | --- | --- |
| [#1](https://github.com/ThedBoyZ/StockScribe/issues/1) | รับของไม่ครบตาม PO | ⏳ Ready |
| [#2](https://github.com/ThedBoyZ/StockScribe/issues/2) | อนุมัติ PO ตามวงเงิน | ⏳ Ready |
| [#3](https://github.com/ThedBoyZ/StockScribe/issues/3) | แจ้งเตือนสินค้าใกล้หมดอายุ | ⏳ Ready |
| [#4](https://github.com/ThedBoyZ/StockScribe/issues/4) | เลือกหน่วยใน PR และแปลงเป็นหน่วยซื้อ | ⏳ Ready |
| [#5](https://github.com/ThedBoyZ/StockScribe/issues/5) | รวม PR หลายสาขาเป็น PO แยกตามผู้ขาย | ⏳ Ready |
| [#6](https://github.com/ThedBoyZ/StockScribe/issues/6) | สร้าง PR อัตโนมัติจากสินค้าต่ำกว่า reorder point | ⏳ Ready |

## Business Flow

![To-Be flow](docs/ba/business-flow/images/to-be.png)

## Tech Stack

| ชั้น | เทคโนโลยี |
| --- | --- |
| Frontend | Nuxt 4 · Vue 3 · TypeScript · Composition API |
| Backend | ASP.NET Core (.NET 10) · Entity Framework Core · REST API |
| Database | PostgreSQL (Neon) · MongoDB (Atlas) |
| DevOps | Docker · GitHub Actions · Google Compute Engine · Nginx |

## กระบวนการทำงาน

```
BA ──► UX/UI ──► SA ──► Dev ──► QA ──► DevOps ──► Release
 ✅       🚧
```

| เฟส | เอกสาร | สถานะ |
| --- | --- | --- |
| Discovery (BA) | [docs/ba](docs/ba/README.md) | ✅ [docs-v0.1](https://github.com/ThedBoyZ/StockScribe/releases/tag/docs-v0.1) |
| UX/UI | [docs/ux](docs/ux) | 🚧 กำลังทำ |
| System Design (SA) | [docs/sa](docs/sa) | ⏳ |
| Development | [apps/](apps) | ⏳ |
| Deploy | [docs/devops](docs/devops) | ⏳ |

## โครงสร้าง Repo

```
apps/web      → Nuxt frontend
apps/api      → .NET backend
deploy/       → Docker, Nginx, scripts
docs/         → เอกสารแยกตามบทบาท (BA, UX, SA, QA, DevOps)
```

## เริ่มต้นใช้งาน (Local)

_จะเพิ่มเมื่อเริ่มเฟส Development_

## การมีส่วนร่วม

ดู [CONTRIBUTING.md](CONTRIBUTING.md) สำหรับ branch, commit และขั้นตอน PR

---

_ข้อมูลในบันทึกสัมภาษณ์เป็นข้อมูลสมมติเพื่อการฝึกกระบวนการทำงาน_