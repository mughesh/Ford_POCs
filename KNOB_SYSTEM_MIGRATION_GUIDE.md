# Knob System Migration Guide

## Problem with Hinge-Based System
- Knob has only 30° motion range
- Hand tracking uses wrist bone rotation
- Grabbing a rotated knob causes immediate unwanted rotation due to wrist angle
- Makes interaction unpredictable and difficult

## Solution: Switch-Based System
- **Grab + Release = Snap to target angle** (like a switch)
- Smooth animated rotation (no physics tracking)
- More reliable for VR hand tracking

---

## Two Systems Available

### OLD SYSTEM (Hinge-Based) - **BACKUP ONLY**
**Files:**
- `KnobRotationTask.cs` → Original (can keep for Step 3 if working)
- `KnobRotationTask_HingeBased_BACKUP.cs` → Backup copy
- `HingeJointInitializer.cs` → Original (still useful if you have HingeJoints)
- `HingeJointInitializer_HingeBased_BACKUP.cs` → Backup copy

**How it works:**
- Continuously checks `hingeJoint.angle` in Update()
- Completes task when angle reaches target
- User physically rotates knob with hand

**When to use:**
- If Step 3 is already working with this system, keep it
- Only use for steps where physics rotation works well

---

### NEW SYSTEM (Switch-Based) - **USE FOR NEW STEPS**
**Files:**
- `KnobSwitchTask.cs` → NEW - Grab/release toggle
- `KnobSwitchInitializer.cs` → NEW - Set initial angle

**How it works:**
1. Player grabs knob
2. Player releases knob
3. Knob animates smoothly to target angle
4. Task completes when animation finishes

**When to use:**
- All future knob steps (Steps 12, 16, 19, 22, 25)
- If Step 3 is giving you trouble, you can update it to this system

---

## Setup: KnobSwitchTask

### 1. Scene Setup

**A. Knob GameObject:**
```
Your Knob GameObject:
├─ Transform (will be rotated)
├─ Grabbable (Auto Hands) ← Required!
├─ Rigidbody (Optional - will be made kinematic during animation)
├─ HingeJoint (Optional - can keep for physics feel, but not required)
└─ KnobSwitchInitializer (Optional - set starting angle)
```

**B. Initial Angle Setup:**
1. Add `KnobSwitchInitializer` to knob GameObject
2. Configure:
   - **Initial Angle**: Starting rotation (e.g., 0° for unlocked, 90° for locked)
   - **Rotation Axis**: Usually `(0, 1, 0)` for Y-axis rotation
   - **Store Original**: ✓ (checked)

### 2. Create Task GameObject

1. Create empty GameObject: `Task_##_YourKnobStep`
2. Add `KnobSwitchTask` component
3. Configure:

```
KnobSwitchTask Component:
├─ Task ID: S2_##_YourTaskID
│
├─ Knob Settings:
│  ├─ Knob Transform: [Drag knob GameObject]
│  └─ Knob Grabbable: [Drag knob GameObject - finds Grabbable component]
│
├─ Rotation Settings:
│  ├─ Target Angle: 90 (or whatever angle you want)
│  ├─ Rotation Axis: (0, 1, 0) [Y-axis for most knobs]
│  ├─ Rotation Speed: 2.0 (higher = faster)
│  └─ Rotation Curve: EaseInOut (smooth start/stop)
│
├─ Lock During Animation: ✓ (prevents grabbing during rotation)
│
├─ Optional Physics:
│  ├─ Hinge Joint: [Drag if knob has HingeJoint - will be locked during animation]
│  └─ Knob Rigidbody: [Drag if knob has Rigidbody - made kinematic during animation]
│
├─ What To Unlock (Optional):
│  └─ Objects To Unlock: [Objects to enable/unlock after rotation]
│
├─ Audio (Optional):
│  ├─ Rotation Sound: [Your rotation sound effect]
│  └─ Audio Source: [AudioSource component]
│
└─ Guidance (Optional):
   └─ Hide On Grab: [Arrows/highlights to hide when grabbed]
```

### 3. Add to TaskSequence

```
Element ##:
├─ TaskID: S2_##_YourTaskID
└─ Task Reference: [Drag Task_##_YourKnobStep GameObject]
```

---

## Example Configurations

### Unlock Knob (CCW rotation)
```
Target Angle: -90
Rotation Axis: (0, 1, 0)
Initial Angle (in KnobSwitchInitializer): 0
```

### Lock Knob (CW rotation)
```
Target Angle: 90
Rotation Axis: (0, 1, 0)
Initial Angle (in KnobSwitchInitializer): 0
```

### Toggle Knob (switches between two states)
For toggling, you'd need two separate tasks:
- Task A: Rotate from 0° to 90°
- Task B: Rotate from 90° to 0°

---

## Migration Steps for Existing Knob Tasks

### If Step 3 is Already Working:
- **Don't change it!** Keep using `KnobRotationTask` for Step 3
- Use `KnobSwitchTask` for all future knob steps

### If Step 3 Needs Updating:
1. In Step 3 Task GameObject, remove `KnobRotationTask` component
2. Add `KnobSwitchTask` component
3. Configure as shown above
4. On knob GameObject:
   - Keep HingeJoint if you want physics feel
   - Add `KnobSwitchInitializer` to set starting angle
   - Make sure `Grabbable` component is present

---

## Knob Steps in Your Simulation

Based on updated task list:

| Step | Task ID | Description | System to Use |
|------|---------|-------------|---------------|
| 3 | S2_03_UnlockKnob | Rotate knob CCW to unlock | Old (if working) OR New |
| 12 | S2_12_RotateToolCW | Rotate knob CW | **NEW** |
| 16 | S2_16_RotateKnobCW | Rotate knob CW | **NEW** |
| 19 | S2_19_LockKnobCW | Rotate knob CW to lock | **NEW** |
| 22 | S2_22_LockKnobCW2 | Rotate knob CW to lock | **NEW** |
| 25 | S2_25_RotateKnobCCW | Rotate knob CCW | **NEW** |

---

## Advantages of New System

✅ **No wrist rotation conflicts** - User just grabs and releases
✅ **Consistent behavior** - Always rotates to target angle
✅ **Smooth animation** - Looks polished with AnimationCurve
✅ **Simpler interaction** - No need to track angle during grab
✅ **VR-friendly** - Works with any hand tracking system

---

## Backup Files Location

All hinge-based backup files are in the same folders with `_HingeBased_BACKUP` suffix:
- `Assets\_SIM_2\Scripts\Tasks\KnobRotationTask_HingeBased_BACKUP.cs`
- `Assets\_SIM_2\Scripts\Helpers\HingeJointInitializer_HingeBased_BACKUP.cs`

You can restore these anytime if needed!
