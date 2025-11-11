# Setup Guide: Knob Rotation & Tool Movement

## PART 1: Initialize Knob to Minimum Position (2 minutes)

### Step 1.1: Add Script to Knob
1. Select your **Lock Knob** GameObject (the one with HingeJoint)
2. Add Component → `HingeJointInitializer`
3. In Inspector:
   - **Initial Position**: Select `Min` (this will set it to minimum angle at start)
   - **Use Custom Angle**: Leave UNCHECKED

✅ **Test**: Press Play → Knob should start at its minimum position

---

## PART 2: Setup Knob Rotation Task (5 minutes)

### Step 2.1: Create Task in TaskDatabase & TaskSequence
1. Open `TaskDatabase` (in Resources)
2. Add: `S2_03_UnlockKnob`
3. Save

4. Open `Simulation2_TaskSequence`
5. Set **Tasks → Size** to `3` (adding Step 3)
6. Configure **Element 2**:
   - **Task ID**: `S2_03_UnlockKnob`
   - **Description**: `"Rotate the lock knob counterclockwise on the Instrument Panel Manipulator to unlock the IP decking tool."`
   - **Audio Clip**: (Optional)
   - **Is Completed**: UNCHECKED

### Step 2.2: Create KnobRotationTask GameObject
1. In Hierarchy, create Empty GameObject: `Task_S2_03_UnlockKnob`
2. Add Component → `KnobRotationTask`
3. In Inspector:
   - **Task ID**: `S2_03_UnlockKnob`
   - **Knob**: Drag your Lock Knob GameObject (with HingeJoint)
   - **Target Angle**: Set based on your hinge limits (e.g., `-90` if counterclockwise)
   - **Angle Tolerance**: `5` (degrees)
   - **Objects To Unlock**: Size `0` for now (we'll handle unlock in next step)

✅ **Test**:
- Complete Steps 1-2 (ticket + teleport)
- Rotate knob → Task should complete when it reaches target angle

---

## PART 3: Setup Decking Tool Controller (5 minutes)

### Step 3.1: Add DeckingToolController to Tool
1. Select your **IP Decking Tool** GameObject (the main tool object)
2. Add Component → `DeckingToolController`
3. In Inspector:
   - **Move Speed**: `0.5` (lower = slower, try 0.3-1.0 range)
   - **Movement Curve**: Leave as default (EaseInOut) or customize
   - **Tool Rigidbody**: Drag the tool's Rigidbody component

### Step 3.2: Create Target Position GameObject
1. In Hierarchy, create Empty GameObject: `ToolTarget_AfterRotate`
2. Move it to where you want the tool to end up after rotating
3. Rotate it to match desired orientation
4. This will be the target for Step 4

---

## PART 4: Setup Tool Movement Task (7 minutes)

### Step 4.1: Add Task to Database & Sequence
1. Open `TaskDatabase`
2. Add: `S2_04_RotateToolCCW`
3. Save

4. Open `Simulation2_TaskSequence`
5. Set **Tasks → Size** to `4` (adding Step 4)
6. Configure **Element 3**:
   - **Task ID**: `S2_04_RotateToolCCW`
   - **Description**: `"Hold the handle, press the Rotate (↺) button, then release the handle to rotate the IP Decking Tool counterclockwise."`
   - **Audio Clip**: (Optional)
   - **Is Completed**: UNCHECKED

### Step 4.2: Create ToolMovementTask GameObject
1. In Hierarchy, create Empty GameObject: `Task_S2_04_RotateToolCCW`
2. Add Component → `ToolMovementTask`
3. In Inspector:
   - **Task ID**: `S2_04_RotateToolCCW`
   - **Decking Tool**: Drag your IP Decking Tool GameObject
   - **Target Position**: Drag `ToolTarget_AfterRotate` GameObject
   - **Handles** → Size: `1` (or more if multiple handles)
     - Element 0: Drag the handle Grabbable component from tool
   - **Movement Button**: Drag the Rotate button GameObject (with PhysicsGadgetButton)

---

## PART 5: Setup Button

### Your Rotate Button needs:
1. **PhysicsGadgetButton** component (already have this)
2. **OnPressed Event** should NOT call anything for ToolMovementTask
   - The task listens to the button internally via AddListener
   - Just make sure the button is pressable and works

✅ **No UnityEvent setup needed** - ToolMovementTask subscribes to button automatically

---

## How It Works - Complete Flow

```
Step 3: Knob Rotation
  ↓
User rotates knob to target angle
  ↓
KnobRotationTask completes → Step 4 starts
  ↓
Step 4: Tool Movement
  ↓
1. User grabs handle (handleGrabbed = true)
  ↓
2. User presses Rotate button (buttonPressed = true)
  ↓
3. User releases handle
  ↓
ToolMovementTask detects: grabbed + button pressed + released
  ↓
DeckingToolController.MoveToTarget()
  ↓
Tool smoothly lerps to ToolTarget_AfterRotate position/rotation
  ↓
On arrival → Task completes → Step 5 starts
```

---

## Customizing Movement Speed

In `DeckingToolController`, you can adjust:

**Move Speed** (0.1 - 2.0):
- `0.3` = Very slow, smooth
- `0.5` = Medium (recommended)
- `1.0` = Fast
- `2.0` = Very fast

**Movement Curve** (AnimationCurve):
- Default: EaseInOut (smooth start and end)
- Linear: Constant speed
- EaseIn: Slow start, fast end
- EaseOut: Fast start, slow end
- Custom: Draw your own curve in inspector

---

## Inspector Checklist

**Lock Knob GameObject:**
```
HingeJoint (limits set)
HingeJointInitializer
  Initial Position: Min
```

**Task_S2_03_UnlockKnob:**
```
KnobRotationTask
  Task ID: S2_03_UnlockKnob
  Knob: [Lock Knob with HingeJoint]
  Target Angle: -90 (or whatever)
  Angle Tolerance: 5
```

**IP Decking Tool:**
```
Rigidbody (will be locked by DeckingToolController)
DeckingToolController
  Move Speed: 0.5
  Tool Rigidbody: [Self]
└─ Handles (children with Grabbable)
```

**Task_S2_04_RotateToolCCW:**
```
ToolMovementTask
  Task ID: S2_04_RotateToolCCW
  Decking Tool: [IP Decking Tool]
  Target Position: [ToolTarget_AfterRotate]
  Handles: [Tool handle Grabbables]
  Movement Button: [Rotate button]
```

---

## Troubleshooting

**Knob doesn't start at minimum:**
- Check HingeJoint limits are set
- Check HingeJointInitializer is on same GameObject as HingeJoint
- Check console for "HingeJoint initialized" message

**Knob task doesn't complete:**
- Check Target Angle matches your hinge limits
- Increase Angle Tolerance to 10-15 for testing
- Watch console for current angle logs

**Tool doesn't move:**
- Check "Button pressed while holding handle!" appears in console
- Check you're releasing AFTER pressing button (not before)
- Check DeckingToolController.IsMoving in inspector during play

**Tool moves instantly:**
- Increase Move Speed value (higher = slower paradoxically in this code)
- Wait, I made an error - let me check the code...

Actually, the moveSpeed is multiplied by Time.deltaTime, so:
- Lower values (0.1-0.5) = slower
- Higher values (1-2) = faster

**Tool movement is jerky:**
- Adjust Movement Curve in inspector
- Lower the Move Speed value
- Make sure tool doesn't have other forces acting on it

---

## Reusing for Future Steps

This ToolMovementTask is **reusable**! For other tool movements:

1. Create new target position GameObject
2. Create new Task GameObject with ToolMovementTask
3. Set different Task ID
4. Point to same DeckingToolController
5. Point to different target position
6. Point to correct button (Rotate/Traverse/Slide)

**Example for Step 5 (Traverse):**
- TaskID: `S2_05_TraverseToBeam`
- Target Position: `ToolTarget_AtBeam`
- Movement Button: `TraverseButton`
- Same tool, same handles!

---

Ready to test! Follow the setup steps and let me know if you hit any issues.
