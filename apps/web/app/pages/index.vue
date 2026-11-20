<script setup lang="ts">
// Home after login. BranchManager and Owner land on the approval screen (W02-A, Slice 2);
// other roles see what is coming for them until their slices ship.
const auth = useAuthStore()
const me = computed(() => auth.me!) // the global route guard guarantees a session here

useHead({ title: 'หน้าแรก · StockScribe' })

const isApprover = computed(() => me.value.role === 'BranchManager' || me.value.role === 'Owner')
const features = computed(() => ROLE_FEATURES[me.value.role])
</script>

<template>
  <AppShell v-if="auth.me">
    <ApprovalDashboard v-if="isApprover" />

    <div v-else class="flex flex-col gap-6">
      <section class="rounded-lg bg-slate px-6 py-8 text-surface">
        <p class="text-label-md uppercase text-lilac-line">
          {{ ROLE_LABEL[me.role] }}<template v-if="me.branch"> · {{ me.branch.name }}</template>
        </p>
        <h1 class="mt-2 text-headline-lg">สวัสดี, {{ me.name }}</h1>
        <p class="mt-3 text-body-lg text-surface/85">
          หน้าหลักของตำแหน่งนี้กำลังพัฒนา ลองเปลี่ยนเป็น <strong>นภา ใจดี (ผู้จัดการสาขา)</strong> เพื่อดูหน้าอนุมัติ PO
        </p>
      </section>

      <div class="grid gap-6 xl:grid-cols-2">
        <section class="rounded-lg bg-surface shadow-sm" aria-labelledby="features-heading">
          <div class="border-b border-hairline p-4 md:p-6">
            <h2 id="features-heading" class="text-headline-sm text-ink">สิ่งที่ {{ ROLE_LABEL[me.role] }} ทำได้</h2>
            <p class="mt-1 text-body-sm text-muted">หน้าจอแต่ละส่วนกำลังทยอยเปิดตามลำดับการพัฒนา</p>
          </div>
          <ul class="divide-y divide-hairline">
            <li v-for="feature in features" :key="feature.text" class="flex items-center justify-between gap-4 px-4 py-3 md:px-6">
              <p class="min-w-0 text-body-md text-ink">{{ feature.text }}</p>
              <span class="shrink-0 rounded-full bg-neutral-bg px-3 py-1 text-label-sm text-neutral">Slice {{ feature.slice }}</span>
            </li>
          </ul>
        </section>

        <SystemStatus />
      </div>
    </div>
  </AppShell>
</template>
