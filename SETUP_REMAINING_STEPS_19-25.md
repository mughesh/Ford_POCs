# Setup Guide: Steps 19-25 (Final Steps)

## Task ID List

```
S2_19_RotateKnobCCW          - Rotate I-shaft lift knob CCW (disengage)
S2_20_ConfirmOperatorReady   - Confirmation: "Confirm operator completed their operation" + OK button
S2_21_PressRedButtonVLRG     - Press red button to disengage VLRG arm
S2_22_UnlockKnobCCW          - Rotate knob CCW to unlock traverse/rotation
S2_23_RetractToolBackward    - Grab + press Traverse + release (retract backward)
S2_24_SlideToolLeftPark      - Grab + press Slide + release (slide left to park)
S2_25_TeleportToControl2     - Teleport to control area (final)
S2_26_LockKnobCW3            - Rotate knob CW to lock (final lock)
```

---

## Step 19: Rotate Knob CCW (Disengage I-Shaft Lift)

**Task Type:** `KnobSwitchTask`

### Setup:
1. Add Task ID: `S2_19_RotateKnobCCW` to TaskDatabase
2. Create GameObject: `Task_19_RotateKnobCCW`
3. Add `KnobSwitchTask` component
4. Configure:

```
KnobSwitchTask:
├─ Task ID: S2_19_RotateKnobCCW
├─ Knob Transform: [Your I-shaft lift knob]
├─ Knob Grabbable: [Same knob]
├─ Target Angle: -90 (CCW rotation)
├─ Rotation Axis: (0, 1, 0)
├─ Rotation Speed: 2.0
├─ Play Animations After Rotation: ✓ (if lifting mechanism retracts)
├─ Animators To Trigger: [Lifting mechanism animator(s)]
├─ Animation Trigger Name: "Retract" (or your trigger name)
└─ Animation Duration: [Match your animation length]
```

**Add to TaskSequence:**
```
Element 18:
├─ TaskID: S2_19_RotateKnobCCW
└─ Task Reference: [Task_19_RotateKnobCCW]
```

---

## Step 19b: Confirmation Screen (NEW!)

**Task Type:** `ConfirmationButtonTask` (NEW script created)

### Setup:

**A. Create Confirmation Canvas:**
1. Create Canvas in World Space
2. Add background panel
3. Add Text: "Confirm whether the other operator completed their operation"
4. Add directional turn arrow image (left side of message)
5. Add OK button with:
   - `PhysicsGadgetButton` component
   - `ConfirmationButton` helper script
6. Position canvas where player should see it

**B. Create Task GameObject:**
1. Create GameObject: `Task_20_ConfirmOperatorReady`
2. Add `ConfirmationButtonTask` component
3. Configure:

```
ConfirmationButtonTask:
├─ Task ID: S2_20_ConfirmOperatorReady
├─ Confirmation Canvas: [Drag your confirmation canvas]
├─ Instruction Audio: [Your audio clip for "confirm operator ready"]
└─ Audio Source: [AudioSource component]
```

**C. Setup Button:**
1. On OK button GameObject, add `ConfirmationButton` script
2. Configure:
   - Confirmation Task: [Drag Task_20_ConfirmOperatorReady]
3. On `PhysicsGadgetButton` component:
   - OnPressed event → Add ConfirmationButton.OnButtonPressed()

**Add to TaskSequence:**
```
Element 19:
├─ TaskID: S2_20_ConfirmOperatorReady
└─ Task Reference: [Task_20_ConfirmOperatorReady]
```

---

## Step 20: Press Red Button (Disengage VLRG Arm)

**Task Type:** `ButtonPressAnimationTask` (NEW script created)

### Setup:

**A. Setup VLRG Arm Animation:**
1. Find your VLRG arm GameObject (the rod that moves)
2. Make sure it has `Animator` component with Animation Controller
3. In Animation Controller:
   - Add Trigger parameter (e.g., "Retract" or "Disengage")
   - Create transition from Any State → Retract animation
   - Set condition: Trigger name

**B. Create Task GameObject:**
1. Add Task ID: `S2_21_PressRedButtonVLRG` to TaskDatabase
2. Create GameObject: `Task_21_PressRedButtonVLRG`
3. Add `ButtonPressAnimationTask` component
4. Configure:

```
ButtonPressAnimationTask:
├─ Task ID: S2_21_PressRedButtonVLRG
│
├─ Button:
│  └─ Button: [Drag red button GameObject with PhysicsGadgetButton]
│
├─ Animations:
│  ├─ Animators To Trigger: [Size: 1]
│  │  └─ Element 0: [Drag VLRG arm GameObject with Animator]
│  ├─ Animation Trigger Name: "Retract" (or your trigger name)
│  └─ Animation Duration: 2.0 (match your animation length)
│
├─ Audio (Optional):
│  ├─ Button Press Sound: [Button click sound]
│  ├─ Animation Sound: [VLRG arm retract sound effect]
│  └─ Audio Source: [AudioSource component]
│
├─ Guidance (Optional):
│  └─ Hide On Press: [Any arrows/highlights]
│
└─ Debug - Test Animation:
   └─ Debug Trigger Animation: □ (check to test animation)
```

