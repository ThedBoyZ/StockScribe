# Screen Inventory

> เจ้าของ: **UX/UI** · Review: **BA, PO, SA** · อัปเดตล่าสุด: 17 พ.ย. 2026

ตารางนี้ตอบ 3 คำถาม: **มีหน้าจออะไรบ้าง · แต่ละ AC ถูกออกแบบไว้ที่หน้าไหน · อะไรยังขาดหรือขัดกับ AC (D-1..D-6)**

## 1. รายการหน้าจอ

สถานะ: `Final` = ใช้เป็นตัวจริง · `Draft` = ยังรอตัดสินใจ (ดู D-N) · `—` = ยังไม่มีภาพใน [screens/](screens/)

### Mobile — พนักงานสาขา (StoreStaff) · 360–430px

| รหัส | หน้าจอ | US | ภาพ | Template | สถานะ |
| --- | --- | --- | --- | --- | --- |
| M00-A | หน้าหลัก | — | [png](screens/mobile/M00-A_home.png) | [html](template-html/mobile/M00-A_home/code.html) | Final |
| M01-A | สร้างใบขอซื้อ (PR) | US-004, US-006 | [png](screens/mobile/M01-A_US004_create-pr.png) | [html](template-html/mobile/M01-A_US004_create-pr/code.html) | Final |
| M01-B | สร้าง PR — กรอกผิด | US-004 | [png](screens/mobile/M01-B_US004_unit-error.png) | [html](template-html/mobile/M01-B_US004_unit-error/code.html) | Final |
| M01-C | สร้าง PR — รายการแนะนำ / เกินโควตา | US-006 | [png](screens/mobile/M01-C_US006_quota-suggest.png) | [html](template-html/mobile/M01-C_US006_quota-suggest/code.html) | Draft (D-5) |
| M01-D | ส่ง PR สำเร็จ | US-004 | [png](screens/mobile/M01-D_US004_submitted.png) | [html](template-html/mobile/M01-D_US004_submitted/code.html) | Final |
| M02-A | รายการ PO รอรับ | US-001 | [png](screens/mobile/M02-A_US001_po-list.png) | [html](template-html/mobile/M02-A_US001_po-list/code.html) | Final |
| M02-B | รับของตาม PO | US-001 | [png](screens/mobile/M02-B_US001_receive.png) | [html](template-html/mobile/M02-B_US001_receive/code.html) | Final |
| M02-C | รับของ — validation | US-001 | [png](screens/mobile/M02-C_US001_validation.png) | [html](template-html/mobile/M02-C_US001_validation/code.html) | Final |
| M02-D | ใบรับของ (GRN) สำเร็จ | US-001 | [png](screens/mobile/M02-D_US001_receipt.png) | [html](template-html/mobile/M02-D_US001_receipt/code.html) | Final |
| M03-A | สต็อกสาขา | US-003 | — | [html](template-html/mobile/M03-A_US003_stock-inventory/code.html) | Draft |
| M03-B | รายละเอียดสินค้า (แยกล็อต) | US-001, US-003 | — | [html](template-html/mobile/M03-B_US003_product-detail/code.html) | Draft |
| M03-C | ปรับสต็อก (bottom sheet) | US-003 | — | [html](template-html/mobile/M03-C_US003_stock-adjust/code.html) | Draft (D-2) |

### Web — ผู้จัดการสาขา / ฝ่ายจัดซื้อ / เจ้าของ · Desktop 1440px

