---
name: Lilac Dusk
colors:
  surface: '#fbf8ff'
  surface-dim: '#d7d9ed'
  surface-bright: '#fbf8ff'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f3f2ff'
  surface-container: '#ecedff'
  surface-container-high: '#e5e7fb'
  surface-container-highest: '#e0e1f5'
  on-surface: '#181b29'
  on-surface-variant: '#4b4451'
  inverse-surface: '#2d303f'
  inverse-on-surface: '#efefff'
  outline: '#7c7482'
  outline-variant: '#cdc3d2'
  surface-tint: '#7349a9'
  primary: '#613696'
  on-primary: '#ffffff'
  primary-container: '#7a4fb0'
  on-primary-container: '#eedbff'
  inverse-primary: '#d9b9ff'
  secondary: '#5a5e70'
  on-secondary: '#ffffff'
  secondary-container: '#dee1f8'
  on-secondary-container: '#606476'
  tertiary: '#1c5368'
  on-tertiary: '#ffffff'
  tertiary-container: '#386b81'
  on-tertiary-container: '#bfe9ff'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#eedbff'
  primary-fixed-dim: '#d9b9ff'
  on-primary-fixed: '#2a0054'
  on-primary-fixed-variant: '#5b2f8f'
  secondary-fixed: '#dee1f8'
  secondary-fixed-dim: '#c2c5db'
  on-secondary-fixed: '#171b2b'
  on-secondary-fixed-variant: '#424658'
  tertiary-fixed: '#bee9ff'
  tertiary-fixed-dim: '#9ccee6'
  on-tertiary-fixed: '#001f2a'
  on-tertiary-fixed-variant: '#144c61'
  background: '#fbf8ff'
  on-background: '#181b29'
  surface-variant: '#e0e1f5'
typography:
  display-hero:
    fontFamily: Plus Jakarta Sans
    fontSize: 40px
    fontWeight: '700'
    lineHeight: 48px
    letterSpacing: -0.03em
  display-hero-mobile:
    fontFamily: Plus Jakarta Sans
    fontSize: 30px
    fontWeight: '700'
    lineHeight: 38px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.02em
  headline-lg-mobile:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '700'
    lineHeight: 32px
    letterSpacing: -0.01em
  headline-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.01em
  headline-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
    letterSpacing: 0em
  title-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '600'
    lineHeight: 24px
    letterSpacing: 0em
  body-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 26px
    letterSpacing: 0em
  body-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 22px
    letterSpacing: 0em
  body-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 13px
    fontWeight: '400'
    lineHeight: 18px
    letterSpacing: 0em
  label-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 12px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.02em
  label-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 11px
    fontWeight: '600'
    lineHeight: 14px
    letterSpacing: 0.04em
  metric-focal:
    fontFamily: Plus Jakarta Sans
    fontSize: 32px
    fontWeight: '800'
    lineHeight: 36px
    letterSpacing: -0.03em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  gutter: 1.5rem
  gutter-mobile: 1rem
  margin: 2rem
  margin-mobile: 1rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 1rem
  space-lg: 1.5rem
  space-xl: 2rem
  space-2xl: 3rem
---

## Brand & Style

The design system establishes an atmospheric, poised, and tactile environment tailored for modern inventory, supply chain, and warehouse intelligence. It departs deliberately from cold, generic enterprise SaaS tropes by embracing an editorial, grounded warmth. 

The aesthetic marries **Tactile Minimalism** with **Warm Editorial Structure**:
- **Warm Groundedness:** A luminous, warm canvas (`#F0EEE9`) paired with alabaster card surfaces (`#FAF9F6`) eliminates clinical screen fatigue, lending an approachable yet premium paper-like weight.
- **Poised Authority:** Deep slate (`#494D5F`) anchors key milestones, metrics chapters, and structural framing, establishing institutional trust and architectural clarity.
- **Intentional Vividness:** Lilac purple (`#7A4FB0`) acts as a deliberate, singular focal beacon for highest-priority intent, balanced by ethereal sky accents (`#A0D2EB`) for illuminating live status pulses and focal data markers.
- **Subtle Tactility:** Low-contrast hairlines, fine-tuned radius treatments, and soft tonal nesting provide structural hierarchy without visual clutter.

## Colors

The palette is engineered to replace legacy corporate blue-and-green dashboard tropes with an elevated twilight-inspired spectrum. Color distribution must strictly observe hierarchy and restraint:

