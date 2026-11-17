# ADR-0001 · PostgreSQL เก็บธุรกรรม · MongoDB เก็บ audit log และแจ้งเตือน

- สถานะ: Accepted · 17 พ.ย. 2026
- ผู้ตัดสินใจ: SA, Tech Lead

## บริบท

โจทย์กำหนดให้ใช้ทั้ง PostgreSQL และ MongoDB ข้อมูลของระบบมี 2 ลักษณะ:
- PR, PO, ใบรับของ, สต็อก ต้องถูกต้องร่วมกันใน transaction เดียว เช่น รับของต้องตัดยอดค้าง เพิ่มล็อต และเขียนสมุดสต็อกพร้อมกัน
- audit log และแจ้งเตือน เขียนเพิ่มอย่างเดียว รูปร่างต่างกันตาม action และไม่ต้อง join กับธุรกรรม

## ตัดสินใจ

- **PostgreSQL (Neon):** ทุกอย่างที่มีความสัมพันธ์และต้องการ transaction ACID
- **MongoDB (Atlas M0):** `audit_logs` (before/after เป็นเอกสารยืดหยุ่น) และ `notifications` (มี TTL 90 วัน และ unique `dedupeKey`)
- เขียน Mongo **หลัง** commit PostgreSQL สำเร็จ ถ้าเขียนไม่ได้ให้ log error และไม่ rollback

## ผลที่ตามมา

- ✅ แต่ละฐานทำงานที่ถนัด และอธิบายในสัมภาษณ์ได้ว่าเลือกเพราะอะไร
- ✅ audit log ไม่ทำให้ตารางธุรกรรมโตเร็ว
- ⚠️ audit log อาจหายได้ถ้า Mongo ล่มหลัง commit → ยอมรับได้สำหรับ MVP · ทางแก้ระยะยาวคือ outbox pattern (backlog)
- ⚠️ integration test ต้องเปิด container ทั้งสองฐาน

## ทางเลือกที่ไม่เลือก

- **ทุกอย่างใน PostgreSQL (jsonb สำหรับ audit):** ง่ายกว่า แต่ไม่ตรงโจทย์ที่ต้องฝึก MongoDB
- **สต็อกใน MongoDB:** ต้องทำ transaction ข้าม collection ทำให้ซับซ้อนและเสี่ยงยอดไม่ตรง
