# Annotations (โน้ตทุกหมุด)

> เจ้าของ: **UX/UI** · Review: **BA, SA, QA** · ภาพหมุด: [annotated/](annotated/) · รายการหน้าจอ: [screen-inventory.md](screen-inventory.md)

หมุดแต่ละอันอธิบาย 1 จุดบนหน้าจอ: **ส่วนไหน → ทำงานอย่างไร → มาจากไหน**

**ประเภทที่มา**

| ที่มา | ความหมาย |
| --- | --- |
| `US-00X ACn` | มาจาก Acceptance Criteria ของ user story ตรง ๆ |
| `UX` | การตัดสินใจด้าน UX ที่ไม่มีใน AC → BA ตรวจว่าควรเพิ่มเป็น AC ไหม |
| `glossary · SA ยืนยัน` | สูตรหรือคำศัพท์ ต้องให้ SA ยืนยันก่อนเขียนโค้ด |
| `screen-inventory` | หน้าจอ/ส่วนที่ยังไม่มี AC รองรับ → ดู D-N |

ข้อความ **ตัวหนา** ในคอลัมน์โน้ต = ข้อความที่แสดงบนหน้าจอหรือสูตร

---

## Mobile (พนักงานสาขา)

### M00-A — หน้าหลัก

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [M00-A_01](annotated/mobile-annotated/M00-A_01_quick-actions.png) | ปุ่มลัด | ปุ่มลัดพาไป M01-A สร้างใบขอซื้อ และ M02-A รับของ | UX |
| [M00-A_02](annotated/mobile-annotated/M00-A_02_branch-summary.png) | ตัวเลขสรุป | ตัวเลขทั้งหมดเป็นของสาขาผู้ใช้เท่านั้น | US-006 AC8 |

### M01 — สร้างใบขอซื้อ (PR)

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [M01-A_01](annotated/mobile-annotated/M01-A_01_unit-select.png) | ช่องเลือกหน่วย | เลือกหน่วยจากรายการเท่านั้น (ขวด / แพ็ค / ลัง) พิมพ์เองไม่ได้ | US-004 AC1 |
| [M01-A_02](annotated/mobile-annotated/M01-A_02_base-unit-calc.png) | ข้อความ "= 60 ขวด" | **จำนวนหน่วยฐาน = จำนวน × ตัวคูณของหน่วยที่เลือก** | US-004 AC2 |
| [M01-A_03](annotated/mobile-annotated/M01-A_03_add-all-suggest.png) | ปุ่ม "เพิ่มทั้งหมด" | กดแล้วเพิ่มรายการแนะนำทั้งหมดเข้า PR อัตโนมัติ มีสถานะขึ้นว่าเพิ่มแล้ว · ถ้าผู้ใช้เพิ่มเองจนเกินโควตาสาขา → ไป M01-C | UX |
| [M01-A_04](annotated/mobile-annotated/M01-A_04_submit-button.png) | ปุ่มส่งขออนุมัติ | กดแล้วได้ PR สถานะ Submitted ไป M01-D · กรอกผิดแสดง M01-B | UX |
| [M01-A_05](annotated/mobile-annotated/M01-A_05_viewport-360.png) | ขอบจอ | ต้องใช้งานได้ที่จอกว้าง 360px | US-004 AC8 |
| [M01-B_01](annotated/mobile-annotated/M01-B_01_qty-invalid.png) | ช่องจำนวนที่ผิด | จำนวนต้องมากกว่า 0 และเป็นจำนวนเต็มในหน่วยที่นับเป็นชิ้น (API 422) | US-004 AC5 |
| [M01-B_02](annotated/mobile-annotated/M01-B_02_inline-error.png) | ข้อความ error | แสดงใต้ช่องที่ผิด ค่าที่กรอกไว้ไม่หาย | UX |
| [M01-C_01](annotated/mobile-annotated/M01-C_01_suggest-qty-formula.png) | จำนวนแนะนำ | **จำนวนแนะนำ = max stock − คงเหลือ** แล้วปัดขึ้นเป็นหน่วยซื้อ | US-006 AC2 |
| [M01-C_02](annotated/mobile-annotated/M01-C_02_suggest-list-rule.png) | รายการแนะนำ | นับยอด PR/PO ที่ยังไม่ปิดรวมกับคงเหลือก่อนแนะนำ ถ้ารวมแล้วไม่ต่ำกว่าจุดสั่งซื้อ ไม่แสดง | US-006 AC4 |
| [M01-C_03](annotated/mobile-annotated/M01-C_03_over-quota-msg.png) | ข้อความ **"เกินโควตา"** | D-5 "เกินโควตา" ไม่มีใน AC ใด ต้องตัดสินใจว่าเก็บหรือตัดออก | screen-inventory |
| [M01-D_01](annotated/mobile-annotated/M01-D_01_pr-summary.png) | สรุปรายการ PR | เก็บหน่วยที่เลือก จำนวน และจำนวนหน่วยฐาน แก้ตัวคูณภายหลังไม่กระทบ PR เก่า | US-004 AC7 |
| [M01-D_02](annotated/mobile-annotated/M01-D_02_bottom-actions.png) | ปุ่มด้านล่าง | ปุ่มกลับหน้าหลัก และปุ่มดูใบขอซื้อ | UX |