### Palette Structure
- **Canvas Base:** `#F0EEE9` (Warm off-white for the root page view).
- **Surface Elevation:** `#FAF9F6` (Alabaster surface for cards, sheets, panels, and floating elements).
- **Primary / Plum Action:** `#7A4FB0` (Reserved for dominant user actions, key progression nodes, and verified states).
- **Deep Slate:** `#494D5F` (Structural hero blocks, high-contrast banners, chapter separators, and primary section headers).
- **Ink / Text Primary:** `#2E3140` (High-contrast typography for core content and labels).
- **Muted Text / Secondary:** `#6B6F80` (Metadata, helper texts, table column headings, and secondary markers).
- **Lilac Tint:** `#E8DFFA` (Active navigational pills, selected table row fills, secondary control backgrounds).
- **Lilac Line:** `#D0BDF4` (Focus rings, selected borders, active state dividers).
- **Sky Accent:** `#A0D2EB` (Focal summary metrics, quick visual anchors, and badge callouts paired strictly with `#2E3140` text for accessibility).
- **Hairline / Border:** `#E2DFD8` (Subtle boundary borders, table cell divisions, and card outlines).

### Semantic & Status System
Status indicators avoid raw primary hues, relying on nuanced tonal pairings:
- **Success:** `#11734A` foreground on `#DDF3E8` badge fill.
- **Warning:** `#8A5A00` foreground on `#FFF0CC` badge fill.
- **Danger:** `#C2303A` foreground on `#FFE3E5` badge fill.
- **Neutral:** `#5D6285` foreground on `#ECEDF2` badge fill.

### Strict Governance Rules
1. Never introduce external emerald greens, electric blues, or saturated yellows as primary brand accents.
2. Limit solid primary plum (`#7A4FB0`) buttons to a strict maximum of 1 to 2 instances per visible view. Secondary interactions must defer to `#E8DFFA` fills or ghost hairline structures.

## Typography

The design system standardizes wholly on **Plus Jakarta Sans**, harnessing its geometric balance, open counterforms, and confident terminal cuts to deliver effortless scanning across dense operational data.

- **Headlines & Titles:** Rendered in weights `600` and `700` with subtle negative tracking (`-0.01em` to `-0.03em`), grounding dashboard titles with a crisp, authoritative silhouette.
- **Body & Data Text:** Set in weight `400` with open line heights (`1.5` to `1.6`) to sustain readability across long data tables and inventory audits. Primary text relies on `#2E3140`, while secondary descriptions utilize `#6B6F80`.
- **Numbers & Metrics:** Large metrics utilize the `metric-focal` token in weight `800`. For financial, inventory count, and tabular data comparison, enable tabular figures (`font-variant-numeric: tabular-nums`) to ensure vertical numeric alignment.
- **Labels & Overlines:** Rendered in weights `600` with increased tracking (`0.02em` to `0.04em`) in uppercase or sentence case for table headings, categorization tags, and pill toggles.

## Layout & Spacing

The layout model is anchored by an 8-point base rhythm, structured as an adaptable fluid grid:

- **Desktop (1024px+):** 12-column responsive fluid grid with `margin: 2rem` and `gutter: 1.5rem`. Layouts accommodate persistent sidebar navigation (width 260px) and multi-card inventory widgets spanning 3, 4, 6, or 12 columns.
- **Tablet (768px - 1023px):** 8-column grid with `margin: 1.5rem` and `gutter: 1.25rem`. Complex metrics collapse to 4-column blocks; navigation switches to a collapsible drawer or slim icon rail.
- **Mobile (<768px):** 4-column fluid grid with `margin-mobile: 1rem` and `gutter-mobile: 1rem`. Multi-column metrics stack into full-width cards or horizontal swipe reels.

### Spacing Principles
- **Internal Card Padding:** Default card containers apply `space-lg` (1.5rem / 24px) for desktop and `space-md` (1rem / 16px) on mobile views.
- **Dense Data Controls:** Table cell spacing, inline chips, and inputs enforce tight vertical cadence (`space-sm` / 8px) with comfortable horizontal buffers (`space-md` / 16px).
- **Section Breaks:** Major workflow chapters and slate hero blocks maintain `space-2xl` (3rem / 48px) separating margins to preserve visual breath.

## Elevation & Depth

Visual hierarchy within the design system avoids heavy drop shadows, instead cultivating depth through **Tonal Layering** and **Hairline Precision**:

