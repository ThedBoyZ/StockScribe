// Every page needs a session except /login. The API checks permissions on every call anyway;
// this guard only decides which page to show.
export default defineNuxtRouteMiddleware(async (to) => {
  const auth = useAuthStore()
  if (!auth.checked) await auth.fetchMe()

  if (to.path === '/login') {
    return auth.me ? navigateTo('/') : undefined
  }
  if (!auth.me) {
    return navigateTo('/login')
  }
})
