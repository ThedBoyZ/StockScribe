<script setup lang="ts">
// Material Symbols icon, as in docs/ux/template-html. Decorative by default (aria-hidden).
withDefaults(defineProps<{ name: string; size?: number; filled?: boolean }>(), { size: 20, filled: false })
</script>

<template>
  <span
    class="material-symbols-outlined shrink-0 select-none leading-none"
    :style="{ fontSize: `${size}px`, fontVariationSettings: `'FILL' ${filled ? 1 : 0}` }"
    aria-hidden="true"
  >{{ name }}</span>
</template>
