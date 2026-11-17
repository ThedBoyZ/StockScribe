# API Spec

> REST · base path `/api/v1` · JSON `camelCase` · Swagger จริงสร้างจากโค้ดที่ `/swagger` (เฉพาะ non-production)
> เอกสารนี้คือสัญญาเริ่มต้น ถ้าโค้ดต้องเปลี่ยน ให้แก้ไฟล์นี้ใน PR เดียวกัน

## ข้อตกลงร่วม

| เรื่อง | ข้อตกลง |
| --- | --- |
| Auth | cookie `sf_session` (JWT) จาก `POST /auth/demo-login` · ทุก endpoint ต้องล็อกอิน ยกเว้น `/auth/*` และ `/health/*` |
| เงิน | ส่งเป็น number 2 ตำแหน่ง เช่น `4200.00` · ฝั่งหน้าจอจัดรูปเป็น `฿4,200` เอง |
| วันเวลา | ISO 8601 UTC `2026-11-17T01:15:00Z` · วันหมดอายุเป็น `2026-11-19` |
| รายการ | `?page=1&pageSize=20` · ตอบ `{ items, page, pageSize, total }` |
| Concurrency | action ที่เปลี่ยนสถานะ PO ส่ง `version` (ค่าที่ได้จาก GET) ไม่ตรง → 409 |
| กดซ้ำ | `POST /goods-receipts` ส่ง header `Idempotency-Key: <uuid>` · key เดิมได้ผลลัพธ์เดิม ไม่สร้างซ้ำ |

### Error (RFC 9457 ProblemDetails)

```json
{
  "type": "https://stockflow.dev/errors/over-receive",
  "title": "รับได้ไม่เกิน 4",
  "status": 422,
  "detail": "นมสด UHT รสจืด 1 ลิตร ค้างส่ง 4 ลัง",
  "traceId": "00-4bf92f…",
  "errors": { "lines[0].qty": ["รับได้ไม่เกิน 4"] }
}
```

| status | ใช้เมื่อ |
| --- | --- |
| 400 | รูปแบบคำขอผิด (JSON พัง, ชนิดข้อมูลผิด) |
| 401 | ยังไม่ล็อกอิน / session หมดอายุ |
| 403 | ไม่มีสิทธิ์ (role, สาขา, วงเงิน) |
| 404 | ไม่พบเอกสาร |
| 409 | สถานะไม่ให้ทำ action นี้ หรือ version ไม่ตรง |
| 422 | ข้อมูลผิดกฎธุรกิจ (รับเกิน, จำนวน ≤ 0, ไม่มีวันหมดอายุ) |

`title` เป็นข้อความภาษาไทยที่หน้าจอแสดงได้ทันที · `errors` ใช้ไฮไลต์ช่องที่ผิด (US-001 AC 5, US-004 AC 5)

---

## Health และ Auth (Slice 0–1)

| Method | Path | ตอบ |
| --- | --- | --- |
| GET | `/health/live` | 200 เมื่อ process ทำงาน |
| GET | `/health/ready` | 200 เมื่อ Postgres และ Mongo ตอบ · 503 พร้อมรายละเอียดเมื่อไม่พร้อม |
| GET | `/api/v1/auth/personas` | รายชื่อ persona สำหรับหน้า demo login |
| POST | `/api/v1/auth/demo-login` | body `{ "personaId": "…" }` → set cookie, ตอบ `me` |
| POST | `/api/v1/auth/logout` | ลบ cookie |
| GET | `/api/v1/me` | `{ id, name, role, branch: { id, name } \| null }` |

---

## US-002 · อนุมัติ PO ตามวงเงิน (Slice 2)

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/purchase-orders?status=PendingApproval&mine=true` | BranchManager, Owner | คิวรออนุมัติของผู้ใช้ |
| GET | `/api/v1/purchase-orders/{id}` | ตาม rbac | |
| POST | `/api/v1/purchase-orders/{id}/submit` | Purchasing, Owner | Draft → PendingApproval |
| POST | `/api/v1/purchase-orders/{id}/approve` | ตามกฎวงเงิน | AC 1–4 |
| POST | `/api/v1/purchase-orders/{id}/reject` | ตามกฎวงเงิน | ตีกลับเป็น Draft |

### GET `/purchase-orders/{id}`

```json
{
  "id": "0193…",
  "number": "PO-2026-0835",
  "status": "PendingApproval",
  "version": 812,
  "supplier": { "id": "…", "name": "บจก. ธารทิพย์ เบฟเวอเรจ" },
  "subtotal": 4200.00, "vatAmount": 294.00, "total": 4494.00,
  "lines": [
    { "id": "…", "product": { "id": "…", "name": "น้ำดื่ม 600 มล." }, "unit": "แพ็ค 12",
      "qtyOrdered": 12, "unitPrice": 60.00, "lineTotal": 720.00,
      "allocations": [ { "branch": "สาขา 2", "qtyOrdered": 12, "qtyReceived": 0, "qtyOutstanding": 12 } ] }
  ],
  "approval": {
    "requiredRole": "BranchManager",
    "limit": 5000.00,
    "remaining": 800.00,
    "canApprove": true,
    "message": null
  }
}
```

`approval` คำนวณฝั่ง API ตามผู้ใช้ที่เรียก หน้าจอไม่ต้องคิดกฎเอง:
- W02-A ใช้ `limit`, `remaining` วาดมิเตอร์ และใช้ `canApprove` แสดงปุ่ม
- W02-B ได้ `canApprove: false`, `message: "รอเจ้าของอนุมัติ"` (US-002 AC 3)

### POST `/purchase-orders/{id}/approve`

```json
// request
{ "version": 812 }

