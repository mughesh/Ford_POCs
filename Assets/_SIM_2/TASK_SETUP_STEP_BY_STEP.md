# Task System Setup - Step by Step Guide

## Part 1: Create TaskSequence ScriptableObject Asset

### Step 1.1: Create the ScriptableObject Asset
1. In Unity Project window, navigate to `Assets/_SIM_2/`
2. Right-click → Create → Tasks → Task Sequence
3. Name it: `Simulation2_TaskSequence`
4. Select the asset to edit in Inspector

### Step 1.2: Configure the TaskSequence
In the Inspector for `Simulation2_TaskSequence`:

1. **Sequence Number**: Set to `2` (for Simulation 2)
2. **Tasks** → Set **Size** to `1` (we're starting with just ticket removal)
3. Expand **Element 0**:
   - **Task ID** → **ID**: Type `"01_RemoveTicket"`
   - **Description**: Type `"Follow the directional arrow. Remove the Tele-auto ticket from the instrument panel, then discard it."`
   - **Sub Tasks** → Leave as Size: 0 (no subtasks for this step)
   - **Audio Clip**: Drag your instruction audio clip here (if you have one)
   - **Audio Delay**: Set to `0.5` (or whatever delay you want)
   - **Is Completed**: Leave UNCHECKED

✅ **Save** the asset (Ctrl+S)

---

## Part 2: Setup Scene GameObjects

### Step 2.1: Create Task GameObject
1. In Hierarchy, create Empty GameObject: `Task_01_RemoveTicket`
2. Add Component → `TicketRemovalTask` script
3. In Inspector, configure:
   - **Task ID** → **ID**: Type `"01_RemoveTicket"` (MUST match TaskSequence!)
   - **Ticket Grabbable**: Drag your ticket GameObject here
   - **Trash Icon**: Drag your trash icon GameObject here
   - **Tearing Sound**: Drag your tearing audio clip (optional)

### Step 2.2: Setup Trash Icon
1. Select your Trash Icon GameObject
2. Make sure it has:
   - ✅ Collider component (set as **Trigger**)
   - ✅ `TrashZone` component
3. In TrashZone Inspector:
   - **Target Tag**: `"Ticket"`
   - **Ticket Task**: Drag `Task_01_RemoveTicket` GameObject here

### Step 2.3: Setup Ticket GameObject
1. Select your Ticket GameObject
2. Make sure it has:
   - ✅ `Grabbable` component (Auto Hands)
   - ✅ Rigidbody
   - ✅ Tag set to `"Ticket"`

---

## Part 3: Setup TaskManager

### Step 3.1: Create or Find TaskManager
**Option A - If you don't have TaskManager in scene:**
1. In Hierarchy, create Empty GameObject: `TaskManager`
2. Add Component → `TaskManager` script

**Option B - If TaskManager already exists from Sim 1:**
1. Find existing TaskManager GameObject
2. We'll just update its references

### Step 3.2: Configure TaskManager Inspector
Select TaskManager GameObject, configure:

1. **Task Sequence Ref**: Drag `Simulation2_TaskSequence` asset here
2. **Task Description**: Drag the TaskDescription prefab (should be in Runtime/Prefabs)
3. **Task Spawn Point**: Create empty GameObject as child of your UI panel, drag it here
4. **Task Audio Source**: Add AudioSource component to TaskManager, drag it here
5. **Tick Audio Source**: Add another AudioSource to TaskManager, drag it here
6. **Timer**: Drag your Timer GameObject (if you have one, or create one)
7. **Last Played Audio**: Drag LastPlayedAudio GameObject (if exists)
8. **Scrollbar**: Drag your UI Scrollbar for task list
9. **Item Count**: Leave at 10 (auto-calculated)
10. **Step Duration**: 0.25
11. **Skip Timer**: Check if you want to skip timer for testing

---

## Part 4: Test Setup

### Checklist Before Testing:
- ✅ Simulation2_TaskSequence asset created with 1 task
- ✅ Task ID in TaskSequence = "01_RemoveTicket"
- ✅ Task_01_RemoveTicket GameObject in scene with TicketRemovalTask component
- ✅ Task ID in TicketRemovalTask = "01_RemoveTicket" (MUST MATCH!)
- ✅ Ticket has Grabbable + Rigidbody + Tag "Ticket"
- ✅ TrashZone has trigger collider + reference to TicketRemovalTask
- ✅ TaskManager configured with Simulation2_TaskSequence reference

### Testing:
1. Press Play
2. You should see task list UI with "Follow the directional arrow..."
3. Grab the ticket → Trash icon should appear
4. Move ticket to trash → Both disappear, task completes
5. Console should show: "Ticket discarded! Completing task..."

---

## Troubleshooting

### "Task not starting"
- Check TaskManager has correct TaskSequence reference
- Check Task ID matches exactly (case-sensitive!)

### "Trash doesn't appear when grabbing"
- Check TicketRemovalTask has Ticket Grabbable reference
- Check console for "Ticket grabbed!" message

### "Task doesn't complete"
- Check TrashZone has reference to TicketRemovalTask
- Check ticket has "Ticket" tag
- Check TrashZone collider is marked as Trigger

### "Hand gets stuck after discard"
- This should be fixed with ForceHandsRelease() in TrashZone
- If still happens, check order: ForceHandsRelease → disable component → disable GameObject

---

## Next Steps

Once Step 1 works:
1. We'll add more tasks to TaskSequence (Step 2: Teleport, etc.)
2. Create more Task scripts (TeleportTask, KnobRotationTask, etc.)
3. Build GuidanceManager for arrows, highlights, progress bars

---

## Quick Reference - Task IDs

Here's what we'll use for all 26 steps:
```
01_RemoveTicket
02_TeleportToIPTool
03_UnlockKnob
04_RotateToolCCW
05_TraverseToBeam
... (we'll add these as we go)
```

**IMPORTANT**: Task ID in TaskSequence MUST exactly match Task ID in Task component!
