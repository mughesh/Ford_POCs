# TaskHighlight Enhancement - Migration Guide

## What Changed?

The `TaskHighlight` script now supports:

✅ **Multiple Task IDs** - One component can show highlight for 5-7 tasks
✅ **Auto-hide on grab/release** - Highlight disappears when you grab and release the object
✅ **Backwards Compatible** - Your existing 7 components will still work

---

## How to Clean Up Your 7 Duplicate Components

### Current Setup (Bad Way):
```
Holding Bar GameObject:
├─ TaskHighlight #1 (Task ID: S2_04_GrabAndPressButton)
├─ TaskHighlight #2 (Task ID: S2_05_SlideToolRight)
├─ TaskHighlight #3 (Task ID: S2_07_PushToolForward)
├─ TaskHighlight #4 (Task ID: S2_11_RetractTool)
├─ TaskHighlight #5 (Task ID: S2_23_RetractToolBackward)
├─ TaskHighlight #6 (Task ID: S2_24_SlideToolLeftPark)
└─ TaskHighlight #7 (Task ID: S2_26_GrabAndPressReturn)
```

### New Setup (Clean Way):
```
Holding Bar GameObject:
└─ TaskHighlight (ONE component with all 7 Task IDs!)
```

---

## Migration Steps

### Step 1: Backup
Before making changes, **duplicate your Holding Bar GameObject** in the scene as a backup (just in case).

### Step 2: Remove 6 Duplicate Components

1. Select your **Holding Bar** GameObject
2. In Inspector, find all the **TaskHighlight** components
3. **Remove/Delete** components #2 through #7 (keep the first one)
4. You should now have only **ONE TaskHighlight component**

### Step 3: Configure the Remaining Component

On the single remaining `TaskHighlight` component:

```
TaskHighlight Component:
├─ Task Settings:
│  ├─ Task ID: [Leave as is - this is deprecated but kept for compatibility]
│  └─ Show On Tasks: [Size: 7]  ← NEW! Add all 7 Task IDs here
│     ├─ Element 0: S2_04_GrabAndPressButton
│     ├─ Element 1: S2_05_SlideToolRight
│     ├─ Element 2: S2_07_PushToolForward
│     ├─ Element 3: S2_11_RetractTool
│     ├─ Element 4: S2_23_RetractToolBackward
│     ├─ Element 5: S2_24_SlideToolLeftPark
│     └─ Element 6: S2_26_GrabAndPressReturn
│
├─ Auto-Hide on Interaction:               ← NEW FEATURE!
│  ├─ Hide On Grab Release: ✓ (checked)   ← Enable this!
│  └─ Grabbable: [Leave empty - auto-detects]
│
├─ Highlight Settings:
│  └─ [Keep your existing settings]
│
└─ Simple GameObject Mode:
   └─ [Keep your existing setting]
```

### Step 4: Test

1. **Play** the simulation
2. Navigate to one of the 7 steps (e.g., Step 4)
3. **Verify**: Holding bar is highlighted
4. **Grab** the holding bar
5. **Release** the holding bar
6. **Verify**: Highlight disappears immediately
7. Test 2-3 other steps to confirm

---

## How the New Features Work

### 1. Multiple Task IDs (Show On Tasks)

**Old way:**
- 7 components, each with one Task ID
- Messy and hard to manage

**New way:**
- 1 component with array of 7 Task IDs
- Highlight shows when **ANY** of those tasks are active
- Clean and organized

### 2. Auto-Hide on Grab/Release

**Behavior:**
```
Task becomes active → Highlight shows
User grabs object → (highlight still visible)
User releases object → Highlight hides immediately
```

**How it works:**
- When `Hide On Grab Release` is checked, the script subscribes to the Grabbable's OnGrab/OnRelease events
- On grab: Marks `hasBeenGrabbed = true`
- On release: If object was grabbed and is highlighted, hides the highlight
- Similar to how arrows hide after button press!

**Grabbable Auto-Detection:**
- Leave `Grabbable` field empty
- Script automatically finds Grabbable component on the GameObject or its children
- If you have multiple Grabbables, drag the specific one you want to track

---

## Backwards Compatibility

**Your existing 7 components will continue to work!**

The script still supports the old single `Task ID` field. You can:

**Option A: Keep all 7 components** (not recommended, but works)
- No migration needed
- Just check `Hide On Grab Release` on all 7 components

**Option B: Clean migration** (recommended)
- Follow migration steps above
- Cleaner, easier to maintain

---

## Troubleshooting

### Issue: "Highlight not hiding on release"

**Check:**
1. `Hide On Grab Release` is checked
2. Holding bar has a `Grabbable` component
3. Console shows: `"TaskHighlight: [name] released - hiding highlight"`

### Issue: "Highlight not showing on some tasks"

**Check:**
1. All 7 Task IDs are added to `Show On Tasks` array
2. Task IDs match exactly (check for typos)
3. Console shows: `"Highlight shown on [name] for task [ID]"`

### Issue: "I want highlight to stay visible after grab/release"

**Solution:**
- Uncheck `Hide On Grab Release`
- Highlight will only hide when task completes (old behavior)

---

## Summary

✅ **Enhanced:** One component now handles 7 tasks
✅ **New Feature:** Auto-hide on grab/release
✅ **Backwards Compatible:** Old setup still works
✅ **Cleaner:** Easier to manage and debug

You can now **delete 6 duplicate components** and use a single clean setup! 🎉