### M02 — รับของตาม PO

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [M02-A_01](annotated/mobile-annotated/M02-A_01_po-card-tap.png) | การ์ด PO | แตะ PO แล้วไป M02-B | UX |
| [M02-A_02](annotated/mobile-annotated/M02-A_02_po-list-status.png) | รายการ PO | แสดง PO สถานะ Sent และ PartiallyReceived ของสาขาตัวเองเท่านั้น | US-001 AC2, AC6 |
| [M02-A_03](annotated/mobile-annotated/M02-A_03_other-branch-403.png) | การ์ด PO | เปิด PO ของสาขาอื่นได้ 403 แสดง **"ไม่มีสิทธิ์"** | US-001 AC6 |
| [M02-A_04](annotated/mobile-annotated/M02-A_04_auto-cancel-7d.png) | ข้อความยอดค้าง | ค้างส่งเกิน 7 วัน ระบบยกเลิกยอดค้างอัตโนมัติ และแจ้งฝ่ายจัดซื้อ | US-001 AC8 |
| [M02-B_01](annotated/mobile-annotated/M02-B_01_qty-stepper.png) | ปุ่ม + / ช่องจำนวน | ปุ่ม + หยุดที่ยอดค้าง ถ้าพิมพ์เกินแสดงหน้า M02-C | US-001 AC4 |
| [M02-B_02](annotated/mobile-annotated/M02-B_02_lot-expiry-fields.png) | ช่องล็อต / วันหมดอายุ | ล็อตและวันหมดอายุต้องกรอกทุกครั้ง | US-001 AC5 |
| [M02-B_03](annotated/mobile-annotated/M02-B_03_outstanding-calc.png) | ตัวเลข "ค้าง" | **ค้าง = สั่ง − รับแล้ว** คำนวณที่ API ใช้หน่วยตาม PO | glossary · SA ยืนยัน |
| [M02-B_04](annotated/mobile-annotated/M02-B_04_save-receipt.png) | ปุ่มบันทึกรับของ | สูง ≥ 44px ชิดล่าง · สำเร็จไปหน้า M02-D · ไม่สำเร็จอยู่หน้าเดิม ข้อมูลไม่หาย แสดง toast | US-001 AC7 · UX |
| [M02-C_01](annotated/mobile-annotated/M02-C_01_over-receive-msg.png) | ข้อความใต้ช่องจำนวน | รับเกินแสดง **"รับได้ไม่เกิน N"** และไม่บันทึก (API 422) | US-001 AC4 |
| [M02-C_02](annotated/mobile-annotated/M02-C_02_expiry-missing.png) | ช่องวันหมดอายุที่ไฮไลต์ | ไม่กรอกวันหมดอายุ ไฮไลต์ช่องที่ขาด กดบันทึกไม่ได้ · ให้กลับไปกรอกวันหมดอายุ → ไปที่หน้า M02-B หลังจำนวนยอดสั่งตรงกัน | US-001 AC5 |
| [M02-D_01](annotated/mobile-annotated/M02-D_01_grn-number.png) | เลขใบรับของ | รับแต่ละครั้งสร้างใบรับของ (GRN) อ้างอิง PO เดิม | US-001 AC3 |
| [M02-D_02](annotated/mobile-annotated/M02-D_02_po-status.png) | สถานะ PO | รับครบ PO เป็น Closed · รับบางส่วนเป็น PartiallyReceived แสดง **"ค้างส่ง N"** | US-001 AC1, AC2 |

### M03 — สต็อกสาขา

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [M03-A_01](annotated/mobile-annotated/M03-A_01_expiry-sort.png) | ลำดับรายการ | เรียงวันหมดอายุใกล้สุดก่อน ไม่แสดงล็อตที่เหลือ 0 | US-003 AC3, AC5 |
| [M03-A_02](annotated/mobile-annotated/M03-A_02_near-expiry-readonly.png) | ส่วนใกล้หมดอายุ | พนักงานเห็นรายการใกล้หมดอายุของสาขาตัวเองแบบอ่านอย่างเดียว ไม่มีปุ่มปรับสต็อก | US-003 AC9 |
| [M03-B_01](annotated/mobile-annotated/M03-B_01_lot-list.png) | รายการล็อต | แยกจำนวนตามล็อต แต่ละล็อตมีวันหมดอายุของตัวเอง | US-001 AC5 |
| [M03-C_01](annotated/mobile-annotated/M03-C_01_adjust-sheet-header.png) | หัว bottom sheet | D-2 ขัดกับ US-003 AC9 (พนักงานไม่มีปุ่มปรับสต็อก) → ย้ายเป็นของผู้จัดการ หรือเปลี่ยนเป็น **"แจ้งของเสีย"** | screen-inventory |

