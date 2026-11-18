# ADR-0002 · Modular monolith แบ่งโฟลเดอร์ตามฟีเจอร์

- สถานะ: Accepted · 17 พ.ย. 2026

## บริบท

ทีมมีคนเดียว VM มี RAM 1 GB และต้องการให้คนอ่านโค้ดแล้วหาเจอจากชื่อ user story ได้เร็ว

## ตัดสินใจ

- API เป็น **process เดียว** (ASP.NET Core .NET 10) job เบื้องหลังรันใน process เดียวกันด้วย `BackgroundService`
- โครงโค้ดแบ่งตาม **ฟีเจอร์** (`Features/PurchaseOrders/Approve`) แต่ละโฟลเดอร์มี endpoint, request/response, validator และ handler
- กฎธุรกิจอยู่ใน `Domain/` เป็นคลาสธรรมดา ไม่อ้าง EF หรือ HTTP เพื่อให้ unit test ได้ตรงๆ
- ใช้ Minimal API + `MapGroup` ต่อฟีเจอร์ · ไม่ใช้ MediatR

## ผลที่ตามมา

- ✅ เพิ่มฟีเจอร์ใหม่แตะไม่กี่ไฟล์ · PR หนึ่งอันต่อหนึ่ง US อ่านง่าย
- ✅ ใช้ RAM น้อย deploy เป็น image เดียว
- ⚠️ ต้องระวังไม่ให้ฟีเจอร์เรียก DbContext ของกันมั่วๆ → ใช้กฎ "ข้ามฟีเจอร์ผ่าน Domain หรือ query service เท่านั้น"

## ทางเลือกที่ไม่เลือก

- **Clean Architecture 4 โปรเจกต์ (Api/Application/Domain/Infrastructure):** เป็นระเบียบ แต่ไฟล์และ boilerplate มากเกินสำหรับทีมคนเดียว
- **Microservices:** เกินความจำเป็น และ VM ฟรีรับไม่ไหว