| รหัส | หน้าจอ | ผู้ใช้ | US | ภาพ | Template | สถานะ |
| --- | --- | --- | --- | --- | --- | --- |
| W00-A | หน้าหลัก (ภาพรวมสต็อก) | ทุก role | US-001, US-003 | [png](screens/web/W00-A_home.png) | [html](template-html/web/W00-A_home/code.html) | Final |
| W01-A | รวมคำสั่งซื้อ | Purchasing, Owner | US-005 | [png](screens/web/W01-A_US005_consolidate.png) | [html](template-html/web/W01-A_US005_consolidate/code.html) · [alt](template-html/web/W01-A_US005_consolidate-alt/code.html) | Final |
| W01-B | รวมคำสั่งซื้อ — เพิ่มสินค้าแนะนำแล้ว | Purchasing | US-005 | [png](screens/web/W01-B_US005_suggest-added.png) | [html](template-html/web/W01-B_US005_suggest-added/code.html) | Final |
| W01-C | เลือกผู้ขาย (modal) | Purchasing | US-005 | [png](screens/web/W01-C_US005_vendor-modal.png) | [html](template-html/web/W01-C_US005_vendor-modal/code.html) | Final |
| W01-D | สร้างร่าง PO สำเร็จ (modal) | Purchasing | US-005 | — | [html](template-html/web/W01-D_US005_po-created/code.html) | Draft |
| W02-A | อนุมัติ PO — ในวงเงิน | BranchManager | US-002, US-003 | [png](screens/web/W02-A_US002_in-limit.png) | [html](template-html/web/W02-A_US002_in-limit/code.html) | Final (D-1) |
| W02-B | อนุมัติ PO — เกินวงเงิน | BranchManager | US-002 | [png](screens/web/W02-B_US002_over-limit.png) | [html](template-html/web/W02-B_US002_over-limit/code.html) | Final (D-1) |
| W02-C | คูปองโปรโมชั่น / ล้างสต็อก (modal) | BranchManager | — | [png](screens/web/W02-A_US002_in-limit-1.png) ⚠️ | [html](template-html/web/W02-C_clearance-coupon-modal/code.html) | Draft (D-3) |
| W03-A | ส่ง PDF ทาง LINE | Purchasing | — | [png](screens/web/W03-A_export-pdf-line.png) | [html](template-html/web/W03-A_export-pdf-line/code.html) | Draft (D-4) |
| *(ไม่มี)* | เจ้าของอนุมัติ PO เกินวงเงิน | Owner | US-002 | — | — | ยังไม่ทำ (D-6) |

> ⚠️ **ชื่อไฟล์ผิด:** ภาพ W02-C ตอนนี้ชื่อ `screens/web/W02-A_US002_in-limit-1.png` และป้าย `web-label/W02-D.png` แต่ในป้ายเขียน "W02-C" → ควรเปลี่ยนเป็น `W02-C_clearance-coupon-modal.png` และ `web-label/W02-C.png`

## 2. Coverage — AC ไหนอยู่หน้าไหน

✅ = มีหน้าจอ + หมุดโน้ต · 🟡 = มีบนหน้าจอแต่ต่างจาก AC หรือยังไม่ครบ · ❌ = ยังไม่มีหน้าจอ · ⚪ = ไม่ต้องมีหน้าจอ (backend / job)

### US-001 รับของไม่ครบตาม PO — ✅ 8/8

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | รับครบ → Closed | M02-D_02 | ✅ |
| 2 | รับบางส่วน → "ค้างส่ง N" | M02-A_02, M02-D_02 | ✅ |
| 3 | รับรอบสอง → GRN 2 ใบ | M02-D_01 | ✅ |
| 4 | รับเกิน → 422 | M02-B_01, M02-C_01 | ✅ |
| 5 | ล็อต / วันหมดอายุบังคับ | M02-B_02, M02-C_02, M03-B_01 | ✅ |
| 6 | สาขาอื่น → 403 | M02-A_02, M02-A_03 | ✅ |
| 7 | 360px, ปุ่ม ≥ 44px | M02-B_04 | ✅ |
| 8 | ค้างส่งเกิน 7 วัน ยกเลิกอัตโนมัติ | M02-A_04, W00-A_03 | ✅ |

### US-002 อนุมัติ PO ตามวงเงิน — 🟡 4/5

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | ≤ 5,000 อนุมัติได้ + audit log | W02-A_01, W02-A_03 | ✅ |
| 2 | 5,000 พอดีอนุมัติได้ | W02-A_01 | ✅ |
| 3 | > 5,000 ไม่เห็นปุ่ม, 403 | W02-B_01 | ✅ |
| 4 | เจ้าของอนุมัติยอดเกิน | W02-B_02 | ❌ D-6 |
| 5 | แก้ยอดหลังอนุมัติ → Draft | W02-A_04 | ✅ |

