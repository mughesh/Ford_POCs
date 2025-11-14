# TaskHighlight - Hide Behavior Guide

## Simple Solution: One Checkbox

Added one checkbox to control when highlight hides:

✅ **Hide On Grab**: Checkbox to control hide behavior per step

---

## Two Hide Behaviors

### Behavior 1: Hide on Task Completion (Default)

**Use for:** Animation/movement steps where highlight should stay visible during animations

**Setup:**
```
TaskHighlight Component:
├─ Hide Behavior:
│  └─ Hide On Grab: □ (unchecked)
└─ [Rest of settings]
```

**Flow:**
```
Task starts → Highlight shows
User grabs → Highlight stays visible
Animations play → Highlight still visible
Task completes → Highlight hides
```

**Example Steps:**
- Step 4: Grab + Press Button + Release → Tool slides (highlight stays until tool finishes moving)
- Step 5: Grab + Press Button + Release → Tool moves right (highlight stays during animation)
- Step 17: Rotate knob → Hooks lock (highlight stays during locking animation)

---

### Behavior 2: Hide on Grab (Immediate)

**Use for:** Grab-only steps where the grab IS the interaction

**Setup:**
```
TaskHighlight Component:
├─ Hide Behavior:
│  ├─ Hide On Grab: ✓ (checked)
│  └─ Grabbable: [Leave empty - auto-finds]
└─ [Rest of settings]
```

**Flow:**
```
Task starts → Highlight shows
User grabs → Highlight hides IMMEDIATELY
Task continues/completes → (highlight already hidden)
```

**Example Steps:**
- Step 7: Grab handle + Release → Tool moves (highlight hides as soon as grabbed)
- Step 11: Grab handle + Release → Tool retracts (highlight hides on grab)

---

## Setup for Your 7 Steps (Holding Bar)

Since you have **one component for 7 tasks**, you need to decide which behavior fits MOST of your steps.

### Option A: Mostly Animation Steps

If **most** of your 7 steps involve animations/movements:

```
TaskHighlight Component:
├─ Show On Tasks: [Size: 7]
│  ├─ S2_04_GrabAndPressButton (grab + button + release → animation)
│  ├─ S2_05_SlideToolRight (grab + button + release → animation)
│  ├─ S2_07_PushToolForward (grab + release → animation)
│  ├─ S2_11_RetractTool (grab + release → animation)
│  ├─ S2_23_RetractToolBackward (grab + button + release → animation)
│  ├─ S2_24_SlideToolLeftPark (grab + button + release → animation)
│  └─ S2_26_GrabAndPressReturn (grab + button + release → animation)
│
└─ Hide Behavior:
   └─ Hide On Grab: □ (UNCHECKED - hide on task completion)
```

**Result:** Highlight stays visible during all animations, hides when task completes.

---

### Option B: Mostly Grab-Only Steps

If **most** of your 7 steps are grab-only (unlikely based on your descriptions):

```
Hide On Grab: ✓ (CHECKED - hide immediately on grab)
```

---

### Option C: Mixed Steps (Requires Multiple Components)

If you have a **mix** of both behaviors, you need **two separate components**:

#### Component 1: Animation Steps (Hide on Completion)
```
TaskHighlight #1:
├─ Show On Tasks: [Steps with animations]
│  ├─ S2_04_GrabAndPressButton
│  ├─ S2_05_SlideToolRight
│  ├─ S2_23_RetractToolBackward
│  └─ S2_24_SlideToolLeftPark
│
└─ Hide On Grab: □ (unchecked)
```

#### Component 2: Grab-Only Steps (Hide on Grab)
```
TaskHighlight #2:
├─ Show On Tasks: [Grab-only steps]
│  ├─ S2_07_PushToolForward
│  ├─ S2_11_RetractTool
│  └─ S2_26_GrabAndPressReturn
│
└─ Hide On Grab: ✓ (checked)
```

---

## Quick Decision Guide

**Question:** Do your 7 steps have animations that play AFTER you release the handle?

**Yes, most/all do** → Use **one component** with `Hide On Grab: UNCHECKED`

**No, most are grab-only** → Use **one component** with `Hide On Grab: CHECKED`

**Mixed (some with, some without)** → Use **two components** with different Task ID lists

---

## Examples

### Example 1: Step 4 (Grab + Button + Release → Tool Slides)
```
Hide On Grab: □ (unchecked)
```
- Highlight stays visible while you grab, press button, release
- Highlight stays visible while tool animates to target
- Highlight hides when tool reaches target (task completes)

### Example 2: Step 7 (Grab + Release → Immediate Movement)
```
Hide On Grab: ✓ (checked)
```
- Highlight hides immediately when you grab
- Clean visual feedback
- Task completes when you release

---

## Recommendation for Your 7 Steps

Based on your descriptions, **all 7 steps** seem to involve animations/movements after interaction.

**Recommended Setup:**
```
ONE TaskHighlight component:
├─ Show On Tasks: [All 7 Task IDs]
└─ Hide On Grab: □ (UNCHECKED)
```

**Result:**
- Highlight shows when task starts
- Highlight stays visible during grab, button press, release
- Highlight stays visible during tool animation
- Highlight hides when animation completes (task completes)

Simple, clean, and works for all your steps!

---

## Summary

✅ **One checkbox**: `Hide On Grab`
✅ **Two behaviors**:
   - Unchecked = Hide on task completion (for animation steps)
   - Checked = Hide immediately on grab (for grab-only steps)
✅ **Simple decision**: Most steps = one setting, mixed steps = two components

Choose based on your step types!
