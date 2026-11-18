# ADR-0003 · ล็อกอินเดโมด้วย persona + JWT ใน cookie httpOnly

- สถานะ: Accepted · 17 พ.ย. 2026

## บริบท

ผู้ใช้หลักของลิงก์เดโมคือ HR และ tech lead ที่ไม่อยากสมัครสมาชิก แต่ต้องได้ลองหลาย role เพื่อเห็นสิทธิ์ที่ต่างกัน ระบบยังต้องตรวจสิทธิ์จริงที่ API (US-002 AC 3 "เรียก API ตรงได้ 403")

## ตัดสินใจ

- หน้า login แสดง persona 4 คน กดแล้วเรียก `POST /api/v1/auth/demo-login`
- API ออก **JWT** (อายุ 8 ชม.) ใส่ใน cookie `sf_session` แบบ `HttpOnly; Secure; SameSite=Lax`
- claim: `sub`, `name`, `role`, `branch_id` · ตรวจสิทธิ์ด้วย ASP.NET Core policy + resource-based authorization
- เปิด endpoint นี้เฉพาะเมื่อ `DEMO_MODE=true`

## ผลที่ตามมา

- ✅ HR เข้าใช้ได้ในคลิกเดียว
- ✅ โค้ดตรวจสิทธิ์เป็นของจริง เปลี่ยนไปใช้ล็อกอินจริงทีหลังได้โดยแก้แค่ส่วนออก token
- ✅ JavaScript อ่าน token ไม่ได้ ลดความเสี่ยง XSS
- ⚠️ ใช้ cookie จึงต้องกัน CSRF: frontend กับ API อยู่โดเมนเดียวกัน (Nginx) + `SameSite=Lax` + action ที่เปลี่ยนข้อมูลเป็น POST/PUT/PATCH/DELETE เท่านั้น

## ทางเลือกที่ไม่เลือก

- **ASP.NET Core Identity + สมัครสมาชิก:** เกินขอบเขต และ HR ไม่สมัคร
- **เก็บ JWT ใน localStorage:** เสี่ยง XSS
- **OAuth (Google):** ต้องตั้งค่า consent screen และ HR บางคนไม่อยากผูกบัญชี