- **Level 0 (Canvas Base):** Grounded directly on the `#F0EEE9` page surface. Unbordered and quiet.
- **Level 1 (Card & Content Blocks):** Resting on `#FAF9F6` with a crisp `1px` border of `#E2DFD8`. No shadow is required under typical daylight viewing; separation is achieved by the contrast between `#FAF9F6` and `#F0EEE9`.
- **Level 2 (Dropdowns, Flyouts & Tooltips):** Surface `#FAF9F6` enveloped in a delicate ambient shadow: `box-shadow: 0 4px 16px -2px rgba(46, 49, 64, 0.08), 0 1px 3px 0 rgba(46, 49, 64, 0.04)` combined with an `#E2DFD8` perimeter border.
- **Level 3 (Modal Dialogs & Slide-overs):** Elevated surface `#FAF9F6` with deep ambient diffusion: `box-shadow: 0 20px 40px -8px rgba(46, 49, 64, 0.16)`. Accompanied by a darkened backdrop scrim composed of `#2E3140` with 40% opacity.
- **Hero / Chapter Containers:** Elevated visually through solid saturation via deep slate `#494D5F`, creating an inverted dark chapter within the light layout for core KPIs and contextual headers.

## Shapes

The design system employs a **Rounded** shape language (`roundedness: 2`):

- **Standard Components:** Buttons, inputs, inline select menus, and alerts utilize `0.5rem` (8px) corner rounding, maintaining a clean architectural balance.
- **Containers & Surfaces (`rounded-lg`):** Cards, charts, hero panels, and table shells take `1rem` (16px) corner rounding to soften the workspace geometry.
- **Modals & Drawers (`rounded-xl`):** Flyout panels and overlay dialogs adopt `1.5rem` (24px) corner rounding.
- **Pills & Status Indicators:** Badge components, category chips, active tab pills, and search filters adopt fully rounded circular ends (`rounded-full` / `9999px`) to immediately separate metadata tags from functional rectangular inputs.

## Components

### Buttons
- **Primary Action (Max 1–2 per view):** Background `#7A4FB0`, label `#FAF9F6`, hover `#693E9E`, active `#5A3387`. Corner radius `0.5rem`.
- **Secondary Action:** Background `#E8DFFA`, label `#7A4FB0`, hover `#DECFF7`, border `1px solid transparent`.
- **Outline / Neutral Action:** Background transparent, label `#2E3140`, border `1px solid #E2DFD8`, hover background `#F0EEE9`.
- **Accent Highlight Action:** Background `#A0D2EB`, label `#2E3140` (weight 600), hover `#8EC7E3`. Ideal for rapid stock reconciliation triggers.

### Chips & Badges
- **Status Badges:** Pill-shaped (`rounded-full`), padding `0.25rem 0.75rem`, font `label-sm`.
  - *Success:* `#11734A` on `#DDF3E8`.
  - *Warning:* `#8A5A00` on `#FFF0CC`.
  - *Danger:* `#C2303A` on `#FFE3E5`.
  - *Neutral:* `#5D6285` on `#ECEDF2`.
- **Navigation Filter Chips:** Unselected chips feature `#FAF9F6` background with `#E2DFD8` border and `#6B6F80` text. Selected chips transition to `#E8DFFA` background with `#7A4FB0` text and border `1px solid #D0BDF4`.

### Input Fields & Controls
- **Text Inputs:** Height 40px, background `#FAF9F6`, border `1px solid #E2DFD8`, text `#2E3140`, placeholder `#6B6F80`. 
- **Focus State:** Border color `#7A4FB0`, complemented by an outer halo ring `0 0 0 3px rgba(122, 79, 176, 0.20)`.
- **Checkboxes & Radios:** Size 18px, border `1.5px solid #E2DFD8`, background `#FAF9F6`. Selected state fills with `#7A4FB0`, displaying a crisp white `#FAF9F6` check icon.

### Data Tables & Lists
- **Table Container:** Surface `#FAF9F6`, outer border `1px solid #E2DFD8`, radius `1rem`, overflow hidden.
- **Header Cells:** Background `#F0EEE9`, text `#6B6F80`, typography `label-md`, padding `0.75rem 1rem`, bottom border `1px solid #E2DFD8`.
- **Body Rows:** Base row `#FAF9F6`, alternating row `#FAF9F6`, dividing border `1px solid #E2DFD8`. Hover state `#F5F2FC`.
- **Selected Rows:** Highlighted in lilac tint `#E8DFFA` with an active indicator border in `#7A4FB0`.

### Cards & Metric Tiles
- **Standard Metric Card:** Background `#FAF9F6`, border `1px solid #E2DFD8`, padding `1.5rem`, radius `1rem`. Contains overline label (`label-md`, `#6B6F80`), large value (`metric-focal`, `#2E3140`), and trailing badge.
- **Hero Color Block Card:** Background `#494D5F`, radius `1rem`, padding `1.75rem`. Text colors invert: headers in `#FAF9F6`, subtext in `#E8DFFA`, and accent highlights featuring sky accent `#A0D2EB` badges.