---

## Web (ผู้จัดการ / ฝ่ายจัดซื้อ / เจ้าของ)

### W00-A — หน้าหลัก

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [W00-A_01](annotated/web-annotated/W00-A_01_sidebar-overview.png) | Sidebar | ไฮไลต์ "ภาพรวมสต็อก" · การ์ดแต่ละใบลิงก์ไปหน้าที่เกี่ยวข้อง | UX |
| [W00-A_02](annotated/web-annotated/W00-A_02_summary-by-role.png) | ตัวเลขสรุป | ผู้จัดการเห็นเฉพาะสาขาตัวเอง เจ้าของร้านเห็นทุกสาขา | US-003 AC7 |
| [W00-A_03](annotated/web-annotated/W00-A_03_po-countdown-7d.png) | ตัวนับถอยหลัง | PO ค้างส่งครบ 7 วัน ระบบยกเลิกยอดค้างอัตโนมัติ | US-001 AC8 |
| [W00-A_04](annotated/web-annotated/W00-A_04_receive-simulation.png) | สวิตช์จำลอง | ตรวจรับสินค้าผักผลไม้ สามารถเข้าไปดูใบสั่งซื้อ (PO) ออเดอร์ที่จะเข้ามา | UX |

### W01 — รวมคำสั่งซื้อ (ฝ่ายจัดซื้อ)

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [W01-A_01](annotated/web-annotated/W01-A_01_pr-count-header.png) | ส่วนหัว **"7 ใบขอซื้อ"** | รวมเฉพาะ PR ที่ Approved · หน้านี้เข้าได้เฉพาะ Purchasing และ Owner | US-005 AC1, AC9 |
| [W01-A_02](annotated/web-annotated/W01-A_02_branch-split.png) | ยอดแยกสาขา | ยอดแต่ละบรรทัดแยกตามสาขา ใช้ตอนกระจายของและรับของ | US-005 AC3 |
| [W01-A_03](annotated/web-annotated/W01-A_03_min-order-warning.png) | ป้ายขาดยอดขั้นต่ำ | ต่ำกว่ายอดขั้นต่ำแสดงคำเตือน แต่ยังส่งขออนุมัติได้ | US-005 AC7 |
| [W01-A_04](annotated/web-annotated/W01-A_04_line-edit.png) | บรรทัดสินค้า | แก้จำนวน/ราคาได้ ยอดคำนวณใหม่ · ลบบรรทัดแล้ว PR บรรทัดนั้นกลับไปรอรวม | US-005 AC8, AC10 |
| [W01-A_05](annotated/web-annotated/W01-A_05_no-vendor-note.png) | โน้ตสินค้ายังไม่มีผู้ขาย | สินค้าที่ยังไม่มีผู้ขายไม่ถูกสร้างเป็น PO จนกว่าจะเลือกผู้ขาย | US-005 AC5 |
| [W01-A_06](annotated/web-annotated/W01-A_06_sidebar-breadcrumb.png) | Sidebar / หัวหน้า | ไฮไลต์ "ใบสั่งซื้อ (PO)" + breadcrumb **"ใบสั่งซื้อ (PO) › รวมคำสั่งซื้อ"** | UX |
| [W01-B_01](annotated/web-annotated/W01-B_01_just-added-row.png) | บรรทัดที่เพิ่งเพิ่ม | มีป้าย **"เพิ่งเพิ่ม"** ยอดผู้ขายและยอดรวมคำนวณใหม่ทันที toast มีปุ่มเลิกทำ | UX |
| [W01-C_01](annotated/web-annotated/W01-C_01_vendor-select.png) | ตัวเลือกผู้ขาย | ระบบเลือกผู้ขายหลักไว้ก่อน เปลี่ยนเป็นผู้ขายอื่นได้ก่อนสร้างร่าง PO | US-005 AC6 |
| [W01-D_01](annotated/web-annotated/W01-D_01_po-created-msg.png) | ข้อความสำเร็จ | ได้ร่าง PO 1 ใบต่อผู้ขาย สถานะ Draft · PR ที่รวมแล้วเปลี่ยนเป็น Converted | US-005 AC2, AC4 |
| [W01-D_02](annotated/web-annotated/W01-D_02_view-po-button.png) | ปุ่มดูใบสั่งซื้อ | พาไปรายการ PO | UX |