// 200
{ "id": "…", "number": "PO-2026-0835", "status": "Approved",
  "approvedBy": "นภา ใจดี", "approvedAt": "2026-11-17T01:15:00Z", "version": 813 }
```

| กรณี | ตอบ |
| --- | --- |
| ยอด ≤ 5,000 และเป็นผู้จัดการสาขาที่ถูกต้อง | 200 + audit log |
| ยอด 5,000.01 ขึ้นไป แต่ผู้จัดการเรียก | 403 `type: approval-limit-exceeded` |
| สถานะไม่ใช่ PendingApproval | 409 `type: invalid-state-transition` |
| version ไม่ตรง (มีคนแก้ก่อน) | 409 `type: concurrency-conflict` |

### POST `/purchase-orders/{id}/reject`

```json
{ "version": 812, "reason": "ราคาน้ำดื่มสูงกว่ารอบก่อน" }
```
`reason` บังคับ 1–200 ตัวอักษร ไม่มี → 422

---

## หน้าหลักผู้จัดการ (W02-A)

| Method | Path | ตอบ |
| --- | --- | --- |
| GET | `/api/v1/dashboard/manager` | ข้อมูลทั้งหน้าในคำขอเดียว |

```json
{
  "approvalQueue": { "count": 3, "next": { "id": "…", "number": "PO-2026-0835", "subtotal": 4200.00 } },
  "expiring": { "days": 3, "totalValue": 740.00, "expired": [ … ], "soon": [ … ] },
  "lateDeliveries": [
    { "poNumber": "PO-2026-0808", "supplier": "บจก. สยามฟาร์มเฟรช",
      "outstanding": "4 ลัง", "autoCancelAt": "2026-11-18T00:00:00Z" }
  ],
  "wasteTrend": { "label": "ข้อมูลจำลอง", "from": 9600.00, "to": 4030.00 }
}
```

---

## US-001 · รับของตาม PO (Slice 3)

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/branches/{branchId}/receivable-pos` | 🏠 Staff, BranchManager | M02-A · AC 6 |
| GET | `/api/v1/purchase-orders/{id}/receiving?branchId=` | 🏠 | M02-B: บรรทัดของสาขานี้ + ยอดค้าง |
| POST | `/api/v1/goods-receipts` | 🏠 | AC 1–5, 9 |
| POST | `/api/v1/purchase-orders/{id}/close` | Purchasing, Owner | ปิดมือ + เหตุผล |

### POST `/goods-receipts`

```http
POST /api/v1/goods-receipts
Idempotency-Key: 6f1c2a7e-…
```
```json
{
  "poId": "…",
  "branchId": "…",
  "lines": [
    { "allocationId": "…", "qty": 4, "lotNo": "L2611-A", "expiryDate": "2026-11-28" }
  ]
}
```

ทำใน transaction เดียว: สร้าง `goods_receipts` + `gr_lines` → เพิ่ม `po_allocations.qty_received` → สร้าง/บวก `stock_lots` → เขียน `stock_movements` → อัปเดตสถานะ PO

```json
// 201
{ "id": "…", "number": "GRN-2026-0311",
  "po": { "number": "PO-2026-0842", "status": "PartiallyReceived" },
  "lines": [ { "product": "นมสด UHT รสจืด 1 ลิตร", "qty": 4, "outstandingAfter": 0 } ] }
```

| กรณี | ตอบ |
| --- | --- |
| qty > ยอดค้าง | 422 `over-receive` "รับได้ไม่เกิน N" (AC 4) |
| สินค้ามีวันหมดอายุแต่ไม่ส่ง `expiryDate` | 422 + `errors["lines[i].expiryDate"]` (AC 5) |
| วันหมดอายุก่อนวันนี้ | 422 |
| allocation ไม่ใช่ของสาขาผู้ใช้ | 403 (AC 6) |
| PO ไม่ใช่ Sent / PartiallyReceived | 409 |
| `Idempotency-Key` ซ้ำ | 201 ผลเดิม ไม่สร้างใหม่ (AC 9) |

---