### US-003 แจ้งเตือนสินค้าใกล้หมดอายุ — 🟡 6/9

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | การ์ด "ใกล้หมดอายุ" บน Dashboard | W02-A (ส่วนล่าง) | 🟡 อยู่ในหน้าอนุมัติ ไม่ใช่ Dashboard |
| 2 | ขอบเขต 3 วัน | W02-A_05 | 🟡 D-1 |
| 3 | ไม่แสดงล็อต qty = 0 | M03-A_01 | ✅ |
| 4 | กลุ่ม "หมดอายุแล้ว" + ปุ่มทิ้ง | W02-A_06 | ✅ (ไม่มีหน้าจอหลังกดปุ่ม) |
| 5 | เรียงวันใกล้สุด + มูลค่ารวม | M03-A_01, W02-A_05 | ✅ |
| 6 | แจ้งเตือน 07:00 | — | ❌ ไม่มีหน้าจอ notification |
| 7 | สิทธิ์ตามสาขา / Owner เห็นทุกสาขา + กรอง | W00-A_02 | ✅ |
| 8 | ตั้งจำนวนวันได้ (Admin) | — | ❌ ไม่มีหน้าตั้งค่า |
| 9 | พนักงานเห็นแบบอ่านอย่างเดียว | M03-A_02 | 🟡 D-2 |

### US-004 เลือกหน่วยใน PR — 🟡 5/9

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | เลือกหน่วยจากรายการ | M01-A_01 | ✅ |
| 2 | แสดง "= 60 ขวด" | M01-A_02 | ✅ |
| 3 | ฝ่ายจัดซื้อเห็น "ขอ 30 ขวด → สั่ง 2 ลัง" | — | ❌ ไม่มีหมุดใน W01 |
| 4 | ลดจำนวน แสดง "ขาด N" | — | ❌ |
| 5 | จำนวนต้องเป็นบวก | M01-B_01 | ✅ |
| 6 | ป้าย "ยังไม่ตั้งหน่วยซื้อ" | — | ❌ |
| 7 | เก็บค่าตามจริง | M01-D_01 | ✅ |
| 8 | 360px | M01-A_05 | ✅ |
| 9 | Admin / ฝ่ายจัดซื้อแก้หน่วย + audit log | — | ❌ ไม่มีหน้าจัดการหน่วย |

### US-005 รวม PR เป็น PO — ✅ 10/10

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | เฉพาะ PR Approved | W01-A_01 | ✅ |
| 2 | แยกตามผู้ขาย → Draft | W01-D_01 | ✅ |
| 3 | ยอดแยกสาขา | W01-A_02 | ✅ |
| 4 | PR → Converted | W01-D_01 | ✅ |
| 5 | ไม่มีผู้ขาย | W01-A_05 | ✅ |
| 6 | เปลี่ยนผู้ขาย | W01-C_01 | ✅ |
| 7 | ยอดขั้นต่ำเตือน | W01-A_03 | ✅ |
| 8 | แก้ร่าง | W01-A_04 | ✅ |
| 9 | สิทธิ์ Purchasing / Owner | W01-A_01 | ✅ |
| 10 | ลบบรรทัด → กลับไปรอรวม | W01-A_04 | ✅ |

### US-006 สร้าง PR จาก reorder point — 🟡 4/8

| AC | เรื่อง | หน้าจอ | |
| --- | --- | --- | --- |
| 1 | ปุ่ม "สร้าง PR จากของใกล้หมด" | M01-A, M01-C | 🟡 ไม่มีปุ่มแยก รายการแนะนำฝังในหน้าสร้าง PR |
| 2 | สูตรจำนวนแนะนำ | M01-C_01 | ✅ |
| 3 | ขอบเขต = reorder point ไม่แสดง | — | ⚪ backend |
| 4 | นับยอดค้างสั่ง | M01-C_02 | ✅ |
| 5 | ติ๊กเลือก / แก้จำนวน ก่อนสร้าง | M01-A_03 | 🟡 มี "เพิ่มทั้งหมด" แต่ไม่มีติ๊กเลือกรายตัว |
| 6 | ได้ร่าง PR ไม่ส่งเอง | M01-A_04 | 🟡 design เพิ่มเข้า PR ที่กำลังทำ ไม่ได้สร้างร่างแยก |
| 7 | รายงานสินค้าไม่ตั้งจุดสั่งซื้อ (Admin) | — | ❌ |
| 8 | เห็นเฉพาะสาขาตัวเอง | M00-A_02 | ✅ |

### สรุป coverage

