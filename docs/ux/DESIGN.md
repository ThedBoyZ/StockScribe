# Design System — Lilac Dusk

> เจ้าของ: **UX/UI** · ใช้กับทุกหน้าจอ M และ W · ต้นฉบับเต็ม (token ครบทุกตัวของ Stitch): [template-html/web/lilac_dusk/DESIGN.md](template-html/web/lilac_dusk/DESIGN.md) และ [template-html/mobile/lilac_dusk/DESIGN.md](template-html/mobile/lilac_dusk/DESIGN.md)

ไฟล์นี้สรุปค่าที่ dev ต้องใช้จริง ถ้าขัดกับไฟล์ต้นฉบับ **ให้ยึดไฟล์นี้**

## แนวคิด

อบอุ่น เรียบ เหมือนกระดาษ ไม่ใช่ dashboard สีฟ้า-เขียวแบบ enterprise ทั่วไป

- พื้นหลังสีขาวอมครีม + การ์ดสีขาวนวล แยกชั้นด้วยสีพื้นและเส้นบาง ไม่ใช้เงาหนา
- สีม่วง (Plum) ใช้กับ **action หลักเท่านั้น** ไม่เกิน 1–2 ปุ่มต่อหน้าจอ
- สีเทาเข้ม (Slate) ใช้กับ hero block / ส่วนหัวที่ต้องการน้ำหนัก

## สี

### พื้นฐาน

| Token | ค่า | ใช้กับ |
| --- | --- | --- |
| `canvas` | `#F0EEE9` | พื้นหลังหน้า |
| `surface` | `#FAF9F6` | การ์ด, sheet, modal |
| `primary` | `#7A4FB0` | ปุ่มหลัก, สถานะที่เลือก (hover `#693E9E`, active `#5A3387`) |
| `slate` | `#494D5F` | hero block, banner, หัว section |
| `ink` | `#2E3140` | ตัวอักษรหลัก |
| `muted` | `#6B6F80` | ตัวอักษรรอง, หัวคอลัมน์ตาราง, helper text |
| `lilac-tint` | `#E8DFFA` | ปุ่มรอง, nav ที่ active, แถวที่เลือก |
| `lilac-line` | `#D0BDF4` | focus ring, เส้นขอบของที่เลือก |
| `sky` | `#A0D2EB` | ตัวเลขเด่น, badge (ใช้คู่กับตัวอักษร `#2E3140` เท่านั้น) |
| `hairline` | `#E2DFD8` | เส้นขอบการ์ด, เส้นแบ่งตาราง |

### สถานะ

| สถานะ | ตัวอักษร | พื้น | ใช้กับ |
| --- | --- | --- | --- |
| Success | `#11734A` | `#DDF3E8` | รับครบ, อนุมัติแล้ว, ในวงเงิน |
| Warning | `#8A5A00` | `#FFF0CC` | ค้างส่ง, ใกล้หมดอายุ, ต่ำกว่ายอดขั้นต่ำ |
| Danger | `#C2303A` | `#FFE3E5` | หมดอายุแล้ว, เกินวงเงิน, error ในฟอร์ม |
| Neutral | `#5D6285` | `#ECEDF2` | Draft, ข้อมูลทั่วไป |

### กฎ

1. ห้ามเพิ่มสีเขียวมรกต / ฟ้าสด / เหลืองสด เป็นสีหลักของแบรนด์
2. ปุ่ม `primary` ทึบไม่เกิน 1–2 ปุ่มต่อหน้าจอ ปุ่มอื่นใช้ `lilac-tint` หรือ outline
3. สีสถานะต้องมีข้อความหรือไอคอนกำกับเสมอ ห้ามใช้สีอย่างเดียวบอกความหมาย

## ฟอนต์

| ใช้กับ | ฟอนต์ | หมายเหตุ |
| --- | --- | --- |
| หัวข้อ / ตัวเลขใหญ่ | **Plus Jakarta Sans** 600–800 | tracking −0.01 ถึง −0.03em |
| เนื้อหา (ไทย + อังกฤษ) | **IBM Plex Sans Thai** + IBM Plex Sans 400–600 | Plus Jakarta Sans ไม่มีอักษรไทย ต้องมี fallback ภาษาไทยเสมอ |
| เลขเอกสาร / SKU / ล็อต | **JetBrains Mono** 500 | เช่น `PO-2026-0842`, `GRN-2026-0311` |
| ตัวเลขเงิน / จำนวน | ฟอนต์หัวข้อ + `font-variant-numeric: tabular-nums` | ให้หลักตรงกันในตาราง |

> ⚠️ **ต้องตัดสินใจ:** HTML จาก Stitch ใช้ฟอนต์ไม่ตรงกัน — หน้า mobile ใช้ IBM Plex Sans + Plus Jakarta Sans + JetBrains Mono ส่วนหน้า web ใช้ Plus Jakarta Sans + **Outfit** (label / ตัวเลข) ข้อเสนอ: ตัด Outfit ออก ใช้ชุดตามตารางด้านบนทั้งสองฝั่ง

### ขนาด

