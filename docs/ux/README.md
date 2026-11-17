# เอกสาร UX/UI

> เจ้าของ: **UX/UI** · Review: **BA, PO, SA** · สถานะเฟส: 🚧 **กำลังทำ (wireflow + hi-fi เสร็จ, รอแก้ D-1..D-6)**

โฟลเดอร์นี้เก็บทุกอย่างที่ตอบคำถาม **"หน้าจอหน้าตาเป็นอย่างไร และผู้ใช้เดินทางผ่านหน้าจอไหนบ้าง"** โดยรับ input จาก [docs/ba](../ba/README.md) (user stories, to-be flow, ข้อกำหนดการใช้งาน)

## ลิงก์

| อะไร | ลิงก์ |
| --- | --- |
| Figma (wireflow + annotation) | `TODO: ใส่ลิงก์ Figma` |
| Google Stitch (สร้าง hi-fi) | `TODO: ใส่ลิงก์โปรเจกต์ Stitch` |
| User stories ที่อ้างอิง | [docs/ba/user-stories](../ba/user-stories/) |

## ลำดับการอ่าน

| ลำดับ | ไฟล์ / โฟลเดอร์ | ตอบคำถาม |
| --- | --- | --- |
| 1 | [screen-inventory.md](screen-inventory.md) | มีหน้าจออะไรบ้าง ครอบคลุม AC ไหน และยังขาดอะไร (D-1..D-6) |
| 2 | [screens/](screens/) | ภาพหน้าจอ hi-fi ที่ใช้เป็นตัวจริง |
| 3 | [annotations.md](annotations.md) | โน้ตทุกหมุดบนหน้าจอ อ้างอิง AC หรือ UX |
| 4 | [annotated/](annotated/) | ภาพหมุดโน้ตที่ export จาก Figma |
| 5 | [DESIGN.md](DESIGN.md) | ชุดสี Lilac Dusk, ฟอนต์, ระยะ, component |
| 6 | [template-html/](template-html/) | HTML + screenshot ที่ export จาก Stitch (ใช้อ้างอิงตอน dev) |
| 7 | [stitch-prompts.md](stitch-prompts.md) | prompt ที่ใช้สร้างแต่ละหน้าใน Stitch |

## โครงสร้างโฟลเดอร์

```
docs/ux/
├─ README.md               ไฟล์นี้
├─ DESIGN.md               design system (Lilac Dusk)
├─ screen-inventory.md     ตารางหน้าจอ + coverage + D-1..D-6
├─ annotations.md          โน้ตทุกหมุด
├─ stitch-prompts.md       prompt ของ Stitch
├─ screens/
│  ├─ mobile/              M00-A … M02-D (.png) + mobile-label/ ป้ายชื่อหน้าจอ
│  └─ web/                 W00-A … W03-A (.png) + web-label/ ป้ายชื่อหน้าจอ
├─ annotated/
│  ├─ mobile-annotated/    หมุดโน้ตของหน้า M (.png)
│  └─ web-annotated/       หมุดโน้ตของหน้า W (.png)
└─ template-html/
   ├─ mobile/<รหัส>/       code.html + screen.png ต่อหน้าจอ
   └─ web/<รหัส>/          code.html + screen.png ต่อหน้าจอ
```

## กติกาตั้งชื่อ

| ชนิด | รูปแบบ | ตัวอย่าง |
| --- | --- | --- |
| รหัสหน้าจอ | `<M\|W><กลุ่ม 2 หลัก>-<สถานะ A-Z>` | `M01-B` = mobile, กลุ่ม 01 (สร้าง PR), สถานะ B (error) |
| ไฟล์หน้าจอ / โฟลเดอร์ template | `<รหัส>_<US>_<รายละเอียด>` | `M02-A_US001_po-list` |
| หมุดโน้ต | `<รหัส>_<ลำดับ 2 หลัก>_<รายละเอียด>.png` | `M01-A_01_unit-select.png` |

- **M** = mobile (กว้าง 360–430px) สำหรับพนักงานสาขา · **W** = web (desktop 1440px) สำหรับผู้จัดการ / ฝ่ายจัดซื้อ / เจ้าของ
- กลุ่ม `00` = หน้าหลัก, สถานะ `A` = สถานะปกติ, `B`, `C`, … = error / modal / ขั้นถัดไป
- หน้าจอที่ยังไม่มี user story รองรับ ใช้ `D-N` (Design gap) ในตาราง [screen-inventory.md](screen-inventory.md#3-design-gaps-d-1d-6)

## ส่งต่อเฟสถัดไป

| ส่งให้ | ใช้อะไร |
| --- | --- |
| **BA** | D-1..D-6 และ "ผลกระทบต่อ AC" ใน [screen-inventory.md](screen-inventory.md) → แก้/เพิ่ม AC |
| **SA** | annotations ที่อ้าง API (403 / 422), สูตรคำนวณ, สถานะเอกสาร |
| **Dev** | template-html (โครง Tailwind + token), DESIGN.md |
| **QA** | annotations + coverage table → test case ระดับ UI |

## กติกา

- แก้หน้าจอใน Figma / Stitch แล้ว export ทับไฟล์เดิมชื่อเดิม ถ้าเพิ่มสถานะใหม่ให้ใช้ตัวอักษรถัดไป
- หมุดโน้ตทุกอันต้องมี **ที่มา** (`US-00X ACn`, `UX`, `glossary`, หรือ `screen-inventory`) และต้องมีแถวใน [annotations.md](annotations.md)
- เนื้อหาบนหน้าจอเป็น **ข้อมูลสมมติ** (ชื่อสินค้า, เลข PO, ยอดเงิน) ใช้ตัวเลขให้ตรงกับ AC เมื่อทำได้
