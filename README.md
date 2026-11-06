# เอกสาร BA (Business Analysis)

> เจ้าของ: **BA** · ผู้ร่วม: Product Owner, SA, Developer

โฟลเดอร์นี้เก็บทุกอย่างที่ตอบคำถาม **"ทำไปเพื่ออะไร และต้องทำอะไร"** ก่อนเริ่มออกแบบและเขียนโค้ด

## ลำดับการอ่าน

| ลำดับ | ไฟล์ | ตอบคำถาม |
| --- | --- | --- |
| 1 | [problem-statement.md](problem-statement.md) | ปัญหาธุรกิจคืออะไร วัดผลยังไง |
| 2 | [stakeholders.md](stakeholders.md) | ใครเกี่ยวข้องบ้าง ใครตัดสินใจ |
| 3 | [glossary.md](glossary.md) | PR, PO, GRN, FEFO คืออะไร |
| 4 | [interview-guide.md](interview-guide.md) | จะถามอะไร ถามยังไง |
| 5 | [interviews/](interviews/) | บันทึกสัมภาษณ์แต่ละครั้ง |
| 6 | [pain-backlog.md](pain-backlog.md) | ปัญหาทั้งหมดเรียงตามความสำคัญ |
| 7 | [business-flow/](business-flow/) | flow ตอนนี้ (As-Is) และ flow ใหม่ (To-Be) |
| 8 | [user-stories/](user-stories/) | requirement ที่พร้อมให้ทีมพัฒนา |

## สถานะ User Stories

| ID | ชื่อ | Pain | ลำดับ | สถานะ |
| --- | --- | --- | --- | --- |
| US-001 | รับของไม่ครบตาม PO | #1 | Must | Ready |
| US-002 | อนุมัติ PO ตามวงเงิน | #2 | Must | Ready (มีคำถามค้าง) |
| US-003 | แจ้งเตือนสินค้าใกล้หมดอายุ | #3 | Must | แบบฝึกหัด — Draft |
| US-004 | เลือกหน่วยใน PR และแปลงเป็นหน่วยซื้อ | #4 | Must | แบบฝึกหัด — Draft |
| US-005 | รวม PR หลายสาขาเป็น PO แยกตามผู้ขาย | #5 | Must | แบบฝึกหัด — Draft |
| US-006 | สร้าง PR อัตโนมัติจากของต่ำกว่า reorder point | #8 | Should | แบบฝึกหัด — Draft |

สถานะ: `Draft` → `Ready` (ผ่าน checklist + ไม่มีคำถามค้าง) → `In progress` → `Done`

## กติกา

- บันทึกสัมภาษณ์ใหม่ทุกครั้งใช้ [interviews/_template.md](interviews/_template.md)
- User Story ใหม่ใช้ [user-stories/_template.md](user-stories/_template.md) และต้องอ้างอิงที่มา (บันทึกสัมภาษณ์ข้อไหน)
- Story ที่สถานะ `Ready` ให้เปิดเป็น GitHub Issue ด้วย template **User Story** แล้วลิงก์กลับมาที่ไฟล์นี้
- ข้อมูลในบันทึกสัมภาษณ์ปัจจุบันเป็น **ข้อมูลสมมติจากการซ้อม** ถ้าได้คุยกับคนจริงให้เพิ่มเป็นบันทึกใหม่
