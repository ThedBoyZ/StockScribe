const baht = new Intl.NumberFormat('th-TH', { style: 'currency', currency: 'THB', maximumFractionDigits: 2, minimumFractionDigits: 0 })

/** 4200 → "฿4,200" · 4494.5 → "฿4,494.5" (api-spec: the API sends numbers, the screen formats). */
export function formatBaht(amount: number): string {
  return baht.format(amount)
}

/** Today in Bangkok, e.g. "พฤ. 20 พ.ย. 2026" (Gregorian year, as in the designs). */
export function todayInBangkok(): string {
  return new Intl.DateTimeFormat('th-TH-u-ca-gregory', {
    timeZone: 'Asia/Bangkok',
    weekday: 'short',
    day: 'numeric',
    month: 'short',
    year: 'numeric',
  }).format(new Date())
}

/** Message from an RFC 9457 ProblemDetails error thrown by $fetch, or a fallback. */
export function problemTitle(error: unknown, fallback: string): string {
  const data = (error as { data?: { title?: string } } | null)?.data
  return typeof data?.title === 'string' && data.title.length > 0 ? data.title : fallback
}
