<script setup lang="ts">
// W02-A lower section "สินค้าเร่งด่วนที่ต้องจัดการก่อนหมดอายุ" (US-003, Slice 4).
// Sample lots from docs/sa/seed-data.md until the stock API exists.
const lots = [
  { category: 'นม & ผลิตภัณฑ์นม', icon: 'water_drop', name: 'นมสด UHT รสจืด 1 ลิตร', lot: 'L2610-C', qty: '8 กล่อง', value: 288, due: 'หมดอายุพรุ่งนี้', tone: 'danger', tip: 'แนะนำติดป้ายลด 30% ทันที', tipDetail: 'ขาย ฿29/กล่อง (คืนทุน ฿232)', action: 'สร้างป้ายโปรโมชัน 30%' },
  { category: 'เบเกอรี่ & แป้ง', icon: 'bakery_dining', name: 'ขนมปังแซนด์วิช 400 กรัม', lot: 'B2611-05', qty: '9 ห่อ', value: 270, due: 'เหลืออีก 2 วัน', tone: 'warning', tip: 'แนะนำติดป้ายลด 20%', tipDetail: 'ขาย ฿24/ห่อ (คืนทุน ฿216)', action: 'สร้างป้ายโปรโมชัน 20%' },
  { category: 'อาหารพร้อมทาน', icon: 'icecream', name: 'โยเกิร์ตรสธรรมชาติ 135 กรัม', lot: 'Y2610-21', qty: '14 ถ้วย', value: 182, due: 'เหลืออีก 3 วัน', tone: 'neutral', tip: 'แนะนำจัดชุด 3 ถ้วย ฿35', tipDetail: 'ระบายเร็วขึ้น 65% ใน 48 ชม.', action: 'สร้างป้ายโปรโมชัน' },
] as const

const dueClass = { danger: 'bg-danger-bg text-danger', warning: 'bg-warning-bg text-warning', neutral: 'bg-neutral-bg text-neutral' }
</script>

<template>
  <section aria-labelledby="expiring-heading" class="flex flex-col gap-4">
    <div class="flex flex-col justify-between gap-3 md:flex-row md:items-start">
      <div class="flex items-start gap-3">
        <span class="flex size-9 shrink-0 items-center justify-center rounded-full bg-danger-bg text-danger"><AppIcon name="timelapse" /></span>
        <div>
          <h2 id="expiring-heading" class="text-title-md text-ink">สินค้าเร่งด่วนที่ต้องจัดการก่อนหมดอายุ</h2>
          <p class="text-body-sm text-muted">ล็อตที่หมดอายุภายใน 3 วัน · ปรับราคาหรือตัดจำหน่าย</p>
        </div>
      </div>
      <div class="flex flex-wrap items-center gap-2">
        <span class="rounded-full bg-danger-bg px-2.5 py-0.5 text-label-sm text-danger">● หมดอายุแล้ว 2</span>
        <span class="rounded-full bg-warning-bg px-2.5 py-0.5 text-label-sm text-warning">ใกล้หมดอายุ 3 วัน (3)</span>
        <PreviewBadge :slice="4" />
      </div>
    </div>

    <div class="grid gap-4 md:grid-cols-3">
      <article v-for="lot in lots" :key="lot.lot" class="flex flex-col gap-3 rounded-lg bg-surface p-4 shadow-sm">
        <div class="flex flex-wrap items-center justify-between gap-2">
          <span class="rounded-full bg-lilac-tint/70 px-2.5 py-0.5 text-label-sm text-primary">{{ lot.category }}</span>
          <span class="rounded-full px-2.5 py-0.5 text-label-sm" :class="dueClass[lot.tone]">● {{ lot.due }}</span>
        </div>
        <div class="flex items-center gap-3">
          <span class="flex size-12 shrink-0 items-center justify-center rounded-full border border-hairline text-primary"><AppIcon :name="lot.icon" :size="24" /></span>
          <div class="min-w-0">
            <p class="truncate text-title-md text-ink" :title="lot.name">{{ lot.name }}</p>
            <p class="font-mono text-label-sm text-muted">ล็อต {{ lot.lot }}</p>
          </div>
        </div>
        <dl class="grid grid-cols-2 gap-y-1 border-t border-hairline pt-3 text-body-sm">
          <dt class="text-muted">จำนวนคงคลัง</dt><dd class="text-right font-semibold text-ink tabular">{{ lot.qty }}</dd>
          <dt class="text-muted">มูลค่าทุนจม</dt><dd class="text-right font-semibold text-danger tabular">{{ formatBaht(lot.value) }}</dd>
        </dl>
        <div class="rounded bg-warning-bg/60 p-2.5 text-body-sm">
          <p class="flex items-center gap-1 font-semibold text-warning"><AppIcon name="trending_down" :size="16" /> {{ lot.tip }}</p>
          <p class="text-muted">{{ lot.tipDetail }}</p>
        </div>
        <button type="button" disabled class="mt-auto inline-flex min-h-11 items-center justify-center gap-2 rounded-full bg-success-bg px-4 text-label-md text-success opacity-80">
          <AppIcon name="sell" :size="18" /> {{ lot.action }}
        </button>
      </article>
    </div>
  </section>
</template>
