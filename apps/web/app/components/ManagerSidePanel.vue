<script setup lang="ts">
// W02-A right column: outstanding deliveries (US-001, Slice 3) and branch waste index (Slice 6).
// Sample data from docs/sa/seed-data.md until those slices ship.
</script>

<template>
  <div class="flex flex-col gap-6">
    <!-- Outstanding deliveries -->
    <section class="rounded-lg bg-surface p-5 shadow-sm" aria-labelledby="outstanding-heading">
      <div class="flex items-center justify-between gap-2">
        <h2 id="outstanding-heading" class="flex items-center gap-2 text-title-md text-ink"><AppIcon name="local_shipping" class="text-primary" /> ติดตามของค้างส่ง</h2>
        <PreviewBadge :slice="3" />
      </div>

      <p class="mt-4 flex gap-2 rounded bg-canvas p-3 text-body-sm text-muted">
        <AppIcon name="info" :size="18" class="text-primary" />
        <span>ยอดค้างส่งจะถูกระบบ <strong class="text-ink">ยกเลิกอัตโนมัติเมื่อครบ 7 วัน</strong> ตามระเบียบจัดซื้อและคำนวณคืนงบสาขา</span>
      </p>

      <article class="mt-4 border-l-4 border-warning bg-warning-bg/40 p-4">
        <p class="font-mono text-label-md text-ink">PO-2026-0808</p>
        <p class="text-title-md text-ink">บจก. สยามฟาร์มเฟรช</p>
        <p class="text-body-sm text-muted">ค้าง: นมสด UHT รสจืด 1 ลิตร (4 ลัง)</p>
        <div class="mt-3 flex items-center justify-between gap-2 rounded bg-surface p-3">
          <span class="flex items-center gap-1.5 text-body-sm text-muted"><span class="size-2 rounded-full bg-warning" /> เหลือเวลาก่อนตัดสิทธิ์</span>
          <span class="font-display text-headline-sm text-warning tabular">20 ชม. 14 นาที</span>
        </div>
        <div class="mt-3 grid grid-cols-2 gap-2">
          <button type="button" disabled class="inline-flex min-h-11 items-center justify-center gap-1 rounded-full bg-surface px-3 text-label-md text-ink shadow-sm"><AppIcon name="call" :size="16" /> โทรเร่งผู้ขาย</button>
          <button type="button" disabled class="inline-flex min-h-11 items-center justify-center gap-1 rounded-full bg-primary px-3 text-label-md text-surface opacity-80"><AppIcon name="inventory" :size="16" /> ตรวจรับสินค้า</button>
        </div>
      </article>

      <article class="mt-4 rounded bg-canvas/70 p-4">
        <div class="flex items-start justify-between gap-2">
          <p class="font-mono text-label-md text-ink">PO-2026-0815</p>
          <span class="rounded-full bg-lilac-tint px-2 py-0.5 text-label-sm text-primary tabular">เหลือ 2 วัน 14 ชม.</span>
        </div>
        <p class="text-title-md text-ink">บจก. ธารทิพย์ เบฟเวอเรจ</p>
        <p class="text-body-sm text-muted">ค้าง: น้ำดื่ม 600 มล. (7 แพ็ค)</p>
      </article>
    </section>

    <!-- Waste index -->
    <section class="rounded-lg bg-surface p-5 shadow-sm" aria-labelledby="waste-heading">
      <div class="flex items-center justify-between gap-2">
        <h2 id="waste-heading" class="text-label-md uppercase tracking-wider text-muted">ดัชนีของเสียสาขาเดือนนี้</h2>
        <PreviewBadge :slice="6" />
      </div>
      <div class="mt-4 flex items-center gap-4">
        <div class="relative size-24 shrink-0">
          <svg viewBox="0 0 36 36" class="size-24 -rotate-90" aria-hidden="true">
            <circle cx="18" cy="18" r="15.9" fill="none" class="stroke-hairline" stroke-width="3.2" />
            <circle cx="18" cy="18" r="15.9" fill="none" class="stroke-primary" stroke-width="3.2" stroke-linecap="round" stroke-dasharray="58 100" />
          </svg>
          <span class="absolute inset-0 flex items-center justify-center font-display text-headline-sm text-ink">-58%</span>
        </div>
        <div>
          <p class="font-display text-headline-sm text-ink">ของเสียลดลง 58%</p>
          <p class="text-body-sm text-muted">มูลค่าเสียทิ้งลดจาก <s>฿9,600</s> สู่ ฿4,030</p>
        </div>
      </div>
      <p class="mt-4 flex items-center gap-2 rounded bg-success-bg p-3 text-label-md text-success">
        <AppIcon name="verified" :size="18" /> เป้าหมายสาขา: ของเสียต่ำกว่า ฿5,000 / เดือน (ผ่านเกณฑ์)
      </p>
    </section>

    <!-- Receiving shortcut -->
    <div class="flex items-center gap-3 rounded-lg bg-surface p-4 shadow-sm">
      <span class="flex size-10 items-center justify-center rounded-full bg-lilac-tint text-primary"><AppIcon name="inventory" /></span>
      <div class="flex-1">
        <p class="text-title-md text-ink">ตรวจรับสินค้าผักผลไม้เข้า</p>
        <p class="text-body-sm text-muted">รอบบ่ายรอเทียบยอดอีก 2 PO</p>
      </div>
      <PreviewBadge :slice="3" />
    </div>
  </div>
</template>
