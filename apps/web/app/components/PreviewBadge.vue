<script setup lang="ts">
// Marks a panel that shows sample data from the design, until its slice ships real data.
defineProps<{ slice: number }>()
</script>

<template>
  <span class="inline-flex shrink-0 items-center gap-1 rounded-full bg-neutral-bg px-2.5 py-0.5 text-label-sm text-neutral" :title="`ข้อมูลตัวอย่างจากดีไซน์ · ใช้ข้อมูลจริงใน Slice ${slice}`">
    <AppIcon name="visibility" :size="14" /> ตัวอย่าง · Slice {{ slice }}
  </span>
</template>
