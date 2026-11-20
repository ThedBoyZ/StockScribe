import type { Role } from '~/stores/auth'

/** Thai labels for roles (docs/sa/rbac.md). */
export const ROLE_LABEL: Record<Role, string> = {
  Staff: 'พนักงานสาขา',
  BranchManager: 'ผู้จัดการสาขา',
  Purchasing: 'ฝ่ายจัดซื้อ',
  Owner: 'เจ้าของร้าน',
  Admin: 'ผู้ดูแลระบบ',
}

export interface RoleFeature {
  text: string
  /** Dev slice that delivers it (docs/sa/README.md). */
  slice: number
}

/** What each role will do, so the demo home explains the coming screens. */
export const ROLE_FEATURES: Record<Role, RoleFeature[]> = {
  BranchManager: [
    { text: 'อนุมัติ / ตีกลับ PO ของสาขาตัวเอง วงเงินไม่เกิน ฿5,000', slice: 2 },
    { text: 'ดูของใกล้หมดอายุ และทิ้งล็อตที่หมดอายุแล้ว', slice: 4 },
    { text: 'อนุมัติใบขอซื้อ (PR) ของสาขา', slice: 5 },
  ],
  Purchasing: [
    { text: 'รวม PR หลายสาขาเป็นร่าง PO แยกตามผู้ขาย', slice: 5 },
    { text: 'ส่ง PO ขออนุมัติ และส่ง PDF ให้ผู้ขาย', slice: 5 },
    { text: 'ดูรายการแนะนำสั่งซื้อจากสินค้าที่ต่ำกว่าจุดสั่งซื้อ', slice: 6 },
  ],
  Staff: [
    { text: 'รับของตาม PO บนมือถือ รวมถึงรับไม่ครบ', slice: 3 },
    { text: 'ดูของใกล้หมดอายุของสาขา (อ่านอย่างเดียว)', slice: 4 },
    { text: 'สร้างใบขอซื้อ (PR) เลือกหน่วยได้', slice: 5 },
  ],
  Owner: [
    { text: 'อนุมัติ PO ทุกยอด รวมถึงเกิน ฿5,000', slice: 2 },
    { text: 'ดูภาพรวมสต็อกทุกสาขา กรองตามสาขาได้', slice: 4 },
  ],
  Admin: [{ text: 'ตั้งค่าระบบ: วันเตือนหมดอายุ วงเงิน หัวกระดาษ', slice: 6 }],
}
