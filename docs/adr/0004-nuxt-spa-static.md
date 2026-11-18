# ADR-0004 · Nuxt 4 แบบ SPA build เป็น static เสิร์ฟด้วย Nginx

- สถานะ: Accepted · 17 พ.ย. 2026

## บริบท

ระบบอยู่หลังล็อกอิน ไม่ต้องการ SEO · VM e2-micro มี RAM 1 GB ต้องรัน API ด้วย · หน้า Case Study อยู่แยก

## ตัดสินใจ

- ตั้ง `ssr: false` แล้ว `nuxt generate` ได้ไฟล์ static
- Nginx เสิร์ฟไฟล์ static และส่ง `/api`, `/health` ต่อไปที่ container API · fallback ทุก path ไป `index.html`
- เรียก API แบบ same-origin (`/api/v1/...`) ไม่ต้องตั้ง CORS

## ผลที่ตามมา

- ✅ ไม่มี Node process บน VM ประหยัด RAM ได้ราว 150–250 MB
- ✅ cache ไฟล์ static ได้นาน (ชื่อไฟล์มี hash)
- ⚠️ หน้าแรกโหลด JS ก่อนแสดงผล → ใส่ skeleton loading
- ⚠️ ถ้าวันหนึ่งต้องการ SSR ต้องเพิ่ม Node container

## ทางเลือกที่ไม่เลือก

- **Nuxt SSR (Node server):** ใช้ RAM เพิ่ม และไม่ได้ประโยชน์เพราะอยู่หลังล็อกอิน
- **โฮสต์ frontend แยกบน Cloudflare Pages / Vercel:** ฟรีและเร็ว แต่ต้องตั้ง CORS และ cookie ข้ามโดเมน ซึ่งขัดกับ ADR-0003
