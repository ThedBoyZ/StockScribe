# StockScribe Web

Nuxt 4 SPA (`ssr: false`, ADR-0004) · Tailwind CSS v4 · Pinia · Lilac Dusk tokens in `tailwind.config.ts` (from `docs/ux/DESIGN.md`).

```bash
npm install
npm run dev        # http://localhost:3000 · /api and /health are proxied to the API (default http://localhost:5089)
npm run generate   # static output in .output/public (served by nginx in Docker)
```

Override the dev API target with `NUXT_DEV_API_URL=http://localhost:xxxx npm run dev`.