### W02 — อนุมัติ PO (ผู้จัดการสาขา)

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [W02-A_01](annotated/web-annotated/W02-A_01_limit-meter.png) | มิเตอร์วงเงิน | ไม่เกิน ฿5,000 (รวม 5,000 พอดี) เทียบยอดก่อน VAT ต่อใบ ผู้จัดการอนุมัติได้ | US-002 AC1, AC2 |
| [W02-A_02](annotated/web-annotated/W02-A_02_total-formula.png) | ยอด ฿4,200 | **ยอด = Σ (จำนวน × ราคาต่อหน่วย)** ทุกบรรทัด ไม่รวม VAT | glossary · SA ยืนยัน |
| [W02-A_03](annotated/web-annotated/W02-A_03_approve-button.png) | ปุ่มอนุมัติ | บันทึกผู้อนุมัติ + เวลาใน audit log แสดง toast แล้วไปใบถัดไป | US-002 AC1 |
| [W02-A_04](annotated/web-annotated/W02-A_04_edit-after-approve.png) | เลข PO | แก้ยอดหลังอนุมัติ สถานะกลับเป็น Draft ต้องอนุมัติใหม่ | US-002 AC5 |
| [W02-A_05](annotated/web-annotated/W02-A_05_near-expiry-header.png) | หัวส่วนใกล้หมดอายุ | D-1 เตือนล่วงหน้า 3 วัน (ไม่ใช่ 7) นับวันนี้เป็นวันที่ 0 เรียงวันใกล้สุดก่อน พร้อมมูลค่ารวม | US-003 AC2, AC5 |
| [W02-A_06](annotated/web-annotated/W02-A_06_expired-group.png) | กลุ่มหมดอายุแล้ว | แยกกลุ่ม **"หมดอายุแล้ว"** สีแดง มีปุ่ม **"ปรับสต็อก (ทิ้ง)"** | US-003 AC4 |
| [W02-B_01](annotated/web-annotated/W02-B_01_over-limit-meter.png) | มิเตอร์ที่เกิน | เกิน ฿5,000 ไม่เห็นปุ่มอนุมัติ เห็น **"รอเจ้าของอนุมัติ"** · เรียก API ตรงได้ 403 | US-002 AC3 |
| [W02-B_02](annotated/web-annotated/W02-B_02_owner-over-limit.png) | ข้อความเกินวงเงิน | เจ้าของร้านเห็นหน้าเดียวกันแต่มีปุ่มอนุมัติ (ยังไม่มีหน้าจอ D-6) | US-002 AC4 |
| [W02-B_03](annotated/web-annotated/W02-B_03_forward-to-owner.png) | ปุ่มส่งต่อเจ้าของร้าน | ส่งต่อแล้วสถานะเป็น **"รอเจ้าของอนุมัติ"** ใบนี้ออกจากคิวผู้จัดการ | UX |
| [W02-C_01](annotated/web-annotated/W02-C_01_modal-header.png) | หัว modal | D-3 ไม่มี AC รองรับ · เพิ่ม AC ใน US-003 หรือติดป้าย "แนวคิดเพิ่มเติม" | screen-inventory |
| [W02-C_02](annotated/web-annotated/W02-C_02_refund-calc.png) | ข้อความ "ได้คืน ฿256" | **เงินที่ได้คืน = จำนวนคงเหลือ × ราคาหลังลด** เทียบกับมูลค่าทุน | UX |

### W03 — ส่ง PO ให้ผู้ขาย

| ไฟล์ | ส่วน | โน้ต | ที่มา |
| --- | --- | --- | --- |
| [W03-A_01](annotated/web-annotated/W03-A_01_header-no-us.png) | หัวหน้า | D-4 ยังไม่มี US รองรับ ต้องเปิด US-007 ส่ง PO ให้ผู้ขาย | screen-inventory |
| [W03-A_02](annotated/web-annotated/W03-A_02_export-pdf-button.png) | ปุ่มส่งออก PDF | ส่งออกได้เฉพาะ PO ที่ Approved · ส่งแล้วสถานะเป็น Sent | UX |

---

## สรุป

| | Mobile | Web | รวม |
| --- | --- | --- | --- |
| หมุดทั้งหมด | 30 | 27 | **57** |
| จาก AC | 21 | 16 | 37 |
| UX (ควรให้ BA ตรวจว่าเพิ่มเป็น AC ไหม) | 6 | 8 | 14 |
| glossary · SA ยืนยัน | 1 | 1 | 2 |
| screen-inventory (D-N) | 2 | 2 | 4 |

> หมุดบางอันมีทั้ง AC และ UX (เช่น M02-B_04) นับตามที่มาแรก
