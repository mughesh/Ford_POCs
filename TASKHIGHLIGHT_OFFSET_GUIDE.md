# TaskHighlight - Offset Controls Guide

## Changes Made

✅ **Removed**: Grab/release auto-hide (was not what you wanted)
✅ **Kept**: Auto-hide when task completes (default behavior)
✅ **Added**: Offset controls with editor preview
✅ **Added**: Multiple Task IDs support

---

## How Highlight Hiding Works Now

**Simple:**
```
Task becomes active → Highlight shows
Task completes → Highlight hides automatically
```

No grab/release detection. Clean and simple!

---

## New Offset Controls

### What They Do:

Control the position and scale of the highlight mesh overlay:

- **Highlight Position Offset**: Move highlight mesh relative to original (X, Y, Z)
- **Highlight Scale Offset**: Scale highlight mesh independently per axis (X, Y, Z)
- **Enable Offset Preview**: Checkbox to preview changes in Play mode

---

## Usage: Adjusting Highlight Offset

### Step 1: Setup Component

Select your Holding Bar GameObject with TaskHighlight component:

```
TaskHighlight Component:
├─ Task Settings:
│  └─ Show On Tasks: [Size: 7]
│     ├─ S2_04_GrabAndPressButton
│     ├─ S2_05_SlideToolRight
│     ├─ S2_07_PushToolForward
│     ├─ S2_11_RetractTool
│     ├─ S2_23_RetractToolBackward
│     ├─ S2_24_SlideToolLeftPark
│     └─ S2_26_GrabAndPressReturn
│
├─ Highlight Offset Controls (Editor Only):
│  ├─ Enable Offset Preview: □ (unchecked initially)
│  ├─ Highlight Position Offset: (0, 0, 0)
│  └─ Highlight Scale Offset: (1, 1, 1)
│
└─ Highlight Settings:
   ├─ Highlight Material: [Your highlight material]
   ├─ Add As Overlay: ✓ (checked)
   └─ Highlight Scale: 1.02
```

### Step 2: Enter Play Mode

1. **Start Play mode**
2. **Navigate to one of the 7 steps** where highlight shows
3. **Select your Holding Bar** GameObject in hierarchy

### Step 3: Enable Preview & Adjust

1. **Check** `Enable Offset Preview` checkbox
2. **Highlight will appear** immediately
3. **Adjust values** in real-time:
   - `Highlight Position Offset`: Move highlight (e.g., `(0, 0.1, 0)` moves up)
   - `Highlight Scale Offset`: Scale highlight (e.g., `(1.1, 1.1, 1.1)` makes bigger)
4. **See changes immediately** as you adjust sliders

### Step 4: Save Values & Exit Play Mode

1. **Copy/remember** the offset values you like
2. **Exit Play mode**
3. **Re-enter the same values** in edit mode
4. **Uncheck** `Enable Offset Preview`

**Important:** Unity doesn't save changes made in Play mode, so copy your values first!

---

## Examples

### Example 1: Move Highlight Up Slightly
```
Highlight Position Offset: (0, 0.05, 0)
Highlight Scale Offset: (1, 1, 1)
```

### Example 2: Make Highlight Bigger on X/Z, Same on Y
```
Highlight Position Offset: (0, 0, 0)
Highlight Scale Offset: (1.2, 1, 1.2)
```

### Example 3: Offset Forward and Scale Up
```
Highlight Position Offset: (0, 0, 0.1)
Highlight Scale Offset: (1.1, 1.1, 1.1)
```

---

## Alternative: Manual Highlight Mesh

If you prefer **full manual control**, you can create your own highlight mesh:

### Option A: Use Simple GameObject Mode

1. Create a duplicate of your Holding Bar
2. Name it: `HoldingBar_Highlight`
3. Apply highlight material to it
4. Position/scale it as you want
5. Make it a child of original Holding Bar (or separate)
6. On this highlight GameObject, add `TaskHighlight` component:

```
TaskHighlight Component:
├─ Show On Tasks: [Same 7 Task IDs]
├─ Simple GameObject Mode: ✓ (checked)
└─ [Ignore other settings]
```

**How it works:**
- Task becomes active → Highlight GameObject enables
- Task completes → Highlight GameObject disables

**Advantages:**
- Full manual control over mesh, material, position, scale
- No automatic offset calculations
- Can use different mesh shape if desired

---

## Workflow Comparison

### Automatic Overlay (Default):
```
✅ Quick setup
✅ Automatically creates highlight mesh
✅ Offset controls for fine-tuning
❌ Limited to duplicating original mesh
```

### Simple GameObject Mode (Manual):
```
✅ Full manual control
✅ Any mesh shape/size/material
✅ Position exactly where you want
❌ More manual setup required
```

---

## Troubleshooting

### Issue: "Preview doesn't show in Play mode"

**Check:**
1. `Enable Offset Preview` is checked
2. You're in Play mode (not edit mode)
3. Navigate to one of the 7 tasks where highlight should appear
4. Select the GameObject while in Play mode

### Issue: "Changes are lost when I exit Play mode"

**Solution:**
- Unity doesn't save changes made in Play mode
- Copy your offset values before exiting
- Re-enter them in edit mode

### Issue: "I want more control over highlight positioning"

**Solution:**
- Use Simple GameObject Mode instead
- Create your own highlight mesh manually
- Full control over everything

---

## Summary

✅ **Hide behavior**: Highlight hides when task completes (not on grab/release)
✅ **Multiple tasks**: One component handles 7 Task IDs
✅ **Offset controls**: Position and scale adjustments with live preview
✅ **Alternative**: Manual highlight mesh with Simple GameObject Mode

Choose whichever workflow suits your needs!