| Story | ✅ | 🟡 | ❌ | ⚪ |
| --- | --- | --- | --- | --- |
| US-001 | 8 | 0 | 0 | 0 |
| US-002 | 4 | 0 | 1 | 0 |
| US-003 | 4 | 3 | 2 | 0 |
| US-004 | 5 | 0 | 4 | 0 |
| US-005 | 10 | 0 | 0 | 0 |
| US-006 | 3 | 3 | 1 | 1 |
| **รวม 49 AC** | **34** | **6** | **8** | **1** |

หน้าจอที่ขาดส่วนใหญ่เป็น **หน้า Admin / ตั้งค่า** (US-003 AC8, US-004 AC9, US-006 AC7) และ **มุมมองฝ่ายจัดซื้อเรื่องหน่วย** (US-004 AC3, 4, 6) → เสนอทำเป็นกลุ่ม W04 (ตั้งค่า) และหมุดเพิ่มใน W01-A

## 3. Design gaps (D-1..D-6)

| ID | หน้าจอ | ปัญหา | ข้อเสนอ | ใครตัดสิน | สถานะ |
| --- | --- | --- | --- | --- | --- |
| **D-1** | W02-A, W02-B, W02-C | หัวส่วนใกล้หมดอายุเขียน **"ใกล้หมดอายุ 7 วัน"** แต่ US-003 AC2 กำหนด 3 วัน (นับวันนี้เป็นวันที่ 0) | แก้ข้อความเป็น "ใกล้หมดอายุ 3 วัน" และอ่านค่าจาก `ExpiryAlertDays` | UX | 🔧 รอแก้ |
| **D-2** | M03-C | bottom sheet ปรับสต็อกบนมือถือของพนักงาน ขัดกับ US-003 AC9 (พนักงานไม่มีปุ่มปรับสต็อก) แต่ [stakeholders](../ba/stakeholders.md) ให้ StoreStaff ปรับสต็อกได้ | (ก) ย้ายเป็นของผู้จัดการ หรือ (ข) เปลี่ยนเป็น "แจ้งของเสีย" ให้ผู้จัดการยืนยัน | BA + Owner | ❓ รอ BA |
| **D-3** | W02-C | modal คูปอง / ล้างสต็อก (ลดราคา, "ได้คืน ฿256") ไม่มี AC และ US-003 ระบุ **"ตั้งราคาโปรลดราคาในระบบ" เป็น out of scope** | ติดป้าย "แนวคิดเพิ่มเติม" ไม่เข้า MVP หรือเปิด story ใหม่ใน backlog | PO | ❓ รอ PO |
| **D-4** | W03-A | หน้าส่ง PDF ทาง LINE ไม่มี US (US-005 out of scope อ้างถึง "story ส่ง PDF" แต่ยังไม่มีไฟล์) | เปิด **US-007 ส่ง PO ให้ผู้ขาย (PDF ทาง LINE)** | BA | ❓ รอ BA |
| **D-5** | M01-C | ข้อความ **"เกินโควตาสาขา 480 ขวดต่อรอบ (10 ลัง)"** ไม่มีใน AC / glossary และ HTML พูดถึง "ส่งคำขออนุมัติพิเศษต่อผู้จัดการเขต" ซึ่งไม่มี role นี้ | ตัดออก หรือนิยาม "โควตา" (เท่ากับ max stock?) เพิ่มใน US-006 + glossary | BA + Owner | ❓ รอ BA |
| **D-6** | — | ยังไม่มีหน้าจอเจ้าของอนุมัติ PO เกินวงเงิน (US-002 AC4) | ทำ W02-D: หน้าเดียวกับ W02-B แต่มีปุ่มอนุมัติ / ปฏิเสธ สำหรับ Owner | UX | 🔧 รอทำ |

## 4. ผลกระทบต่อเอกสาร BA (ส่งให้ BA)

สิ่งที่ design ทำให้รู้ว่า AC ต้อง **แก้** หรือ **เพิ่ม** — รอ BA ตัดสินแล้วอัปเดตใน [docs/ba](../ba/README.md)

### ต้องแก้ AC (design ต่างจาก AC)

