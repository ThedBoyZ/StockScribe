<script setup lang="ts">
// Home after login (Slice 1). Role-specific screens (W02-A, W01-A, M00-A, W00-A) arrive in later slices;
// until then this page shows who you are, what your role will do, and the system status.
const auth = useAuthStore()
const me = computed(() => auth.me!) // the global route guard guarantees a session here

useHead({ title: 'หน้าแรก · StockScribe' })

const features = computed(() => ROLE_FEATURES[me.value.role])
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
</script>

<template>
  <main v-if="auth.me" class="mx-auto flex min-h-dvh w-full max-w-xl flex-col gap-6 px-4 py-8 md:py-16">
    <header class="rounded-lg bg-slate px-6 py-8 text-surface">
      <p class="text-label-md uppercase text-lilac-line">
        {{ ROLE_LABEL[me.role] }}<template v-if="me.branch"> · {{ me.branch.name }}</template>
      </p>
      <h1 class="mt-2 text-headline-lg">สวัสดี, {{ me.name }}</h1>
      <p class="mt-3 text-body-lg text-surface/85">ยินดีต้อนรับสู่ StockScribe</p>
      <button
        type="button"
        class="mt-6 min-h-11 rounded border border-surface/40 px-4 text-title-md text-surface transition-colors hover:bg-surface/10 disabled:opacity-60"
        :disabled="loggingOut"
        @click="logout"
      >
        {{ loggingOut ? 'กำลังออก…' : 'เปลี่ยนผู้ใช้ / ออกจากระบบ' }}
      </button>
    </header>

    <section class="rounded-lg border border-hairline bg-surface" aria-labelledby="features-heading">
      <div class="border-b border-hairline p-4 md:p-6">
        <h2 id="features-heading" class="text-headline-sm text-ink">สิ่งที่ {{ ROLE_LABEL[me.role] }} ทำได้</h2>
        <p class="mt-1 text-body-sm text-muted">หน้าจอแต่ละส่วนกำลังทยอยเปิดตามลำดับการพัฒนา</p>
      </div>
      <ul class="divide-y divide-hairline">
        <li
          v-for="feature in features"
          :key="feature.text"
          class="flex items-center justify-between gap-4 px-4 py-3 md:px-6"
        >
          <p class="min-w-0 text-body-md text-ink">{{ feature.text }}</p>
          <span class="shrink-0 rounded-full bg-neutral-bg px-3 py-1 text-label-sm text-neutral">
            เร็วๆ นี้ · Slice {{ feature.slice }}
          </span>
        </li>
      </ul>
    </section>

    <SystemStatus />

    <footer class="mt-auto text-center text-body-sm text-muted">
      Nuxt 4 SPA · ASP.NET Core · PostgreSQL · MongoDB
    </footer>
  </main>
</template>