| Token | Web (W) | Mobile (M) | น้ำหนัก |
| --- | --- | --- | --- |
| `display-hero` | 40 / 48 | 28 / 36 | 700 |
| `headline-lg` | 32 / 40 | 24 / 32 | 700 |
| `headline-md` | 24 / 32 | 20 / 28 | 600 |
| `headline-sm` | 20 / 28 | 18 / 24 | 600 |
| `title-md` | 16 / 24 | — | 600 |
| `body-lg` | 16 / 26 | 16 / 24 | 400 |
| `body-md` | 14 / 22 | 14 / 20 | 400 |
| `body-sm` | 13 / 18 | 12 / 16 | 400 |
| `label-lg` | — | 14 / 20 | 600 |
| `label-md` | 12 / 16 | 12 / 16 | 600, tracking 0.02em |
| `label-sm` | 11 / 14 | — | 600, tracking 0.04em |
| `metric-focal` | 32 / 36 | — | 800 |
| `code-md` / `code-sm` | — | 13 / 18 · 11 / 14 | JetBrains Mono 500 |

(ขนาด / line-height เป็น px · Web = ไฟล์ web ของ Stitch ที่ breakpoint desktop · Mobile = ไฟล์ mobile ของ Stitch)

## ระยะและ layout

ฐาน 8px

| Token | ค่า |
| --- | --- |
| `space-xs` / `sm` / `md` / `lg` / `xl` / `2xl` | 4 / 8 / 16 / 24 / 32 / 48 px |

| อุปกรณ์ | Grid | Margin | Gutter | หมายเหตุ |
| --- | --- | --- | --- | --- |
| Desktop ≥ 1024px | 12 คอลัมน์ | 32px | 24px | sidebar กว้าง 260px · ออกแบบที่ 1440px |
| Tablet 768–1023px | 8 คอลัมน์ | 24px | 20px | sidebar ยุบเป็น icon rail |
| Mobile < 768px | 4 คอลัมน์ | 16px | 16px | ออกแบบที่ 360–430px · bottom nav 4 แท็บ |

- padding การ์ด: desktop 24px · mobile 16px
- **ปุ่มหลักบนมือถือสูง ≥ 44px** และวางในโซนนิ้วโป้ง (ล่างจอ) ตามข้อกำหนดใน [problem-statement](../ba/problem-statement.md)
- ใช้ได้ที่จอกว้าง **360px** ไม่มี scroll แนวนอน

## มุมโค้ง

| Token | ค่า | ใช้กับ |
| --- | --- | --- |
| `rounded` | 8px | ปุ่ม, input, alert |
| `rounded-lg` | 16px | การ์ด, ตาราง, hero |
| `rounded-xl` | 24px | modal, bottom sheet |
| `rounded-full` | 9999px | badge, chip, pill |

## ความลึก

| ระดับ | ใช้กับ | สไตล์ |
| --- | --- | --- |
| 0 | พื้นหน้า | `canvas` ไม่มีขอบ |
| 1 | การ์ด | `surface` + ขอบ 1px `hairline` ไม่มีเงา |
| 2 | dropdown, tooltip | `0 4px 16px -2px rgba(46,49,64,.08), 0 1px 3px rgba(46,49,64,.04)` |
| 3 | modal, bottom sheet | `0 20px 40px -8px rgba(46,49,64,.16)` + scrim `#2E3140` 40% |

## Component หลัก

| Component | สเปก |
| --- | --- |
| ปุ่มหลัก | พื้น `primary` ตัวอักษร `#FAF9F6` |
| ปุ่มรอง | พื้น `lilac-tint` ตัวอักษร `primary` · hover `#DECFF7` |
| ปุ่ม outline | โปร่งใส ขอบ `hairline` ตัวอักษร `ink` · hover พื้น `canvas` |
| Input | สูง 40px (mobile 44px) พื้น `surface` ขอบ `hairline` · focus ขอบ `primary` + ring `0 0 0 3px rgba(122,79,176,.2)` · error ขอบ + ข้อความใต้ช่องสี Danger |
| Stepper (+/−) | ใช้ในหน้ารับของ / สร้าง PR · ปุ่ม + หยุดที่ค่าสูงสุด (เช่น ยอดค้าง) |
| Badge | pill, padding 4×12px, `label-sm`, สีตามตารางสถานะ |
| Chip กรอง | ปกติ: พื้น `surface` ขอบ `hairline` · เลือก: พื้น `lilac-tint` ตัวอักษร `primary` ขอบ `lilac-line` |
| ตาราง | หัวคอลัมน์พื้น `canvas` ตัวอักษร `muted` · hover แถว `#F5F2FC` · แถวที่เลือก `lilac-tint` |
| Metric card | `surface` + ขอบ · overline `label-md` · ตัวเลข `metric-focal` · badge ท้าย |
| Toast | ล่างจอ (mobile) / มุมล่างขวา (web) · มีปุ่ม "เลิกทำ" เมื่อ action ย้อนได้ |

## ไอคอน

Material Symbols Outlined (weight 400, ตัวเต็มเมื่อ active)
