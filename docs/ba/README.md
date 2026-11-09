# เอกสาร BA (Business Analysis)

> เจ้าของ: **BA** · ผู้ร่วม: Product Owner, SA, Developer · สถานะเฟส: ✅ **Discovery เสร็จ (docs-v0.1)**

โฟลเดอร์นี้เก็บทุกอย่างที่ตอบคำถาม **"ทำไปเพื่ออะไร และต้องทำอะไร"** ก่อนเริ่มออกแบบและเขียนโค้ด

## ลำดับการอ่าน

| ลำดับ | ไฟล์ | ตอบคำถาม |
| --- | --- | --- |
| 1 | [problem-statement.md](problem-statement.md) | ปัญหาธุรกิจคืออะไร วัดผลยังไง |
| 2 | [stakeholders.md](stakeholders.md) | ใครเกี่ยวข้องบ้าง ใครตัดสินใจ |
| 3 | [glossary.md](glossary.md) | PR, PO, GRN, FEFO คืออะไร |
| 4 | [interview-guide.md](interview-guide.md) | จะถามอะไร ถามยังไง |
| 5 | [interviews/](interviews/) | บันทึกสัมภาษณ์ #001–#003 |
| 6 | [pain-backlog.md](pain-backlog.md) | ปัญหาทั้งหมดเรียงตามความสำคัญ |
| 7 | [business-flow/](business-flow/) | flow ตอนนี้ (As-Is) และ flow ใหม่ (To-Be) |
| 8 | [user-stories/](user-stories/) | requirement ที่พร้อมให้ทีมพัฒนา |

## สถานะ User Stories

| ID | ชื่อ | Pain | ลำดับ | ขนาด | สถานะ | Issue |
| --- | --- | --- | --- | --- | --- | --- |
| [US-001](user-stories/US-001-partial-goods-receipt.md) | รับของไม่ครบตาม PO | P-01 | Must | M | Ready | # |
| [US-002](user-stories/US-002-po-approval-limit.md) | อนุมัติ PO ตามวงเงิน | P-02 | Must | S | Ready | # |
| [US-003](user-stories/US-003-expiry-alert.md) | แจ้งเตือนสินค้าใกล้หมดอายุ | P-03 | Must | M | Ready | # |
| [US-004](user-stories/US-004-unit-conversion.md) | เลือกหน่วยใน PR และแปลงเป็นหน่วยซื้อ | P-04 | Must | M | Ready | # |
| [US-005](user-stories/US-005-consolidate-pr-to-po.md) | รวม PR หลายสาขาเป็น PO แยกตามผู้ขาย | P-05 | Must | L (แตก a/b) | Ready | # |
| [US-006](user-stories/US-006-auto-pr-reorder-point.md) | สร้าง PR อัตโนมัติจากสินค้าต่ำกว่า reorder point | P-08 | Should | M | Ready | # |

สถานะ: `Draft` → `Ready` (ผ่าน checklist + ไม่มีคำถามค้างที่ block) → `In progress` → `Done`

## ลำดับพัฒนาที่แนะนำ

```
US-004 (หน่วย) ─┐
                ├─► US-005 (รวมเป็น PO) ─► US-002 (อนุมัติ) ─► US-001 (รับของ) ─► US-003 (หมดอายุ)
US-006 (PR อัตโนมัติ, Should) ทำหลังสุด
```

เหตุผล: US-001 ต้องมี PO ก่อน, US-003 ต้องมีล็อตจาก US-001, US-005 ต้องมีหน่วยซื้อจาก US-004

## คำถามที่ clarify แล้ว (9 พ.ย. 2026)

| เรื่อง | Story | คำตอบ | ตรงกับที่ตัดสินชั่วคราว? | Issue |
| --- | --- | --- | --- | --- |
| ของขาดรอรอบหน้า หรือยกเลิก | US-001 | รอไม่เกิน 7 วัน แล้วยกเลิกอัตโนมัติ | ❌ เพิ่ม AC 8 | # |
| วงเงิน 5,000 คิดต่อใบหรือต่อวัน | US-002 | ต่อใบ ก่อน VAT | ✅ | # |
| พนักงานเห็นรายการใกล้หมดอายุไหม | US-003 | เห็น แบบอ่านอย่างเดียว | ✅ เพิ่ม AC 9 | # |
| ขนมปังเตือนน้อยกว่า 3 วันไหม | US-003 | ใช้ 3 วันไปก่อน | ✅ | # |
| ใครทิ้งล็อตที่หมดอายุ | US-003 | ผู้จัดการสาขา | ✅ | # |
| ปัดขึ้นเป็นหน่วยซื้อเสมอไหม | US-004 | ปัดขึ้น | ✅ | # |
| ใครแก้ตัวคูณหน่วย | US-004 | Admin + ฝ่ายจัดซื้อ | ❌ เพิ่ม AC 9 | # |
| ผู้ขายส่งแยกสาขาหรือส่งรวม | US-005 | ส่งแยกทุกเจ้า → ปลด blocked | ✅ | # |
| ยอดไม่ถึงขั้นต่ำ ห้ามหรือเตือน | US-005 | เตือน | ✅ | # |
| ลบบรรทัดออกจากร่าง PO ได้ไหม | US-005 | ได้ กลับไปรอรอบหน้า | (ใหม่) เพิ่ม AC 10 | # |
| จำนวนแนะนำถึง max stock ไหม | US-006 | ใช่ | ✅ | # |

ไม่มีคำถามค้าง ทุก story พร้อมพัฒนา

## ส่งต่อเฟสถัดไป

| ส่งให้ | ใช้อะไร |
| --- | --- |
| **UX/UI** | stakeholders, interviews (ทำ persona), to-be flow (ทำ wireflow), ข้อกำหนดการใช้งานใน problem-statement |
| **SA** | glossary (ชื่อในโค้ด + สถานะเอกสาร), กฎธุรกิจ BR-01..08 ใน to-be, AC + Tasks ของทุก story |
| **QA** | AC ของทุก story (แปลงเป็น test case) |

## กติกา

- บันทึกสัมภาษณ์ใหม่ทุกครั้งใช้ [interviews/_template.md](interviews/_template.md)
- User Story ใหม่ใช้ [user-stories/_template.md](user-stories/_template.md) และต้องอ้างอิงที่มา
- Story ที่สถานะ `Ready` ให้เปิดเป็น GitHub Issue ด้วย template **User Story** แล้วใส่เลข issue ในตารางด้านบน
- ข้อมูลในบันทึกสัมภาษณ์ปัจจุบันเป็น **ข้อมูลสมมติจากการซ้อม** ถ้าได้คุยกับคนจริงให้เพิ่มเป็นบันทึกใหม่
