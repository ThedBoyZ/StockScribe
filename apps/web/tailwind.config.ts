import type { Config } from 'tailwindcss'

/**
 * Lilac Dusk design tokens — source of truth: docs/ux/DESIGN.md.
 * Loaded by Tailwind v4 through `@config` in app/assets/css/main.css.
 *
 * Usage rules (DESIGN.md):
 *  - `primary` (solid) only for the main action, max 1–2 per screen; others use `lilac-tint` or outline.
 *  - Status colours always come with text or an icon, never colour alone.
 */
export default {
  content: ['./app/**/*.{vue,ts}'],
  theme: {
    extend: {
      colors: {
        canvas: '#F0EEE9',
        surface: '#FAF9F6',
        primary: {
          DEFAULT: '#7A4FB0',
          hover: '#693E9E',
          active: '#5A3387',
        },
        slate: '#494D5F',
        ink: '#2E3140',
        muted: '#6B6F80',
        'lilac-tint': {
          DEFAULT: '#E8DFFA',
          hover: '#DECFF7',
        },
        'lilac-line': '#D0BDF4',
        'row-hover': '#F5F2FC',
        sky: '#A0D2EB',
        hairline: '#E2DFD8',

        success: { DEFAULT: '#11734A', bg: '#DDF3E8' },
        warning: { DEFAULT: '#8A5A00', bg: '#FFF0CC' },
        danger: { DEFAULT: '#C2303A', bg: '#FFE3E5' },
        neutral: { DEFAULT: '#5D6285', bg: '#ECEDF2' },
      },

      fontFamily: {
        // Plus Jakarta Sans has no Thai glyphs, so Thai always falls back to IBM Plex Sans Thai.
        display: ['"Plus Jakarta Sans"', '"IBM Plex Sans Thai"', 'sans-serif'],
        sans: ['"IBM Plex Sans"', '"IBM Plex Sans Thai"', 'system-ui', 'sans-serif'],
        mono: ['"JetBrains Mono"', 'ui-monospace', 'monospace'],
      },

      // Web (W) sizes in px. Mobile (M) uses the smaller step via responsive classes,
      // e.g. `text-headline-sm md:text-headline-md`.
      fontSize: {
        'display-hero': ['40px', { lineHeight: '48px', fontWeight: '700' }],
        'headline-lg': ['32px', { lineHeight: '40px', fontWeight: '700' }],
        'headline-md': ['24px', { lineHeight: '32px', fontWeight: '600' }],
        'headline-sm': ['20px', { lineHeight: '28px', fontWeight: '600' }],
        'title-md': ['16px', { lineHeight: '24px', fontWeight: '600' }],
        'body-lg': ['16px', { lineHeight: '26px' }],
        'body-md': ['14px', { lineHeight: '22px' }],
        'body-sm': ['13px', { lineHeight: '18px' }],
        'label-md': ['12px', { lineHeight: '16px', fontWeight: '600', letterSpacing: '0.02em' }],
        'label-sm': ['11px', { lineHeight: '14px', fontWeight: '600', letterSpacing: '0.04em' }],
        'metric-focal': ['32px', { lineHeight: '36px', fontWeight: '800' }],
      },

      borderRadius: {
        DEFAULT: '8px', // buttons, inputs, alerts
        lg: '16px', // cards, tables, hero
        xl: '24px', // modals, bottom sheets
      },

      boxShadow: {
        // Level 1 (cards) uses a hairline border, no shadow.
        dropdown: '0 4px 16px -2px rgba(46,49,64,.08), 0 1px 3px rgba(46,49,64,.04)',
        modal: '0 20px 40px -8px rgba(46,49,64,.16)',
        focus: '0 0 0 3px rgba(122,79,176,.2)',
      },
    },
  },
} satisfies Config
