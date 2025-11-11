# Setup Guide: Task Highlighting & UI Positioning

## PART 1: Mesh Highlighting (3 minutes)

### What It Does:
Shows a highlight overlay on a mesh when a specific task becomes active.

### Setup Steps:

#### Step 1.1: Add Highlight to Handle Mesh
1. Select the **mesh GameObject you want to highlight** (e.g., decking tool handle)
2. Add Component → `TaskHighlight`
3. In Inspector:
   - **Task ID**: Select `S2_03_UnlockKnob` (or whichever task should trigger highlight)
   - **Highlight Material**: Drag your highlight material here
   - **Target Renderer**: Should auto-fill (or drag the MeshRenderer)
   - **Material Index**: `0` (usually)
   - **Add As Overlay**: ✅ CHECKED (recommended - creates duplicate mesh scaled up)
   - **Highlight Scale**: `1.02` (makes highlight slightly bigger than mesh)

#### Optional Settings:
- **Hide On Tasks**: Add task IDs where highlight should hide (e.g., when task completes)
- **Add As Overlay = false**: Will replace material instead of creating overlay

### How It Works:
```
Task S2_03_UnlockKnob starts
    ↓
TaskEvents.OnTaskActive fires
    ↓
TaskHighlight hears event
    ↓
Creates duplicate mesh with highlight material (scaled 1.02x)
    ↓
Task completes
    ↓
Highlight destroyed
```

### Multiple Highlights:
You can add TaskHighlight to **multiple meshes** with the same Task ID - they'll all highlight simultaneously!

Example:
- Handle mesh → TaskHighlight (Task: S2_04_RotateToolCCW)
- Button mesh → TaskHighlight (Task: S2_04_RotateToolCCW)
Both highlight when Step 4 starts!

---

## PART 2: UI Canvas Positioning (5 minutes)

### What It Does:
Moves your task list UI canvas to different positions when specific tasks start.

### Setup Steps:

#### Step 2.1: Create Position Target GameObjects
1. In Hierarchy, create Empty GameObject: `UIPosition_Step03`
2. Move it to where you want UI during Step 3
3. Rotate it to face correct direction
4. Repeat for other steps that need UI repositioning:
   - `UIPosition_Step05`
   - `UIPosition_Step10`
   - etc.

#### Step 2.2: Add Script to UI Canvas
1. Select your **Task List UI Canvas** GameObject
2. Add Component → `TaskUIPositioner`
3. In Inspector:
   - **UI Canvas Transform**: Should auto-fill (or drag RectTransform)
   - **Move Only Once**: ✅ CHECKED (prevents re-positioning if task repeats)
   - **Reset To Original On Disable**: ✅ CHECKED (returns to start position when disabled)

#### Step 2.3: Configure Position Rules
In TaskUIPositioner component:

1. **Position Rules → Size**: Set to number of position changes you need
2. For each element:
   - **Task ID**: Select task from dropdown (e.g., `S2_03_UnlockKnob`)
   - **Target Position**: Drag position GameObject (e.g., `UIPosition_Step03`)
   - **Copy Scale**: Usually UNCHECKED (keeps UI scale)

**Example Configuration:**
```
Position Rules (Size: 3)
  Element 0:
    Task ID: S2_03_UnlockKnob
    Target Position: UIPosition_Step03
    Copy Scale: false

  Element 1:
    Task ID: S2_05_TraverseToBeam
    Target Position: UIPosition_Step05
    Copy Scale: false

  Element 2:
    Task ID: S2_10_TeleportToControl
    Target Position: UIPosition_Step10
    Copy Scale: false
```

### How It Works:
```
Step 3 starts (S2_03_UnlockKnob)
    ↓
TaskEvents.OnTaskActive("S2_03_UnlockKnob")
    ↓
TaskUIPositioner checks rules
    ↓
Finds Element 0 matches
    ↓
INSTANTLY moves UI to UIPosition_Step03 location/rotation
    ↓
UI stays there until next rule triggers
```

### Important Notes:

✅ **Instant Movement**: No lerping/animation - jumps instantly to avoid clipping through objects

✅ **Stays Until Next Rule**: UI stays at last position until another rule matches

✅ **Not All Tasks Need Rules**: Only add rules for steps where UI needs to move. UI keeps its position for tasks without rules.

✅ **Order Doesn't Matter**: Rules are checked by Task ID, not array order

---

## Example Setup for Your Current Progress:

### Highlight Handle for Step 4:
```
Handle Mesh GameObject:
  └─ TaskHighlight
       Task ID: S2_04_RotateToolCCW
       Highlight Material: [Your highlight material]
       Add As Overlay: ✓
       Highlight Scale: 1.02
```

### Move UI for Steps 3, 5, 10:
```
Task List UI Canvas:
  └─ TaskUIPositioner
       Position Rules (Size: 3)
         [0] S2_03_UnlockKnob → UIPosition_Step03
         [1] S2_05_TraverseToBeam → UIPosition_Step05
         [2] S2_10_TeleportToControl → UIPosition_Step10
```

---

## Testing:

### Test Highlighting:
1. Play simulation
2. Complete steps until target task starts
3. Mesh should have glowing outline
4. Task completes → highlight disappears

### Test UI Positioning:
1. Note original UI position
2. Play simulation
3. Complete tasks up to first position rule
4. UI should instantly jump to new position
5. Continue - UI jumps again at next rule

---

## Tips:

**Highlight Material**:
- Use an emissive material for glow effect
- Or use bright colored material with transparency
- Add to Assets/Materials folder

**UI Position Targets**:
- Place them in scene where comfortable to view
- Face towards player
- Keep distance appropriate for VR (1-3 meters)

**Performance**:
- Overlay highlights create extra mesh - use sparingly
- For many highlights, consider using material replacement instead (uncheck Add As Overlay)

---

## Troubleshooting:

**Highlight doesn't appear:**
- Check Task ID matches exactly
- Check highlight material is assigned
- Check Target Renderer is assigned
- Look for "Highlight shown on..." in console

**UI doesn't move:**
- Check UI Canvas Transform is assigned
- Check Position Rules have correct Task IDs
- Check target position GameObjects exist
- Look for "UI moved for task..." in console

**Highlight stays after task completes:**
- Highlight auto-hides when task completes
- If persisting, add task ID to "Hide On Tasks" array

**UI moves through objects:**
- This is expected (instant teleport)
- Position targets carefully to avoid awkward transitions
- Consider fewer position changes

---

You're all set! Add highlights and position your UI as needed. Both systems work automatically via TaskEvents.
