# Transform Snapshot Recorder - Usage Guide

## What It Does

Saves you from the repetitive work of:
1. Entering Play mode
2. Positioning guidance object for Step X
3. Copying transform values in Inspector
4. Exiting Play mode
5. Pasting transform values
6. Repeating for every step

**Instead:**
- Position object in Play mode → Click `Record Snapshot` → Done!
- All snapshots persist after exiting Play mode
- Apply any snapshot to the object anytime

---

## Setup

### 1. Add Component

1. Select your guidance GameObject (e.g., arrow, highlight, instruction panel)
2. Add Component → `TransformSnapshotRecorder`

### 2. Inspector Layout

```
TransformSnapshotRecorder Component:
├─ Recorded Snapshots:                     ← Shows all recorded snapshots
│  ├─ Size: 0 (initially)
│  └─ [Empty list - will fill as you record]
│
├─ Recording Controls:
│  ├─ Next Snapshot Name: "1"              ← Auto-increments (1 → 2 → 3...)
│  └─ Record Snapshot: □                   ← Check to record in Play mode
│
├─ Apply Snapshot:
│  ├─ Apply Snapshot Index: 0              ← Which snapshot to apply
│  └─ Apply Snapshot: □                    ← Check to apply snapshot
│
└─ Utilities:
   └─ Clear All Snapshots: □               ← Delete all recordings
```

---

## Workflow: Recording Guidance Positions

### Example Scenario:
You have an arrow that needs different positions for Steps 1-26.

### Step-by-Step:

#### 1. Enter Play Mode

Press Play button.

#### 2. Navigate to Step 1

Complete tasks until you reach Step 1 where guidance should appear.

#### 3. Position Arrow for Step 1

- Select your arrow GameObject
- Move it to the correct position for Step 1
- Rotate/scale as needed

#### 4. Record Snapshot

In Inspector, **check** the `Record Snapshot` checkbox.

**What happens:**
- Current transform captured as snapshot "1"
- Snapshot added to list
- Next Snapshot Name auto-updates to "2"
- Console logs: `Recorded snapshot '1' at index 0`

#### 5. Move to Step 2

Continue simulation to Step 2.

#### 6. Position Arrow for Step 2

Move arrow to correct position for Step 2.

#### 7. Record Snapshot

**Check** `Record Snapshot` again.

**What happens:**
- Snapshot "2" captured at index 1
- Next Snapshot Name becomes "3"

#### 8. Repeat for All Steps

Continue until you've recorded all 26 positions:
- Step 3 → Position → Record → Snapshot "3" (index 2)
- Step 4 → Position → Record → Snapshot "4" (index 3)
- ...
- Step 26 → Position → Record → Snapshot "26" (index 25)

#### 9. Exit Play Mode

Press Stop button.

**IMPORTANT:** All snapshots are SAVED! They persist after exiting Play mode.

---

## Applying Snapshots (After Recording)

### In Edit Mode:

1. **Select your guidance GameObject**
2. **Set Apply Snapshot Index** to desired snapshot (e.g., `17` for Step 17)
3. **Check Apply Snapshot** checkbox
4. **Transform updates instantly** to that position

### In Play Mode:

Same process works in Play mode too!

---

## Inspector After Recording 26 Steps

```
Recorded Snapshots:
├─ Size: 26
├─ Element 0:
│  ├─ Name: "1"
│  ├─ Position: (0.5, 1.2, 0.3)
│  ├─ Rotation: (0, 0, 0, 1)
│  ├─ Scale: (1, 1, 1)
│  └─ Info: "Pos: (0.500, 1.200, 0.300)
│            Rot: (0.0, 0.0, 0.0)
│            Scale: (1.000, 1.000, 1.000)"
├─ Element 1:
│  ├─ Name: "2"
│  ├─ Position: (0.8, 1.5, 0.2)
│  └─ ...
├─ ...
└─ Element 25:
   ├─ Name: "26"
   └─ ...
```

Each snapshot shows:
- **Name**: Step number (1-26)
- **Position**: World position
- **Rotation**: Quaternion
- **Scale**: Local scale
- **Info**: Human-readable summary

---

## Advanced Features

### Custom Snapshot Names

Instead of numbers, you can use descriptive names:

