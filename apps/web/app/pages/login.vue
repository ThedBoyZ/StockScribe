<script setup lang="ts">
// Demo login (ADR-0003): pick a persona, the API sets the sf_session cookie, go to the home page.
import type { Persona, Role } from '~/stores/auth'

useHead({ title: 'เลือกผู้ใช้ · StockScribe' })

const auth = useAuthStore()

const { data: personas, status, refresh } = useFetch<Persona[]>('/api/v1/auth/personas', {
  server: false,
  default: () => [],
})

/** Where each persona lands and what to try first (docs/sa/seed-data.md). */
const HINT: Record<Role, string> = {
  BranchManager: 'อนุมัติ PO ตามวงเงิน · ของใกล้หมดอายุ',
  Purchasing: 'รวมใบขอซื้อหลายสาขาเป็น PO',
  Staff: 'รับของบนมือถือ · แนะนำเปิดบนมือถือ',
  Owner: 'ภาพรวมทุกสาขา · อนุมัติทุกยอด',
  Admin: 'ตั้งค่าระบบ',
}

const pendingId = ref<string | null>(null)
const loginError = ref<string | null>(null)

async function choose(persona: Persona) {
  if (pendingId.value) return
  pendingId.value = persona.id
  loginError.value = null
  try {
    await auth.loginAs(persona.id)
    await navigateTo('/')
  } catch {
    loginError.value = 'เข้าสู่ระบบไม่สำเร็จ ลองใหม่อีกครั้ง'
  } finally {
    pendingId.value = null
  }
}

function initial(name: string) {
  // "คุณสมศรี" → "ส": skip the polite prefix so the avatar shows the name.
  return name.replace(/^คุณ/, '').charAt(0)
}
</script>

<template>
  <main class="mx-auto flex min-h-dvh w-full max-w-3xl flex-col gap-6 px-4 py-8 md:py-16">
    <header class="rounded-lg bg-slate px-6 py-8 text-surface">
      <p class="text-label-md uppercase text-lilac-line">Demo · ไม่ต้องสมัครสมาชิก</p>
      <h1 class="mt-2 text-headline-lg md:text-display-hero">StockScribe</h1>
      <p class="mt-3 text-body-lg text-surface/85">
        เลือกผู้ใช้เพื่อทดลองระบบ แต่ละคนมีสิทธิ์และเห็นข้อมูลต่างกันตามตำแหน่ง
      </p>
    </header>

    <section aria-labelledby="personas-heading">
      <h2 id="personas-heading" class="text-headline-sm text-ink">เข้าใช้งานในฐานะ</h2>

      <p v-if="status === 'pending'" class="mt-4 text-body-md text-muted" role="status">กำลังโหลดรายชื่อ…</p>

      <div
        v-else-if="status === 'error' || personas.length === 0"
        class="mt-4 rounded-lg border border-hairline bg-surface p-4 md:p-6"
        role="alert"
      >
        <p class="text-title-md text-danger">โหลดรายชื่อผู้ใช้ไม่ได้</p>
        <p class="mt-1 text-body-sm text-muted">ระบบอาจกำลังเริ่มทำงาน รอสักครู่แล้วลองใหม่</p>
        <button
          type="button"
          class="mt-4 min-h-11 rounded bg-lilac-tint px-4 text-title-md text-primary transition-colors hover:bg-lilac-tint-hover"
          @click="() => refresh()"
        >
          ลองใหม่
        </button>
      </div>

      <ul v-else class="mt-4 grid gap-3 md:grid-cols-2">
        <li v-for="persona in personas" :key="persona.id">
          <button
            type="button"
            class="flex min-h-11 w-full items-center gap-4 rounded-lg border border-hairline bg-surface p-4 text-left transition-colors hover:border-lilac-line hover:bg-row-hover disabled:opacity-60 md:p-5"
            :disabled="pendingId !== null"
            :aria-busy="pendingId === persona.id"
            @click="choose(persona)"
          >
            <span
              class="flex size-12 shrink-0 items-center justify-center rounded-full bg-lilac-tint font-display text-headline-sm text-primary"
              aria-hidden="true"
            >
              {{ initial(persona.name) }}
            </span>
            <span class="min-w-0 flex-1">
              <span class="block text-title-md text-ink">{{ persona.name }}</span>
              <span class="block text-body-sm text-muted">
                {{ ROLE_LABEL[persona.role] }}<template v-if="persona.branch"> · {{ persona.branch.name }}</template>
              </span>
              <span class="mt-1 block text-body-sm text-slate">{{ HINT[persona.role] }}</span>
            </span>
            <span class="shrink-0 text-label-md text-primary" aria-hidden="true">
              {{ pendingId === persona.id ? 'กำลังเข้า…' : '→' }}
            </span>
          </button>
        </li>
      </ul>

      <p v-if="loginError" class="mt-4 text-body-md text-danger" role="alert">{{ loginError }}</p>
    </section>

    <footer class="mt-auto text-center text-body-sm text-muted">
      ชื่อร้าน ผู้ขาย และข้อมูลทั้งหมดเป็นข้อมูลสมมติ
    </footer>
  </main>
</template>
