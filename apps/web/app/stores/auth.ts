// Session state. The token itself lives in the HttpOnly sf_session cookie (ADR-0003),
// so the browser can't read it; we only keep the `me` profile the API returns.

export type Role = 'Staff' | 'BranchManager' | 'Purchasing' | 'Owner' | 'Admin'

export interface BranchSummary {
  id: string
  name: string
}

export interface Me {
  id: string
  name: string
  role: Role
  branch: BranchSummary | null
}

export interface Persona extends Me {
  key: string
}

export const useAuthStore = defineStore('auth', () => {
  const me = ref<Me | null>(null)
  /** True once we asked the API who we are (so the route guard asks only once per page load). */
  const checked = ref(false)

  async function fetchMe(): Promise<Me | null> {
    try {
      me.value = await $fetch<Me>('/api/v1/me')
    } catch {
      // 401 = no session. Network errors also land here; the login page shows its own error.
      me.value = null
    } finally {
      checked.value = true
    }
    return me.value
  }

  async function loginAs(personaId: string): Promise<Me> {
    const result = await $fetch<Me>('/api/v1/auth/demo-login', { method: 'POST', body: { personaId } })
    me.value = result
    checked.value = true
    return result
  }

  async function logout(): Promise<void> {
    try {
      await $fetch('/api/v1/auth/logout', { method: 'POST' })
    } finally {
      me.value = null
    }
  }

  return { me, checked, fetchMe, loginAs, logout }
})