```
Next Snapshot Name: "Step_4_Button_Press"
```

Record → Snapshot named "Step_4_Button_Press" (doesn't auto-increment)

### Editing Snapshots

You can manually edit snapshot values in Inspector:

1. Expand `Recorded Snapshots`
2. Expand specific element (e.g., Element 17)
3. Manually adjust Position/Rotation/Scale values
4. Apply snapshot to see changes

### Clearing All Snapshots

If you want to start over:

1. **Check** `Clear All Snapshots` checkbox
2. All snapshots deleted
3. Next Snapshot Name resets to "1"

---

## Practical Example: 26-Step Workflow

### Before (Old Way):
```
Total time: ~30-45 minutes

For each step (1-26):
1. Enter Play mode
2. Navigate to step X
3. Position guidance
4. Right-click Transform → Copy Component
5. Exit Play mode
6. Right-click Transform → Paste Component Values
7. Repeat 26 times!
```

### After (With Recorder):
```
Total time: ~10-15 minutes

1. Enter Play mode ONCE
2. For each step (1-26):
   - Position guidance
   - Click Record Snapshot
3. Exit Play mode
4. Done! All 26 positions saved.

Later, apply any snapshot instantly in 1 click!
```

**Time saved: 15-30 minutes per recording session!**

---

## Multiple Guidance Objects

If you have multiple guidance objects (arrows, highlights, panels), add the component to each:

```
Arrow GameObject:
└─ TransformSnapshotRecorder (26 snapshots for arrow positions)

Highlight GameObject:
└─ TransformSnapshotRecorder (26 snapshots for highlight positions)

Instruction Panel:
└─ TransformSnapshotRecorder (26 snapshots for panel positions)
```

Each object has its own independent snapshot list.

---

## Tips & Tricks

### Tip 1: Naming Convention
Start snapshot names at the step number they correspond to:
- Step 4 → Name: "4"
- Step 17 → Name: "17"

This makes it easy to remember: `Apply Snapshot Index: 3` = Step 4 position (index starts at 0)

### Tip 2: Recording in Batches
You don't have to record all 26 steps in one session:
- Session 1: Record Steps 1-10
- Exit and test
- Session 2: Record Steps 11-20
- Session 3: Record Steps 21-26

All snapshots accumulate in the list!

### Tip 3: Quick Apply
If you need to quickly test a specific step's guidance position:
1. Set `Apply Snapshot Index: 16` (for Step 17)
2. Click `Apply Snapshot`
3. Enter Play mode to test
4. Position looks wrong? Exit, adjust, and re-record

### Tip 4: Backup Snapshots
Since snapshots are saved with the GameObject:
1. **Duplicate the GameObject** (Ctrl+D)
2. You now have a backup with all 26 snapshots
3. Experiment on the original, keep backup safe

---

## Troubleshooting

### Issue: "Snapshots disappear after exiting Play mode"

**Cause:** The GameObject might be a runtime-instantiated object.

**Solution:** Make sure the GameObject with `TransformSnapshotRecorder` exists in the scene hierarchy BEFORE entering Play mode.

### Issue: "I clicked Record Snapshot but nothing happened"

**Check:**
1. Are you in Play mode? (Recording only works in Play mode)
2. Check Console for green log message: `[TransformRecorder] Recorded snapshot...`
3. Expand `Recorded Snapshots` in Inspector - Size should increase

### Issue: "Apply Snapshot doesn't do anything"

**Check:**
1. `Apply Snapshot Index` is within valid range (0 to Size-1)
2. Snapshot list has recorded snapshots (Size > 0)
3. Console shows cyan log message when applying

### Issue: "Auto-increment stopped working"

**Cause:** Next Snapshot Name is not a number.

**Fix:** Change it back to a number (e.g., "17") and it will auto-increment again.

---

## Summary

✅ **One-time recording** in Play mode
✅ **Persistent snapshots** that survive exiting Play mode
✅ **Auto-incrementing names** (1 → 2 → 3 → ...)
✅ **Apply anytime** in Edit or Play mode
✅ **Human-readable info** for each snapshot
✅ **Saves massive time** on repetitive positioning work

Perfect for recording guidance positions for all 26 steps! 🎉