**C. Add to TaskSequence:**
```
Element 20:
├─ TaskID: S2_21_PressRedButtonVLRG
└─ Task Reference: [Task_21_PressRedButtonVLRG]
```

**Expected behavior:**
- Player presses red button
- VLRG arm rod retracts to original position (animation plays)
- Audio effects play
- Task completes after animation finishes

---

## Step 21: Unlock Knob CCW

**Task Type:** `KnobSwitchTask`

### Setup:
```
KnobSwitchTask:
├─ Task ID: S2_22_UnlockKnobCCW
├─ Knob Transform: [Lock knob]
├─ Target Angle: -90 (CCW rotation)
├─ Rotation Axis: (0, 1, 0)
└─ Play Animations: NO (just knob rotation, no extra animations)
```

---

## Step 22: Retract Tool Backward

**Task Type:** `ToolMovementTask` (existing script)

### Setup:
```
ToolMovementTask:
├─ Task ID: S2_23_RetractToolBackward
├─ Decking Tool Controller: [Your DeckingToolController]
├─ Target Position: [Position for retracted state]
├─ Handles: [Array of grabbable handles]
├─ Movement Button: [Traverse button]
└─ Hide On Button Press: [Arrows/guidance]
```

**Pattern:** Grab handle → Press Traverse (↕) button → Release → Tool moves backward

---

## Step 23: Slide Tool Left to Park

**Task Type:** `ToolMovementTask` (existing script)

### Setup:
```
ToolMovementTask:
├─ Task ID: S2_24_SlideToolLeftPark
├─ Decking Tool Controller: [Your DeckingToolController]
├─ Target Position: [Park position on left]
├─ Handles: [Array of grabbable handles]
├─ Movement Button: [Slide (↔) button]
└─ Hide On Button Press: [Arrows/guidance]
```

**Pattern:** Grab handle → Press Slide (↔) button → Release → Tool slides left to park

---

## Step 24: Teleport to Control (Final)

**Task Type:** `TeleportTask` (existing script)

### Setup:
```
TeleportTask:
├─ Task ID: S2_25_TeleportToControl2
├─ Teleport Target: [Control area position]
├─ Player Transform: [Auto Hand Player root]
├─ Reset Transforms To Zero: [TrackerOffsets, Auto Hand Player]
└─ Teleport Canvas UI: [Teleport canvas with button]
```

---

## Step 25: Lock Knob CW (Final Lock)

**Task Type:** `KnobSwitchTask`

### Setup:
```
KnobSwitchTask:
├─ Task ID: S2_26_LockKnobCW3
├─ Knob Transform: [Lock knob]
├─ Target Angle: 90 (CW rotation)
├─ Rotation Axis: (0, 1, 0)
└─ Play Animations: NO
```

**After this step completes:**
- Show "Module Complete - Success!" message (refer to Sim 1 completion screen)
- Display completion time

---

## Component Mapping

| Step | Component to Use | Notes |
|------|------------------|-------|
| 19 | `KnobSwitchTask` | With lifting mechanism animation |
| 19b | `ConfirmationButtonTask` | **NEW** - Canvas + OK button |
| 20 | `ButtonPressAnimationTask` | **NEW** - Button → Animation |
| 21 | `KnobSwitchTask` | Just rotation, no animation |
| 22 | `ToolMovementTask` | Traverse button |
| 23 | `ToolMovementTask` | Slide button |
| 24 | `TeleportTask` | Reuse existing |
| 25 | `KnobSwitchTask` | Final lock |

---

## Scripts Already Available

✅ **KnobSwitchTask** - Grab/release knob rotation with optional animations
✅ **ToolMovementTask** - Grab + button + release → tool moves
✅ **TeleportTask** - Teleport canvas + button
✅ **ConfirmationButtonTask** - NEW - Confirmation canvas + OK button
✅ **ButtonPressAnimationTask** - NEW - Button press → Animation → Task complete
✅ **DeckingToolController** - Smooth tool movement controller

---

## All Scripts Created - Ready to Go!

All task scripts are now available. You can implement all remaining steps (19-25) using the existing components!

---

## Final Module Complete Screen

After Step 25 (S2_26_LockKnobCW3) completes:
- Show completion UI (similar to Sim 1)
- Display total time elapsed
- Play success sound/animation

You can reuse completion screen logic from Sim 1 if available.
