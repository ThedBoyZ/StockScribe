<script setup lang="ts">
// Web shell from the designs (W00–W02): fixed sidebar + header bar. Below lg the sidebar becomes a drawer.
const auth = useAuthStore()
const me = computed(() => auth.me!)

interface NavItem { label: string; icon: string; active?: boolean; slice?: number }

const nav = computed<NavItem[]>(() => [
  { label: 'ภาพรวมสต็อก', icon: 'grid_view', slice: 4 },
  { label: 'ใบสั่งซื้อ (PO)', icon: 'shopping_bag', active: me.value.role === 'BranchManager' || me.value.role === 'Owner' },
  { label: 'คลังสินค้าสด & เกษตร', icon: 'inventory_2', slice: 4 },
  { label: 'ตรวจรับสินค้า', icon: 'verified', slice: 3 },
  { label: 'คู่ค้า & ผู้ขาย', icon: 'storefront', slice: 5 },
  { label: 'รายงานต้นทุน & ของเสีย', icon: 'bar_chart', slice: 6 },
])

const drawerOpen = ref(false)
const menuOpen = ref(false)
const loggingOut = ref(false)

async function logout() {
  loggingOut.value = true
  try {
    await auth.logout()
  } finally {
    loggingOut.value = false
    await navigateTo('/login')
  }
}

const shortRole = computed(() => {
  const label = me.value.role === 'BranchManager' ? 'ผจก.' : ROLE_LABEL[me.value.role]
  return me.value.branch ? `${label} ${me.value.branch.name}` : label
})
</script>

