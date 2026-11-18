<script setup lang="ts">
// Slice 0: prove the browser → nginx → API → Postgres/Mongo path works end to end.
type CheckStatus = 'Healthy' | 'Degraded' | 'Unhealthy'

interface HealthReport {
  status: CheckStatus
  durationMs: number
  checks: Record<string, { status: CheckStatus; durationMs: number }>
}

type State =
  | { kind: 'loading' }
  | { kind: 'ready' | 'notReady'; report: HealthReport }
  | { kind: 'unreachable'; httpStatus?: number }

type Tone = 'success' | 'danger' | 'neutral'

interface Row {
  label: string
  detail: string
  tone: Tone
  text: string
  durationMs?: number
}

const state = ref<State>({ kind: 'loading' })
const checkedAt = ref<Date | null>(null)

async function checkHealth() {
  state.value = { kind: 'loading' }
  try {
    // Same-origin call: nginx (Docker) or nitro devProxy (npm run dev) forwards it to the API.
    // 503 still carries a JSON report, so don't throw on non-2xx.
    const res = await $fetch.raw<HealthReport>('/health/ready', {
      ignoreResponseError: true,
      timeout: 10_000,
      headers: { accept: 'application/json' },
    })
    const report = res._data
    state.value =
      report && typeof report === 'object' && 'checks' in report
        ? { kind: res.ok ? 'ready' : 'notReady', report }
        : { kind: 'unreachable', httpStatus: res.status }
  } catch {
    state.value = { kind: 'unreachable' }
  } finally {
    checkedAt.value = new Date()
  }
}

onMounted(checkHealth)

const DEPENDENCIES = [
  { key: 'postgres', label: 'PostgreSQL', detail: 'ธุรกรรม · PR, PO, รับของ, สต็อก' },
  { key: 'mongo', label: 'MongoDB', detail: 'audit log · แจ้งเตือน' },
] as const

const rows = computed<Row[]>(() => {
  const s = state.value
  const api: Row =
    s.kind === 'loading'
      ? { label: 'API', detail: 'ASP.NET Core · .NET 10', tone: 'neutral', text: 'กำลังตรวจ…' }
      : s.kind === 'unreachable'
        ? { label: 'API', detail: 'ASP.NET Core · .NET 10', tone: 'danger', text: 'ติดต่อไม่ได้' }
        : { label: 'API', detail: 'ASP.NET Core · .NET 10', tone: 'success', text: 'ออนไลน์', durationMs: s.report.durationMs }

  const deps = DEPENDENCIES.map<Row>(({ key, label, detail }) => {
    if (s.kind === 'loading') return { label, detail, tone: 'neutral', text: 'กำลังตรวจ…' }
    if (s.kind === 'unreachable') return { label, detail, tone: 'neutral', text: 'ไม่ทราบ' }
    const check = s.report.checks[key]
    if (!check) return { label, detail, tone: 'neutral', text: 'ไม่มีข้อมูล' }
    return check.status === 'Healthy'
      ? { label, detail, tone: 'success', text: 'พร้อม', durationMs: check.durationMs }
      : { label, detail, tone: 'danger', text: 'ไม่พร้อม', durationMs: check.durationMs }
  })

  return [api, ...deps]
})

const summary = computed(() => {
  switch (state.value.kind) {
    case 'loading':
      return { tone: 'neutral' as Tone, text: 'กำลังตรวจสอบระบบ…' }
    case 'ready':
      return { tone: 'success' as Tone, text: 'ระบบพร้อมใช้งาน' }
    case 'notReady':
      return { tone: 'danger' as Tone, text: 'API ทำงาน แต่ฐานข้อมูลยังไม่พร้อม' }
    default: {
      const code = state.value.httpStatus
      return { tone: 'danger' as Tone, text: code ? `ติดต่อ API ไม่ได้ (HTTP ${code})` : 'ติดต่อ API ไม่ได้' }
    }
  }
})

