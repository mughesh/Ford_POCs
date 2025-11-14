# Quick Reference: Steps 19-25

## Task IDs & Components

| Step | Task ID | Component | Key Settings |
|------|---------|-----------|--------------|
| **19** | `S2_19_RotateKnobCCW` | `KnobSwitchTask` | Target Angle: -90, Play Animations: ✓ (lifting mechanism) |
| **19b** | `S2_20_ConfirmOperatorReady` | `ConfirmationButtonTask` | Canvas + OK button + audio instruction |
| **20** | `S2_21_PressRedButtonVLRG` | `ButtonPressAnimationTask` | Red button → VLRG arm retract animation |
| **21** | `S2_22_UnlockKnobCCW` | `KnobSwitchTask` | Target Angle: -90, No animations |
| **22** | `S2_23_RetractToolBackward` | `ToolMovementTask` | Traverse button, retract backward |
| **23** | `S2_24_SlideToolLeftPark` | `ToolMovementTask` | Slide button, move left to park |
| **24** | `S2_25_TeleportToControl2` | `TeleportTask` | Final teleport to control area |
| **25** | `S2_26_LockKnobCW3` | `KnobSwitchTask` | Target Angle: 90, Final lock |

---

## New Scripts Created

### 1. `ButtonPressAnimationTask.cs`
**Purpose:** Press button → Trigger animation → Complete task

**Key Features:**
- Direct button press detection (no grab required)
- Supports multiple animators (if needed)
- Two audio clips: button press + animation sound
- Debug checkbox to test animations
- Auto-completes after animation duration

**Inspector Fields:**
```
├─ Button: PhysicsGadgetButton reference
├─ Animators To Trigger: Array of Animator components
├─ Animation Trigger Name: String (e.g., "Retract")
├─ Animation Duration: Float (match animation length)
├─ Button Press Sound: AudioClip
├─ Animation Sound: AudioClip
├─ Audio Source: AudioSource component
├─ Hide On Press: GameObject array (guidance)
└─ Debug Trigger Animation: Bool checkbox
```

---

### 2. `ConfirmationButtonTask.cs`
**Purpose:** Show confirmation screen → Wait for OK button → Complete task

**Key Features:**
- Shows canvas when task becomes active
- Waits for button press
- Audio instruction support
- Simple and reusable

**Inspector Fields:**
```
├─ Confirmation Canvas: GameObject (contains message + button)
├─ Instruction Audio: AudioClip
└─ Audio Source: AudioSource component
```

**Helper Script:** `ConfirmationButton.cs` - Attach to OK button

---

## Common Patterns

### Knob Rotation (Steps 19, 21, 25)
```
KnobSwitchTask:
├─ Knob Transform + Grabbable
├─ Target Angle: ±90
├─ Rotation Axis: (0, 1, 0)
└─ Optional: Animations after rotation
```

### Tool Movement (Steps 22, 23)
```
ToolMovementTask:
├─ DeckingToolController + Target Position
├─ Handles array (grabbable)
├─ Movement Button (PhysicsGadgetButton)
└─ Pattern: Grab → Press button → Release → Move
```

### Button Press Animation (Step 20)
```
ButtonPressAnimationTask:
├─ Button (PhysicsGadgetButton)
├─ Animators + Trigger name
├─ Animation Duration
└─ Pattern: Press button → Animation plays → Complete
```

### Confirmation Screen (Step 19b)
```
ConfirmationButtonTask:
├─ Confirmation Canvas (with OK button)
├─ Audio instruction
└─ Pattern: Canvas shows → Press OK → Complete
```

---

## Testing Tips

1. **Debug Checkboxes:**
   - `KnobSwitchTask` has "Debug Trigger Animations"
   - `ButtonPressAnimationTask` has "Debug Trigger Animation"
   - Check these in Play mode to test animations without doing the full task

2. **Animation Duration:**
   - Always match animation duration to actual clip length
   - Too short = task completes before animation finishes
   - Too long = unnecessary waiting

3. **Audio Clips:**
   - Button press sounds = instant feedback
   - Animation sounds = mechanical effects during movement
   - Instruction audio = guidance for player

4. **Task Order:**
   - Make sure TaskSequence order matches step numbers
   - Step 19 → Step 19b → Step 20 → etc.

---

## Final Step (After 25)

After `S2_26_LockKnobCW3` completes:
- Show "Module Complete - Success!" screen
- Display total time elapsed
- Play completion sound/animation
- (Refer to Sim 1 completion logic)

---

## All 26 Steps Complete! 🎉

You now have all the tools needed to finish the simulation!
