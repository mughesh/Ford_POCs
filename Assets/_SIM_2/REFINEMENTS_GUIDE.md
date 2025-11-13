# Refinements Guide: Highlight GameObject & Arrow Hiding

## FIX 1: Enable/Disable Highlight GameObject (2 minutes)

### Problem:
You have a separate highlight mesh that overlaps the original. You want it disabled by default and only enabled during the task.

### Solution:
Use **Simple GameObject Mode** in TaskHighlight.

### Setup:

1. Select your **separate highlight mesh GameObject**
2. **Disable it** (uncheck checkbox next to name in inspector)
3. Add Component → `TaskHighlight`
4. In Inspector:
   - **Task ID**: Select the task when this should appear (e.g., `S2_04_RotateToolCCW`)
   - **Simple GameObject Mode**: ✅ **CHECK THIS!**
   - **Hide On Tasks**: Leave empty (will auto-hide when task completes)

That's it! You can ignore all the material settings when in Simple GameObject Mode.

### How It Works:
```
Task S2_04_RotateToolCCW starts
    ↓
TaskHighlight.ShowHighlight()
    ↓
gameObject.SetActive(true) ← Your highlight mesh appears
    ↓
Task completes
    ↓
gameObject.SetActive(false) ← Hides again
```

### Why This Works:
- Simple GameObject Mode bypasses all material manipulation
- Just enables/disables the GameObject
- Perfect for separate meshes that are already set up with correct materials

---

## FIX 2: Hide Arrows When Button is Pressed (3 minutes)

### Problem:
Arrows pointing to buttons stay visible until tool finishes moving, which is misleading. Should hide immediately after button press.

### Solution:
Use the updated **ToolMovementTask** with `hideOnButtonPress` array.

### Setup:

1. Select your `Task_S2_04_RotateToolCCW` GameObject (with ToolMovementTask)
2. In Inspector, find **Guidance (Optional)** section
3. **Hide On Button Press → Size**: Set to number of objects to hide
4. Drag arrow GameObjects into array:
   - Arrow pointing to handle
   - Arrow pointing to button
   - Any other guidance for this task

### Example Configuration:
```
ToolMovementTask
  Task ID: S2_04_RotateToolCCW
  Decking Tool: [IP Decking Tool]
  Target Position: [ToolTarget_AfterRotate]
  Handles: [Handle Grabbable]
  Movement Button: [Rotate Button]

  Hide On Button Press (Size: 2)
    Element 0: [Arrow_PointToHandle]
    Element 1: [Arrow_PointToButton]
```

### How It Works:
```
User grabs handle
    ↓
User presses button
    ↓
ToolMovementTask.OnButtonPressed()
    ↓
Immediately hides all objects in hideOnButtonPress array
    ↓
User releases handle → Tool moves (arrows already hidden!)
```

---

## ALTERNATIVE: Standalone Script (For Non-Tool Tasks)

If you have arrows that need to hide on button press but **not** part of ToolMovementTask, use `HideOnButtonPress`:

### Setup:

1. Create Empty GameObject: `ArrowController_Step04`
2. Add Component → `HideOnButtonPress`
3. In Inspector:
   - **Objects To Hide**: Drag arrows/guidance GameObjects
   - **Button**: Drag the button (with PhysicsGadgetButton)

This script listens to the button directly and hides objects when pressed.

---

## Summary of Changes:

### TaskHighlight.cs - Added:
- ✅ **Simple GameObject Mode** checkbox
- When enabled: Just does `gameObject.SetActive(true/false)`
- When disabled: Uses original material overlay system
- Use for: Separate highlight meshes

### ToolMovementTask.cs - Added:
- ✅ **Hide On Button Press** array
- Hides guidance objects immediately when button is pressed
- Separate from task completion
- Use for: Arrows pointing to handles/buttons

### HideOnButtonPress.cs - New Script:
- Standalone component for hiding on button press
- Use when not using ToolMovementTask
- More flexible for other button interactions

---

## Testing Checklist:

### Test Highlight GameObject:
1. ✅ Highlight mesh is disabled in scene initially
2. ✅ Complete previous tasks until target task starts
3. ✅ Highlight mesh should appear
4. ✅ Task completes → Highlight mesh disappears

### Test Arrow Hiding:
1. ✅ Arrows visible when task starts
2. ✅ Grab handle → Arrows still visible
3. ✅ Press button → **Arrows immediately disappear**
4. ✅ Release handle → Tool moves (arrows still hidden)
5. ✅ Tool reaches target → Task completes

---

## Quick Inspector Reference:

**Highlight Mesh (Separate GameObject):**
```
TaskHighlight
  Task ID: S2_04_RotateToolCCW
  Simple GameObject Mode: ✓ CHECKED
  (All material settings ignored)
```

**ToolMovementTask with Arrow Hiding:**
```
ToolMovementTask
  ... (all existing settings)

  Hide On Button Press (Size: 2)
    [0] Arrow_Handle
    [1] Arrow_Button
```

---

Both fixes are now in place and ready to use!