<template>
  <div class="min-h-dvh bg-canvas">
    <!-- Drawer backdrop (mobile) -->
    <div v-if="drawerOpen" class="fixed inset-0 z-40 bg-ink/30 lg:hidden" @click="drawerOpen = false" />

    <!-- Sidebar -->
    <aside
      class="fixed inset-y-0 left-0 z-50 flex w-64 flex-col justify-between bg-surface px-4 py-6 shadow-[0_1px_8px_rgba(0,0,0,0.04)] transition-transform lg:translate-x-0"
      :class="drawerOpen ? 'translate-x-0' : '-translate-x-full'"
      aria-label="เมนูหลัก"
    >
      <div class="flex flex-col gap-6">
        <div class="flex items-center gap-3 px-2">
          <span class="flex size-10 items-center justify-center rounded-full bg-lilac-tint text-primary shadow-[0_4px_14px_0_rgba(122,79,176,0.2)]">
            <AppIcon name="eco" :size="24" />
          </span>
          <div class="flex flex-col">
            <span class="font-display text-headline-sm text-ink">StockScribe</span>
            <span class="text-label-sm text-muted">Multi-branch Retail OS</span>
          </div>
        </div>

        <button
          type="button"
          class="flex w-full items-center justify-center gap-2 rounded-full bg-primary px-4 py-2.5 text-title-md text-surface opacity-60 shadow-[0_4px_14px_0_rgba(122,79,176,0.35)]"
          disabled
          title="สร้างใบขอซื้อ · Slice 5"
        >
          <AppIcon name="add_circle" />
          สร้างใบขอซื้อ
        </button>

        <nav class="flex flex-col gap-1">
          <a
            v-for="item in nav"
            :key="item.label"
            :href="item.active ? '/' : undefined"
            :aria-current="item.active ? 'page' : undefined"
            :aria-disabled="!item.active"
            :title="item.slice ? `เร็วๆ นี้ · Slice ${item.slice}` : undefined"
            class="flex items-center gap-3 rounded px-4 py-2.5 text-title-md transition-colors"
            :class="item.active ? 'bg-lilac-tint text-primary' : 'cursor-default text-muted hover:bg-row-hover'"
          >
            <AppIcon :name="item.icon" />
            <span class="flex-1">{{ item.label }}</span>
            <span v-if="item.slice" class="text-label-sm text-muted/70">S{{ item.slice }}</span>
          </a>
        </nav>
      </div>

      <div v-if="me.branch" class="flex flex-col gap-2 rounded bg-canvas/70 p-3">
        <div class="flex items-center justify-between px-1 text-muted">
          <span class="text-label-sm uppercase tracking-wider">โหมดสาขา</span>
          <span class="size-2 animate-pulse rounded-full bg-success" />
        </div>
        <div class="flex items-center gap-2 px-1 text-title-md text-ink">
          <AppIcon name="store" :size="18" class="text-primary" />
          <span class="truncate">{{ me.branch.name }}</span>
        </div>
      </div>
      <div v-else class="rounded bg-canvas/70 p-3 text-body-sm text-muted">
        <AppIcon name="domain" :size="18" class="mr-1 align-middle text-primary" /> ดูได้ทุกสาขา
      </div>
    </aside>

    <div class="lg:pl-64">
      <!-- Header bar -->
      <header class="sticky top-0 z-30 flex h-16 items-center justify-between gap-4 bg-surface/90 px-4 shadow-[0_1px_8px_rgba(0,0,0,0.04)] backdrop-blur-xl md:h-20 md:px-8">
        <button type="button" class="flex size-10 items-center justify-center rounded-full text-ink hover:bg-row-hover lg:hidden" aria-label="เปิดเมนู" @click="drawerOpen = true">
          <AppIcon name="menu" :size="24" />
        </button>

        <div class="hidden max-w-xl flex-1 md:block">
          <label class="relative flex items-center">
            <span class="sr-only">ค้นหา</span>
            <AppIcon name="search" class="absolute left-4 text-muted" />
            <input
              type="search"
              disabled
              placeholder="ค้นหาบาร์โค้ด, รหัสสินค้า, หรือ PO…"
              class="w-full rounded-full bg-canvas py-2.5 pl-11 pr-14 text-body-md text-ink placeholder:text-muted"
            />
            <kbd class="absolute right-3 rounded bg-hairline px-2 py-0.5 text-label-sm text-muted">⌘K</kbd>
          </label>
        </div>

        <div class="flex items-center gap-2 md:gap-4">
          <span v-if="me.branch" class="hidden items-center gap-1.5 rounded-full bg-canvas px-4 py-1.5 text-label-md text-ink lg:inline-flex">
            <AppIcon name="pin_drop" :size="18" class="text-primary" /> {{ me.branch.name }}
          </span>
          <button type="button" class="relative flex size-10 items-center justify-center rounded-full bg-surface text-muted shadow-[0_2px_6px_-1px_rgba(15,23,42,0.05)]" aria-label="การแจ้งเตือน" title="การแจ้งเตือน · Slice 4">
            <AppIcon name="notifications" :size="22" />
            <span class="absolute right-2 top-2 size-2 rounded-full bg-danger" />
          </button>

          <div class="relative">
            <button
              type="button"
              class="flex items-center gap-2 rounded-full bg-canvas/70 py-1 pl-1 pr-3 hover:bg-row-hover"
              :aria-expanded="menuOpen"
              aria-haspopup="menu"
              @click="menuOpen = !menuOpen"
            >
              <span class="flex size-8 items-center justify-center rounded-full bg-primary text-surface"><AppIcon name="person" :size="18" /></span>
              <span class="hidden flex-col text-left sm:flex">
                <span class="text-label-md leading-tight text-ink">{{ me.name }}</span>
                <span class="text-label-sm leading-tight text-muted">{{ shortRole }}</span>
              </span>
              <AppIcon name="expand_more" :size="18" class="text-muted" />
            </button>
            <div v-if="menuOpen" class="absolute right-0 mt-2 w-56 rounded-lg border border-hairline bg-surface p-2 shadow-dropdown" role="menu">
              <button type="button" role="menuitem" class="flex min-h-11 w-full items-center gap-2 rounded px-3 text-body-md text-ink hover:bg-row-hover disabled:opacity-60" :disabled="loggingOut" @click="logout">
                <AppIcon name="switch_account" /> {{ loggingOut ? 'กำลังออก…' : 'เปลี่ยนผู้ใช้ / ออกจากระบบ' }}
              </button>
            </div>
          </div>
        </div>
      </header>

      <main class="px-4 pb-10 pt-6 md:px-8">
        <slot />
      </main>
    </div>
  </div>
</template>
