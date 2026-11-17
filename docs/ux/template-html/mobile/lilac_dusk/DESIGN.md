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
    fontSize: 36px
    fontWeight: '700'
    lineHeight: 44px
    letterSpacing: -0.03em
  display-hero-mobile:
    fontFamily: Plus Jakarta Sans
    fontSize: 28px
    fontWeight: '700'
    lineHeight: 36px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
    letterSpacing: -0.02em
  headline-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
    letterSpacing: -0.01em
  headline-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 18px
    fontWeight: '600'
    lineHeight: 24px
  body-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  body-sm:
    fontFamily: Plus Jakarta Sans
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 16px
  label-lg:
    fontFamily: Plus Jakarta Sans
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
    letterSpacing: 0.01em
  label-md:
    fontFamily: Plus Jakarta Sans
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
  code-md:
    fontFamily: JetBrains Mono
    fontSize: 13px
    fontWeight: '500'
    lineHeight: 18px
  code-sm:
    fontFamily: JetBrains Mono
    fontSize: 11px
    fontWeight: '500'
    lineHeight: 14px
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  gutter: 1rem
  margin: 1.25rem
  space-2xs: 0.25rem
  space-xs: 0.5rem
  space-sm: 0.75rem
  space-md: 1rem
  space-lg: 1.25rem
  space-xl: 1.5rem
  space-2xl: 2rem
---

## Brand & Style

The design system projects quiet confidence, poise, and tactile precision. Designed specifically for focused mobile experiences, it blends contemporary minimalism with warm, tactile card layers to banish clinical starkness in favor of an inviting, human editorial aesthetic.

### Brand Personality & Core Atmosphere
- **Composed & Intentional:** Generous negative space, decisive typographic weightings, and purposeful color anchors create an unhurried, thoughtful journey.
- **Warm Architectural Depth:** Grounded by mineral off-whites, atmospheric deep slate banners, and softened lilac accents, avoiding hyper-saturated tech gradients.
- **Selective Focus:** Chromatic energy is rigorously gated. High-chroma accents like the luminous sky tone are reserved for solitary, high-impact hero metrics per viewport.

### Target Audience & Emotional Impact
Designed for discerning mobile users managing workflows, personal rituals, or metrics where clutter creates anxiety. The interface induces grounded clarity, seamless spatial orientation, and visual calm.

## Colors

The palette balances warm organic undertones with atmospheric twilight slates and focused lilac vectors. Color roles are strictly hierarchical to prevent visual competition.

### Core Roles & Tokens
- **Canvas Base (`#F0EEE9`):** "Cloud Dancer" warm off-white, applied to viewport foundations and behind-card structural valleys.
- **Surface Elevation (`#FAF9F6`):** Pure warm surface tint for all cards, bottom sheets, and elevated overlays.
- **Hero & Structural Anchor (`#494D5F`):** Deep slate dedicated to mobile status bars, header panels, top navigation bars, and primary sectional headers.
- **Typography Base:**
  - Ink (`#2E3140`): Primary high-contrast readability across all body, inputs, and dark labels.
  - Muted Slate (`#6B6F80`): Secondary descriptors, timestamps, helper labels, and inactive iconography.
- **Primary Interactive (`#7A4FB0`):** Action-driving energetic violet for primary buttons, active icon fills, active navigation tabs, links, and accessibility focus borders. States shift to `#6A41A0` on active press.
- **Lilac Sub-Layers:**
  - Wash (`#E8DFFA`): Backgrounds for selected rows, counter pills, secondary buttons, and active bottom-tab backplates.
  - Line (`#D0BDF4`): Active selection outlines, progress indicator tracks, and highlighted radio/checkbox borders.
- **Singular Sky Focal Accent (`#A0D2EB`):** Dedicated to **one single element per screen view** (e.g., a primary metric display card or singular key indicator), always styled with `#2E3140` typography atop it.
- **Hairline Border (`#E2DFD8`):** Restrained, low-contrast structural divisor for cards, dividers, and inactive state borders.

### Functional Status System
Semantic chips and banners use high-contrast tinted pairings:
- **Success:** Text `#11734A` atop surface fill `#DDF3E8`
- **Warning:** Text `#8A5A00` atop surface fill `#FFF0CC`
- **Danger:** Text `#C2303A` atop surface fill `#FFE3E5`
- **Neutral:** Text `#5D6285` atop surface fill `#ECEDF2`

## Typography

The typographic system orchestrates clear hierarchy using **Plus Jakarta Sans** for core editorial and UI roles, paired with **JetBrains Mono** for data, financial values, keys, and transaction references. (Thai-language scripts map 1:1 to **IBM Plex Sans Thai** across matching weights).

### Editorial Hierarchy
- **Header Blocks:** Headlines use negative letter-spacing for tight structural cohesion on mobile displays. Headlines over Deep Slate (`#494D5F`) hero regions must render in white or tinted off-white (`#FAF9F6`).
- **Body & Content:** Standard body text maintains a generous 1.4-1.5x line height ratio to guarantee legibility across smaller handheld viewports.
- **Monospaced Data:** JetBrains Mono is deployed for identifiers, balance readouts, dates, and telemetry tags, delivering technical rigor alongside the approachable sans-serif baseline.

## Layout & Spacing

