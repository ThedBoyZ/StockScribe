# Architecture Decision Records

บันทึกการตัดสินใจทางเทคนิคที่สำคัญ 1 เรื่องต่อ 1 ไฟล์ · เขียนแล้วไม่แก้ ถ้าเปลี่ยนใจให้เขียน ADR ใหม่แล้วเปลี่ยนสถานะของอันเก่าเป็น "Superseded by ADR-XXXX"

| # | เรื่อง | สถานะ |
| --- | --- | --- |
| [0001](0001-postgres-mongo-split.md) | PostgreSQL เก็บธุรกรรม · MongoDB เก็บ audit log และแจ้งเตือน | Accepted |
| [0002](0002-modular-monolith-feature-folders.md) | Modular monolith แบ่งโฟลเดอร์ตามฟีเจอร์ | Accepted |
| [0003](0003-demo-auth-jwt-cookie.md) | ล็อกอินเดโมด้วย persona + JWT ใน cookie httpOnly | Accepted |
| [0004](0004-nuxt-spa-static.md) | Nuxt 4 แบบ SPA build เป็น static เสิร์ฟด้วย Nginx | Accepted |
| [0005](0005-money-and-quantity.md) | เงินใช้ numeric(12,2) · จำนวนเป็นจำนวนเต็มในหน่วยฐาน | Accepted |

แม่แบบ: บริบท → ตัดสินใจ → ผลที่ตามมา → ทางเลือกที่ไม่เลือก
