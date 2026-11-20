<script setup lang="ts">
// W02-A / W02-B · approve PO within the limit (US-002, Slice 2). Layout follows
// docs/ux/template-html/web/W02-A_US002_in-limit. The API decides permission (`approval.canApprove`).
interface PoLine { id: string; productName: string; unit: string; qtyOrdered: number; unitPrice: number; lineTotal: number }
interface PurchaseOrder {
  id: string
  number: string
  status: string
  version: number
  supplier: { id: string; name: string }
  deliverTo: string | null
  subtotal: number
  vatAmount: number
  total: number
  lines: PoLine[]
  approval: { requiredRole: string; limit: number; remaining: number | null; canApprove: boolean; message: string | null }
}

const auth = useAuthStore()
const me = computed(() => auth.me!)
const isOwner = computed(() => me.value.role === 'Owner')

const { data: queue, status, refresh } = useFetch<PurchaseOrder[]>('/api/v1/purchase-orders', {
  query: { status: 'PendingApproval', mine: true },
  server: false,
  default: () => [],
})

const showLines = ref(false)
const rejecting = ref(false)
const reason = ref('')
const busy = ref(false)
const toast = ref<{ tone: 'success' | 'danger'; text: string } | null>(null)
let toastTimer: ReturnType<typeof setTimeout> | undefined

const selectedId = ref<string | null>(null)
const selected = computed(() => queue.value.find((p) => p.id === selectedId.value) ?? queue.value[0] ?? null)
watch(selected, () => {
  showLines.value = false
  rejecting.value = false
  reason.value = ''
})

const approvableCount = computed(() => queue.value.filter((p) => p.approval.canApprove).length)
const usedPercent = computed(() => (selected.value ? Math.round((selected.value.subtotal / selected.value.approval.limit) * 100) : 0))
const remaining = computed(() => selected.value?.approval.remaining ?? 0)

function notify(tone: 'success' | 'danger', text: string) {
  toast.value = { tone, text }
  clearTimeout(toastTimer)
  toastTimer = setTimeout(() => (toast.value = null), 4000)
}

async function decide(action: 'approve' | 'reject') {
  const po = selected.value
  if (!po || busy.value) return
  busy.value = true
  try {
    await $fetch(`/api/v1/purchase-orders/${po.id}/${action}`, {
      method: 'POST',
      body: { version: po.version, reason: action === 'reject' ? reason.value : undefined },
    })
    notify('success', action === 'approve' ? `อนุมัติ ${po.number} แล้ว · บันทึกใน audit log` : `ตีกลับ ${po.number} ให้ฝ่ายจัดซื้อแล้ว`)
    // W02-A_03: after a decision, open the next PO in the queue.
    const index = queue.value.findIndex((p) => p.id === po.id)
    await refresh()
    selectedId.value = queue.value[Math.min(index, queue.value.length - 1)]?.id ?? null
  } catch (error) {
    notify('danger', problemTitle(error, 'ทำรายการไม่สำเร็จ ลองใหม่อีกครั้ง'))
    await refresh() // 409: someone else changed it; show the current state
  } finally {
    busy.value = false
  }
}