The interface uses a 4-column fluid mobile grid anchored by consistent margin buffers and an 8pt structural rhythm.

### Mobile Grid Mechanics
- **Columns:** 4 fluid columns on mobile devices (`<600px`), expanding to 8 columns on tablet viewports.
- **Horizontal Screen Padding:** Fixed at `1.25rem` (20px) to anchor touch controls cleanly inside the hardware bezel boundaries.
- **Column Gutter:** Fixed at `1rem` (16px) between adjacent vertical cards and multi-column widgets.

### Spacing Tokens
- `space-2xs` (4px): Fine icon-to-label spacing, badge interior padding.
- `space-xs` (8px): Chip internal spacing, compact stack gaps.
- `space-sm` (12px): Standard form field vertical padding, small list item spacing.
- `space-md` (16px): Standard element stack padding, card interior body padding.
- `space-lg` (20px): Structural card interior padding, primary gap between grouped cards.
- `space-xl` (24px): Inter-section spacing.
- `space-2xl` (32px): Hero section bottom margin, major structural breaks.

## Elevation & Depth

Visual hierarchy uses tactile physical layering rather than dramatic drop shadows. The design emphasizes natural stack order using tonal contrasts and whisper-light containment lines.

### Tonal Stratification
- **Level 0 (Canvas):** Ground layer in `#F0EEE9`. Unclickable foundation.
- **Level 1 (Card Surfaces):** `#FAF9F6` with a crisp hairline border (`1px solid #E2DFD8`). A micro-tinted shadow (`0 2px 8px -2px rgba(46, 49, 64, 0.04)`) separates card panels from the canvas.
- **Level 2 (Active Cards & Overlays):** Interactive selected surfaces adopt the Lilac line border (`1.5px solid #D0BDF4`) with a localized soft glow (`0 4px 16px -4px rgba(122, 79, 176, 0.08)`).
- **Level 3 (Modals & Bottom Drawers):** `#FAF9F6` base layered over a dimmed backdrop (`rgba(46, 49, 64, 0.4)`), lifted by `0 -8px 24px -4px rgba(46, 49, 64, 0.12)`.

## Shapes

The design system applies a disciplined, triple-tier corner radius language that mirrors organic ergonomics:

- **Large Structural Enclosures (20px / `1.25rem`):** Applied exclusively to cards, actionable containers, and bottom sheets.
- **Controls & Form Fields (12px / `0.75rem`):** Applied to primary/secondary buttons, text inputs, dropdown triggers, and interactive list cells.
- **Pills & Badges (9999px):** Applied to active navigation pills, steppers, unit chips, status tags, and avatar frames.

## Components

### Buttons
- **Primary Action:** Background `#7A4FB0`, text `#FAF9F6`, 12px border-radius, minimum height 48px for touch ergonomics. Pressed/hover state: `#6A41A0`. Focus state: 2px solid `#7A4FB0` with a 2px offset.
- **Secondary Action:** Background `#E8DFFA`, text `#7A4FB0`, border 1px solid `#D0BDF4`, 12px border-radius. Pressed state: `#D0BDF4`.
- **Text Button:** Background transparent, text `#7A4FB0`, semibold font weight, no border.

### Inputs & Form Fields
- **Container:** Background `#FAF9F6`, height 52px, border-radius 12px, border `1px solid #E2DFD8`.
- **Typography:** Value text `#2E3140`, placeholder text `#6B6F80`.
- **Focused State:** Border `1.5px solid #7A4FB0` with an outer focus ring `rgba(122, 79, 176, 0.15)`.

### Cards & Hero Blocks
- **Standard Card:** Background `#FAF9F6`, border-radius 20px, border `1px solid #E2DFD8`, padding `1.25rem`.
- **Selected Card:** Border `1.5px solid #D0BDF4`, background `#FAF9F6` with subtle `#E8DFFA` tint highlight.
- **Sky Hero Metric Card:** One single hero card or focal badge per screen uses background `#A0D2EB`, border-radius 20px, with typography rendered in `#2E3140`.
- **Header Block:** Background `#494D5F`, border-radius 0px on top with optional 24px bottom corners when terminating into the page canvas. Headline text `#FAF9F6`.

### Chips, Pills & Steppers
- **Active Navigation Pill:** Background `#E8DFFA`, icon & label `#7A4FB0`, border-radius 9999px.
- **Unit / Filter Chip (Default):** Background `#FAF9F6`, border `1px solid #E2DFD8`, text `#6B6F80`, border-radius 9999px.
- **Unit / Filter Chip (Active):** Background `#E8DFFA`, border `1px solid #D0BDF4`, text `#7A4FB0`, border-radius 9999px.
- **Numeric Counter / Stepper:** Lilac tint `#E8DFFA` capsule with `#7A4FB0` icon controls.

### Selection Controls
- **Checkboxes & Radios:** Unchecked: border `1.5px solid #6B6F80`, background `#FAF9F6`. Checked: background `#7A4FB0`, border `#7A4FB0`, check/dot glyph in `#FAF9F6`.
- **Progress Track:** Inactive track `#E2DFD8`, active fill `#7A4FB0` with intermediate milestones in `#D0BDF4`.

### Lists
- **List Items:** Background `#FAF9F6`, separated by `1px solid #E2DFD8` dividers inset by 16px. Active row selection renders with background `#E8DFFA` and text `#2E3140`.