const badgeClass: Record<Tone, string> = {
  success: 'bg-success-bg text-success',
  danger: 'bg-danger-bg text-danger',
  neutral: 'bg-neutral-bg text-neutral',
}
const badgeIcon: Record<Tone, string> = { success: '✓', danger: '✕', neutral: '•' }

const checkedAtText = computed(() =>
  checkedAt.value
    ? checkedAt.value.toLocaleTimeString('th-TH', { timeZone: 'Asia/Bangkok', hour: '2-digit', minute: '2-digit', second: '2-digit' })
    : '—',
)
</script>

<template>
  <main class="mx-auto flex min-h-dvh w-full max-w-xl flex-col gap-6 px-4 py-8 md:py-16">
    <!-- Hero: slate block per DESIGN.md -->
    <header class="rounded-lg bg-slate px-6 py-8 text-surface">
      <p class="text-label-md uppercase text-lilac-line">Walking skeleton · Slice 0</p>
      <h1 class="mt-2 text-headline-lg md:text-display-hero">StockScribe</h1>
      <p class="mt-3 text-body-lg text-surface/85">
        ระบบจัดซื้อและสต็อกสำหรับซูเปอร์มาร์เก็ตหลายสาขา
      </p>
    </header>

    <!-- Status card: level 1 = surface + hairline border, no shadow -->
    <section class="rounded-lg border border-hairline bg-surface" aria-labelledby="status-heading">
      <div class="flex flex-col items-start gap-3 border-b border-hairline p-4 sm:flex-row sm:justify-between sm:gap-4 md:p-6">
        <div>
          <h2 id="status-heading" class="text-headline-sm text-ink">สถานะระบบ</h2>
          <p class="mt-1 text-body-sm text-muted">
            จาก <code class="whitespace-nowrap font-mono text-body-sm">GET /health/ready</code> ·
            <span class="whitespace-nowrap">ตรวจล่าสุด <span class="tabular">{{ checkedAtText }}</span></span>
          </p>
        </div>
        <span
          class="inline-flex shrink-0 items-center gap-1 rounded-full px-3 py-1 text-label-sm"
          :class="badgeClass[summary.tone]"
          role="status"
          aria-live="polite"
        >
          <span aria-hidden="true">{{ badgeIcon[summary.tone] }}</span>
          {{ summary.text }}
        </span>
      </div>

      <ul class="divide-y divide-hairline">
        <li v-for="row in rows" :key="row.label" class="flex items-center justify-between gap-4 px-4 py-3 md:px-6">
          <div class="min-w-0">
            <p class="text-title-md text-ink">{{ row.label }}</p>
            <p class="truncate text-body-sm text-muted">{{ row.detail }}</p>
          </div>
          <div class="flex shrink-0 items-center gap-3">
            <span v-if="row.durationMs !== undefined" class="font-mono text-body-sm text-muted tabular">
              {{ row.durationMs }} ms
            </span>
            <span class="inline-flex items-center gap-1 rounded-full px-3 py-1 text-label-sm" :class="badgeClass[row.tone]">
              <span aria-hidden="true">{{ badgeIcon[row.tone] }}</span>
              {{ row.text }}
            </span>
          </div>
        </li>
      </ul>

      <div class="border-t border-hairline p-4 md:p-6">
        <!-- Secondary button: lilac-tint (primary is reserved for main actions) -->
        <button
          type="button"
          class="min-h-11 w-full rounded bg-lilac-tint px-4 text-title-md text-primary transition-colors hover:bg-lilac-tint-hover disabled:opacity-60 md:w-auto"
          :disabled="state.kind === 'loading'"
          @click="checkHealth"
        >
          {{ state.kind === 'loading' ? 'กำลังตรวจ…' : 'ตรวจอีกครั้ง' }}
        </button>
      </div>
    </section>

    <footer class="mt-auto text-center text-body-sm text-muted">
      Nuxt 4 SPA · ASP.NET Core · PostgreSQL · MongoDB
    </footer>
  </main>
</template>