async function resetDemo() {
  busy.value = true
  try {
    await $fetch('/api/v1/demo/reset', { method: 'POST' })
    await refresh()
    selectedId.value = null
    notify('success', 'รีเซ็ตข้อมูลเดโมแล้ว PO กลับมารออนุมัติครบ')
  } catch (error) {
    notify('danger', problemTitle(error, 'รีเซ็ตไม่สำเร็จ'))
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <div class="flex flex-col gap-6">
    <!-- Decision context banner (slate) -->
    <section class="flex flex-col justify-between gap-4 rounded-lg bg-slate p-6 text-surface md:flex-row md:items-center">
      <div class="flex flex-col gap-1">
        <div class="flex flex-wrap items-center gap-3">
          <h1 class="text-headline-md">{{ isOwner ? 'การอนุมัติของเจ้าของร้าน' : 'การตัดสินใจของผู้จัดการ' }}</h1>
          <span class="inline-flex items-center gap-1.5 rounded-full bg-lilac-tint px-3 py-1 text-label-md text-primary">
            <span class="size-1.5 animate-pulse rounded-full bg-primary" /> {{ todayInBangkok() }}
          </span>
        </div>
        <p class="text-body-md text-hairline">
          {{ me.name }} · {{ ROLE_LABEL[me.role] }}<template v-if="me.branch"> {{ me.branch.name }}</template> ·
          <strong class="font-semibold text-surface">{{ isOwner ? 'อนุมัติได้ทุกยอด รวมถึงเกิน ฿5,000' : 'เพดานอนุมัติ ฿5,000 ต่อใบ' }}</strong>
          (เทียบยอดก่อน VAT)
        </p>
      </div>
      <div class="flex items-center gap-4 self-start rounded-full border border-surface/20 bg-surface/10 px-4 py-2 md:self-auto">
        <span class="flex items-center gap-2 text-label-md"><AppIcon name="assignment_turned_in" class="text-lilac-line" /> รอพิจารณา</span>
        <span class="h-4 w-px bg-surface/30" />
        <span class="flex items-baseline gap-1.5 text-title-md tabular">
          {{ queue.length }} <span class="text-body-sm font-normal text-hairline">ใบ · อนุมัติได้ {{ approvableCount }}</span>
        </span>
      </div>
    </section>

    <div class="grid grid-cols-1 gap-6 xl:grid-cols-12">
      <!-- LEFT: decision -->
      <div class="flex flex-col gap-6 xl:col-span-8">
        <p v-if="status === 'pending' && queue.length === 0" class="rounded-lg bg-surface p-8 text-body-md text-muted" role="status">กำลังโหลดรายการ…</p>

        <div v-else-if="status === 'error'" class="rounded-lg bg-surface p-8" role="alert">
          <p class="text-title-md text-danger">โหลดรายการ PO ไม่ได้</p>
          <button type="button" class="mt-4 min-h-11 rounded-full bg-lilac-tint px-5 text-title-md text-primary hover:bg-lilac-tint-hover" @click="() => refresh()">ลองใหม่</button>
        </div>

        <div v-else-if="!selected" class="flex flex-col items-center rounded-lg bg-surface p-10 text-center shadow-sm">
          <span class="flex size-14 items-center justify-center rounded-full bg-success-bg text-success"><AppIcon name="task_alt" :size="32" /></span>
          <p class="mt-4 text-headline-sm text-ink">ไม่มี PO รออนุมัติแล้ว</p>
          <p class="mt-1 text-body-md text-muted">รีเซ็ตข้อมูลเดโมเพื่อลองอนุมัติอีกครั้ง</p>
          <button type="button" class="mt-5 inline-flex min-h-11 items-center gap-2 rounded-full bg-primary px-6 text-title-md text-surface shadow-md hover:bg-primary-hover disabled:opacity-60" :disabled="busy" @click="resetDemo">
            <AppIcon name="restart_alt" /> รีเซ็ตข้อมูลเดโม
          </button>
        </div>

        <!-- Hero: PO approval card -->
        <article v-else class="relative overflow-hidden rounded-lg bg-surface shadow-md" aria-labelledby="po-number">
          <div class="pointer-events-none absolute -right-20 -top-20 size-72 rounded-full bg-lilac-line/30 blur-3xl" />

          <div class="relative p-6 md:p-8">
            <!-- Top bar + queue switcher -->
            <div class="flex flex-col justify-between gap-4 sm:flex-row sm:items-start">
              <div class="flex flex-col gap-1">
                <div class="flex flex-wrap items-center gap-2">
                  <span id="po-number" class="rounded-full bg-hairline/70 px-3 py-0.5 font-mono text-label-md tracking-wider text-ink">{{ selected.number }}</span>
                  <span class="inline-flex items-center gap-1 rounded-full bg-lilac-tint/70 px-2 py-0.5 text-label-sm text-primary">
                    <AppIcon name="verified" :size="14" /> {{ selected.lines.length }} SKU
                  </span>
                  <span class="text-body-sm text-muted">· {{ selected.supplier.name }}</span>
                </div>
                <span class="text-body-sm text-muted">ส่งถึง {{ selected.deliverTo ?? 'หลายสาขา' }}</span>
              </div>

              <div class="flex max-w-full gap-1 self-start overflow-x-auto rounded-full bg-canvas p-1" role="tablist" aria-label="PO รออนุมัติ">
                <button
                  v-for="po in queue"
                  :key="po.id"
                  type="button"
                  role="tab"
                  :aria-selected="po.id === selected.id"
                  class="shrink-0 rounded-full px-3 py-1 text-label-sm tabular transition-all"
                  :class="po.id === selected.id ? 'bg-surface text-primary shadow-sm' : 'text-muted hover:text-ink'"
                  :title="po.number"
                  @click="selectedId = po.id"
                >
                  <span :class="po.approval.canApprove ? 'text-success' : 'text-warning'">{{ po.approval.canApprove ? '●' : '○' }}</span>
                  {{ po.approval.canApprove ? 'ในวงเงิน' : 'เกินวงเงิน' }} {{ formatBaht(po.subtotal) }}
                </button>
              </div>
            </div>

            <!-- Amount + status -->
            <div class="flex flex-col justify-between gap-4 py-6 md:flex-row md:items-end">
              <div>
                <div class="flex flex-wrap items-center gap-2">
                  <span class="text-label-sm uppercase tracking-wider text-muted">ยอดจัดซื้อรอบนี้</span>
                  <span class="rounded-full bg-hairline/70 px-2 py-0.5 text-label-sm text-muted">เทียบยอดก่อน VAT ต่อใบ</span>
                </div>
                <div class="mt-1 flex flex-wrap items-baseline gap-x-3">
                  <span class="font-display text-[44px] font-extrabold leading-[52px] tracking-tight text-ink tabular md:text-[56px] md:leading-[64px]">{{ formatBaht(selected.subtotal) }}</span>
                  <span class="text-body-md text-muted tabular">ยอดรวมสุทธิรวม VAT 7% {{ formatBaht(selected.total) }}</span>
                </div>
              </div>
              <span
                class="inline-flex items-center gap-2 self-start rounded-full px-4 py-2 md:self-end"
                :class="selected.approval.canApprove ? 'bg-success-bg text-success' : 'bg-warning-bg text-warning'"
                role="status"
              >
                <span class="size-2 rounded-full" :class="selected.approval.canApprove ? 'bg-success' : 'bg-warning'" />
                <span class="text-title-md">
                  {{ selected.approval.canApprove ? (isOwner ? 'อนุมัติได้' : 'อยู่ในวงเงิน (อนุมัติได้ทันที)') : selected.approval.message }}
                </span>
              </span>
            </div>

            <!-- Limit gauge -->
            <div class="flex flex-col gap-2 pb-2">
              <div class="flex flex-wrap items-center justify-between gap-2 text-body-sm">
                <span class="text-muted">
                  สัดส่วนวงเงินของสาขา:
                  <strong class="tabular" :class="remaining >= 0 ? 'text-primary' : 'text-warning'">{{ usedPercent }}%</strong>
                  <span class="tabular" :class="remaining >= 0 ? 'text-primary' : 'text-warning'">
                    ({{ remaining >= 0 ? `เหลือเพดาน ${formatBaht(remaining)}` : `เกินเพดาน ${formatBaht(-remaining)}` }})
                  </span>
                </span>
                <span class="flex items-center gap-1 font-semibold text-ink tabular"><AppIcon name="flag" :size="16" class="text-muted" /> เพดานอนุมัติ {{ formatBaht(selected.approval.limit) }}</span>
              </div>
              <div class="relative h-3.5 w-full overflow-hidden rounded-full bg-hairline/70 p-0.5" role="meter" :aria-valuenow="usedPercent" aria-valuemin="0" aria-valuemax="100" aria-label="สัดส่วนวงเงิน">
                <div class="h-full rounded-full transition-all duration-500" :class="remaining >= 0 ? 'bg-primary' : 'bg-warning'" :style="{ width: `${Math.min(usedPercent, 100)}%` }" />
                <div class="absolute inset-y-0 right-0 w-1 rounded-full bg-muted" />
              </div>
              <div class="flex justify-between text-label-sm text-muted">
                <span>฿0 (ฐานคำนวณ)</span>
                <span>จุดตัดสิทธิ์อนุมัติระดับสาขา (฿5,000)</span>
              </div>
            </div>
          </div>

          <!-- Lines -->
          <div v-if="showLines" class="relative border-t border-hairline">
            <table class="w-full text-body-sm">
              <thead class="text-left text-label-md text-muted">
                <tr>
                  <th class="px-6 py-3 font-semibold md:px-8">สินค้า</th>
                  <th class="px-2 py-3 text-right font-semibold">จำนวน</th>
                  <th class="hidden px-2 py-3 text-right font-semibold sm:table-cell">ราคา/หน่วย</th>
                  <th class="px-6 py-3 text-right font-semibold md:px-8">รวม</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-hairline">
                <tr v-for="line in selected.lines" :key="line.id" class="hover:bg-row-hover">
                  <td class="px-6 py-3 text-ink md:px-8">{{ line.productName }}</td>
                  <td class="whitespace-nowrap px-2 py-3 text-right tabular">{{ line.qtyOrdered }} {{ line.unit }}</td>
                  <td class="hidden px-2 py-3 text-right tabular sm:table-cell">{{ formatBaht(line.unitPrice) }}</td>
                  <td class="px-6 py-3 text-right font-semibold text-ink tabular md:px-8">{{ formatBaht(line.lineTotal) }}</td>
                </tr>
              </tbody>
              <tfoot class="text-body-sm">
                <tr class="border-t border-hairline">
                  <td colspan="3" class="hidden px-6 py-2 text-right text-muted sm:table-cell md:px-8">VAT 7%</td>
                  <td colspan="2" class="px-6 py-2 text-right text-muted sm:hidden">VAT 7%</td>
                  <td class="px-6 py-2 text-right tabular md:px-8">{{ formatBaht(selected.vatAmount) }}</td>
                </tr>
              </tfoot>
            </table>
          </div>

          <!-- Reject reason -->
          <div v-if="rejecting" class="relative border-t border-hairline px-6 py-4 md:px-8">
            <label for="reject-reason" class="text-title-md text-ink">เหตุผลที่ตีกลับ <span class="text-danger">*</span></label>
            <textarea
              id="reject-reason"
              v-model="reason"
              rows="2"
              maxlength="500"
              class="mt-2 w-full rounded border border-hairline bg-canvas p-3 text-body-md"
              placeholder="เช่น ราคาน้ำดื่มสูงกว่ารอบก่อน"
            />
          </div>

          <!-- Actions panel -->
          <div class="relative flex flex-col items-stretch justify-between gap-4 bg-canvas/60 px-6 py-5 sm:flex-row sm:items-center md:px-8">
            <button type="button" class="inline-flex items-center gap-2 py-2 text-title-md text-primary hover:underline" :aria-expanded="showLines" @click="showLines = !showLines">
              <AppIcon :name="showLines ? 'expand_less' : 'format_list_bulleted'" />
              {{ showLines ? 'ซ่อนรายการสินค้า' : `ตรวจสอบรายการสินค้า (${selected.lines.length} SKU)` }}
            </button>
            <div class="flex items-center gap-3">
              <template v-if="rejecting">
                <button type="button" class="min-h-11 flex-1 rounded-full bg-surface px-6 text-title-md text-ink shadow-sm hover:bg-hairline/60 sm:flex-none" :disabled="busy" @click="rejecting = false">ยกเลิก</button>
                <button type="button" class="inline-flex min-h-11 flex-1 items-center justify-center gap-2 rounded-full bg-danger px-6 text-title-md text-surface shadow-md disabled:opacity-60 sm:flex-none" :disabled="busy || !reason.trim()" @click="decide('reject')">
                  <AppIcon name="undo" :size="18" /> ยืนยันตีกลับ
                </button>
              </template>
              <template v-else>
                <button type="button" class="min-h-11 flex-1 rounded-full bg-surface px-6 text-title-md text-ink shadow-sm hover:bg-hairline/60 disabled:opacity-50 sm:flex-none" :disabled="busy || !selected.approval.canApprove" @click="rejecting = true">ตีกลับใบสั่งซื้อ</button>
                <button type="button" class="inline-flex min-h-11 flex-1 items-center justify-center gap-2 rounded-full bg-primary px-8 text-title-md text-surface shadow-md transition-all hover:-translate-y-0.5 hover:bg-primary-hover active:translate-y-0 disabled:translate-y-0 disabled:opacity-50 sm:flex-none" :disabled="busy || !selected.approval.canApprove" @click="decide('approve')">
                  <AppIcon name="check_circle" :size="18" /> {{ busy ? 'กำลังบันทึก…' : 'อนุมัติใบสั่งซื้อ' }}
                </button>
              </template>
            </div>
          </div>
        </article>

        <p class="text-body-sm text-muted">
          <AppIcon name="history" :size="16" class="mr-1 align-[-3px]" /> ทุกการอนุมัติ/ตีกลับถูกบันทึกใน audit log (MongoDB) ·
          <button type="button" class="text-primary hover:underline disabled:opacity-60" :disabled="busy" @click="resetDemo">รีเซ็ตข้อมูลเดโม</button>
        </p>

        <ExpiringPreview />
      </div>

      <!-- RIGHT: tactical panel -->
      <div class="xl:col-span-4">
        <ManagerSidePanel />
      </div>
    </div>

    <!-- Toast -->
    <div
      v-if="toast"
      class="fixed inset-x-4 bottom-4 z-50 mx-auto flex max-w-md items-center gap-2 rounded-full px-5 py-3 text-title-md shadow-modal"
      :class="toast.tone === 'success' ? 'bg-success-bg text-success' : 'bg-danger-bg text-danger'"
      role="status"
      aria-live="polite"
    >
      <AppIcon :name="toast.tone === 'success' ? 'check_circle' : 'error'" /> {{ toast.text }}
    </div>
  </div>
</template>