## US-003 · ใกล้หมดอายุและปรับสต็อก (Slice 4)

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/branches/{branchId}/stock-lots/expiring?days=3` | 🏠 ทุก role สาขา · Owner ทุกสาขา | AC 1–5, 7, 9 |
| GET | `/api/v1/stock-lots/expiring?branchId=` | Owner | AC 7 กรองสาขา |
| GET | `/api/v1/branches/{branchId}/stock?search=` | 🏠 | M03-A |
| GET | `/api/v1/branches/{branchId}/products/{productId}/lots` | 🏠 | M03-B |
| POST | `/api/v1/stock-adjustments` | 🏠 BranchManager, Owner | AC 4, 10 |
| GET | `/api/v1/notifications?unread=true` | ผู้ใช้ปัจจุบัน | AC 6 |
| POST | `/api/v1/notifications/{id}/read` | ผู้ใช้ปัจจุบัน | |

```json
// POST /stock-adjustments
{ "stockLotId": "…", "qtyChange": -5, "type": "AdjustWaste", "reason": "หมดอายุ ทิ้ง" }
```
ผลลัพธ์ทำให้ `qty_on_hand` ติดลบ → 422 · Staff เรียก → 403

---

## US-004 · สินค้า หน่วย และ PR (Slice 5)

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/products?search=` | ทุก role | |
| GET | `/api/v1/products/{id}/units` | ทุก role | AC 1 |
| PUT | `/api/v1/products/{id}/units` | Purchasing, Admin | AC 9 + audit |
| POST | `/api/v1/purchase-requests` | 🏠 Staff, BranchManager | สร้าง Draft |
| PUT | `/api/v1/purchase-requests/{id}/lines` | 🏠 | AC 2, 5 · ตอบ `baseQty` ต่อบรรทัด |
| POST | `/api/v1/purchase-requests/{id}/submit` | 🏠 | |
| POST | `/api/v1/purchase-requests/{id}/approve` · `/reject` | 🏠 BranchManager | Q-SA-02 |

## US-005 · รวมเป็น PO

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/consolidation/preview` | Purchasing, Owner | W01-A: จัดกลุ่มตามผู้ขาย + แยกสาขา + กลุ่มยังไม่มีผู้ขาย + เตือนยอดขั้นต่ำ |
| POST | `/api/v1/consolidation` | Purchasing, Owner | สร้างร่าง PO ทั้งหมดใน transaction เดียว (AC 2, 4) |
| PUT | `/api/v1/consolidation/vendor-overrides` | Purchasing, Owner | เลือกผู้ขายให้สินค้า (AC 5, 6) |
| PATCH | `/api/v1/purchase-orders/{id}/lines/{lineId}` | Purchasing, Owner | แก้จำนวน/ราคา (AC 8) |
| DELETE | `/api/v1/purchase-orders/{id}/lines/{lineId}` | Purchasing, Owner | PR กลับไปรอรวม (AC 10) |
| GET | `/api/v1/purchase-orders/{id}/suggestions` | Purchasing, Owner | AC 11 |
| POST | `/api/v1/purchase-orders/{id}/suggestions/{productId}` | Purchasing, Owner | เพิ่มสินค้าแนะนำ |

## US-006 · รายการแนะนำ

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/branches/{branchId}/reorder-suggestions` | 🏠 | AC 1–4, 7 |
| POST | `/api/v1/purchase-requests/{id}/lines/from-suggestions` | 🏠 | "เพิ่มทั้งหมด" ไม่เพิ่มซ้ำ (AC 5) |

## US-007 · ส่ง PO ให้ผู้ขาย

| Method | Path | สิทธิ์ | อ้างอิง |
| --- | --- | --- | --- |
| GET | `/api/v1/purchase-orders/{id}/pdf` | Purchasing, Owner | AC 1, 2, 5 · `application/pdf` |
| POST | `/api/v1/purchase-orders/{id}/mark-sent` | Purchasing, Owner | body `{ version, channel }` · AC 4 |

---

## งานเบื้องหลัง (ไม่ใช่ endpoint)

| Job | เวลา (Asia/Bangkok) | ทำอะไร | อ้างอิง |
| --- | --- | --- | --- |
| `ExpiryAlertJob` | 07:00 ทุกวัน | สร้าง notification `ExpiryDigest` ให้ผู้จัดการแต่ละสาขา ถ้ามีรายการ · `dedupeKey` กันซ้ำ | US-003 AC 6 |
| `OutstandingCancelJob` | 00:30 ทุกวัน | PO ที่ `sent_at` เกิน `OutstandingCancelDays` และยังค้าง → ยกเลิกยอดค้าง ปิด PO แจ้ง Purchasing | US-001 AC 8 |

ทั้งสอง job ต้องรันซ้ำได้โดยไม่เกิดผลซ้ำ (idempotent) เพราะ VM อาจ restart ระหว่างทำงาน

## ประวัติการแก้ไข

| วันที่ | การเปลี่ยนแปลง |
| --- | --- |
| 17 พ.ย. 2026 | ฉบับแรก |