| Story | AC | ตอนนี้ | ข้อเสนอ | ที่มา |
| --- | --- | --- | --- | --- |
| US-003 | 1 | "เปิด **Dashboard** เห็นการ์ดใกล้หมดอายุ" | ระบุหน้าให้ชัด: การ์ดอยู่ในหน้าหลัก W00-A และหน้าผู้จัดการ W02-A หรือแก้ design ให้มีบน W00-A ด้วย | W02-A |
| US-003 | 9 | พนักงานไม่มีปุ่มปรับสต็อก | ระบุว่าห้ามเฉพาะในรายการใกล้หมดอายุ หรือห้ามทั้งหมด (ขัดกับ stakeholders ที่ให้ StoreStaff ปรับสต็อกได้) | D-2 |
| US-006 | 1, 5, 6 | ปุ่มแยก "สร้าง PR จากของใกล้หมด" → ติ๊กเลือก → "สร้างร่าง PR" | design ฝังรายการแนะนำในหน้าสร้าง PR + ปุ่ม "เพิ่มทั้งหมด" → แก้ AC ให้ตรง design หรือแก้ design ให้มีติ๊กเลือกรายตัว | M01-A_03 |

### ต้องเพิ่ม AC (design มี แต่ AC ไม่มี)

| Story | AC ใหม่ที่เสนอ | ที่มา |
| --- | --- | --- |
| US-001 | **บันทึกไม่สำเร็จ** — Given เน็ตหลุดหรือ API error, When กดบันทึกรับของ, Then อยู่หน้าเดิม ข้อมูลที่กรอกไม่หาย แสดง toast ให้ลองใหม่ | M02-B_04 |
| US-001 | **ค้างส่ง = สั่ง − รับแล้ว** คิดในหน่วยซื้อของ PO (เพิ่มสูตรใน glossary `OutstandingQty`) | M02-B_03 |
| US-002 | **ส่งต่อเจ้าของ** — Given PO > 5,000, When ผู้จัดการกด "ส่งต่อเจ้าของร้านอนุมัติ", Then สถานะเป็น `PendingApproval` (แสดง "รอเจ้าของอนุมัติ") ออกจากคิวผู้จัดการ และเจ้าของได้รับแจ้งเตือน | W02-B_03 |
| US-002 | **ยอดรวม = Σ (จำนวน × ราคาต่อหน่วย) ไม่รวม VAT** (เพิ่มใน glossary) | W02-A_02 |
| US-002 | **อนุมัติแล้วไปใบถัดไป** — หลังอนุมัติ แสดง toast และเปิด PO ใบถัดไปในคิว | W02-A_03 |
| US-004 | **error ไม่ล้างค่า** — กรอกผิดแสดงข้อความใต้ช่องที่ผิด ค่าที่กรอกไว้ไม่หาย | M01-B_02 |
| US-005 | **เพิ่มสินค้าแนะนำในร่าง PO** — ฝ่ายจัดซื้อเพิ่มสินค้าแนะนำเข้าร่าง PO ได้ บรรทัดใหม่มีป้าย "เพิ่งเพิ่ม" ยอดคำนวณใหม่ทันที และเลิกทำได้ | W01-B_01 |
| US-006 | **นิยามโควตา** (ถ้าเก็บ D-5) — จำนวนสูงสุดต่อรอบต่อสาขา, เกินแล้วทำอะไร | D-5 |

### ต้องเพิ่ม story / เอกสาร

| อะไร | เหตุผล | ที่มา |
| --- | --- | --- |
| **US-007 ส่ง PO ให้ผู้ขาย (PDF ทาง LINE)** — ส่งได้เฉพาะ Approved, ส่งแล้วเป็น Sent | มีหน้าจอ W03-A แล้วแต่ไม่มี story · CHANGELOG ระบุ "ส่ง PO ทาง LINE (PDF) แทนอีเมล" | D-4, W03-A_02 |
| glossary: `OutstandingQty` สูตร, ยอดรวมก่อน VAT, โควตา (ถ้าเก็บ) | หมุดอ้าง "glossary · SA ยืนยัน" แต่ glossary ยังไม่มีสูตร | M02-B_03, W02-A_02 |
| stakeholders: role "ผู้จัดการเขต" | HTML ของ M01-C / W02-A / W02-C มีข้อความ "ส่งต่อให้ผู้จัดการเขต" แต่ไม่มี role นี้ → ลบออกจาก design หรือเพิ่ม role | D-5 |
| problem-statement: ความกว้าง desktop | ข้อกำหนดเขียน ≥ 1280px แต่ design ทำที่ 1440px → ทดสอบที่ 1280px ด้วย | screens/web |
| backlog: คูปอง / ล้างสต็อกใกล้หมดอายุ | W02-C อยู่นอก scope ของ US-003 | D-3 |
