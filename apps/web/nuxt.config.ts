import tailwindcss from '@tailwindcss/vite'

// API used by `npm run dev` (dotnet run --launch-profile http). Override with NUXT_DEV_API_URL.
const devApiUrl = process.env.NUXT_DEV_API_URL ?? 'http://localhost:5089'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  // Skip the interactive telemetry prompt; it blocks `nuxt dev` until answered (and hangs in Docker/CI).
  telemetry: false,

  // ADR-0004: static SPA. `nuxt generate` outputs plain files that nginx serves;
  // the browser calls the API same-origin (/api, /health), so no CORS is needed.
  ssr: false,

  modules: ['@pinia/nuxt'],

  css: ['~/assets/css/main.css'],

  vite: {
    plugins: [tailwindcss()],
  },

  app: {
    head: {
      htmlAttrs: { lang: 'th' },
      title: 'StockScribe',
      meta: [
        { name: 'viewport', content: 'width=device-width, initial-scale=1' },
        { name: 'theme-color', content: '#F0EEE9' },
      ],
      link: [
        { rel: 'icon', type: 'image/svg+xml', href: '/favicon.svg' },
        { rel: 'preconnect', href: 'https://fonts.googleapis.com' },
        { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' },
        {
          rel: 'stylesheet',
          // docs/ux/DESIGN.md: headings Plus Jakarta Sans, body IBM Plex Sans Thai + IBM Plex Sans, codes JetBrains Mono
          href: 'https://fonts.googleapis.com/css2?family=IBM+Plex+Sans+Thai:wght@400;500;600&family=IBM+Plex+Sans:wght@400;500;600&family=JetBrains+Mono:wght@500&family=Plus+Jakarta+Sans:wght@600;700;800&display=swap',
        },
        {
          rel: 'stylesheet',
          // Icons used by the designs in docs/ux/template-html (<span class="material-symbols-outlined">name</span>).
          href: 'https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..24,400,0..1,0&display=block',
        },
      ],
    },
  },

  // Dev only: forward API calls from the Nuxt dev server (:3000) to the .NET API.
  // In Docker, nginx does the same job (apps/web/nginx.conf).
  nitro: {
    devProxy: {
      '/api': { target: `${devApiUrl}/api`, changeOrigin: true },
      '/health': { target: `${devApiUrl}/health`, changeOrigin: true },
    },
  },
})
