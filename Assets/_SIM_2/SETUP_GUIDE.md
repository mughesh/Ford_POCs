# Ford Simulation 2 - Setup Guide

## Architecture Overview

The simulation uses a simple step-based state machine:

- **SimulationController**: Main controller that manages step progression
- **StepBase**: Abstract base class for all steps (provides guidance, instruction text)
- **Step Scripts**: Individual step implementations (e.g., TicketRemovalStep, KnobRotationStep)

## Step 1: Ticket Removal Setup

### Scene Hierarchy Setup

1. **Create SimulationController**
   - Create empty GameObject: "SimulationController"
   - Add `SimulationController.cs` component

2. **Setup Ticket**
   - Your ticket GameObject should have:
     - `Grabbable` component (Auto Hands)
     - Tag: "Ticket"
     - Rigidbody (required by Auto Hands)

3. **Setup Trash Zone**
   - Create GameObject: "TrashIcon" (or use your existing trash icon)
   - Add `TrashZone.cs` component
   - Add `Collider` component (Box/Sphere) and set as **Trigger**
   - This will be disabled initially and enabled when ticket is grabbed

4. **Create Step GameObject**
   - Create empty GameObject: "Step_01_TicketRemoval"
   - Add `TicketRemovalStep.cs` component
   - Configure in inspector:
     - **Instruction Text**: "Follow the directional arrow. Remove the Tele-auto ticket from the instrument panel, then discard it."
     - **Ticket Grabbable**: Drag your ticket GameObject
     - **Trash Icon**: Drag your trash icon GameObject
     - **Tearing Sound**: Drag your tearing audio clip
     - **Guidance Arrows**: Create arrow GameObjects and add them to array

5. **Create Guidance Arrow**
   - Create GameObject with arrow 3D model/sprite
   - Position it pointing at the ticket
   - Add `GuidanceArrow.cs` component (optional - for animation)
   - Drag this into the TicketRemovalStep's "Guidance Arrows" array

6. **Wire Up SimulationController**
   - Select SimulationController GameObject
   - In the "Steps" array, set size to 1
   - Drag "Step_01_TicketRemoval" into array slot 0
   - Setup UI references (instruction panel, text)
   - Add audio source and step complete sound

### Inspector Configuration Checklist

**TicketRemovalStep:**
- ✓ Instruction Text (filled)
- ✓ Ticket Grabbable (assigned)
- ✓ Trash Icon (assigned)
- ✓ Tearing Sound (optional audio clip)
- ✓ Guidance Arrows array (arrow objects pointing to ticket)

**TrashZone:**
- ✓ Target Tag = "Ticket"
- ✓ Collider component is Trigger

**SimulationController:**
- ✓ Steps array size = 1
- ✓ Step_01_TicketRemoval in array
- ✓ Instruction Panel UI reference
- ✓ Instruction Text (TextMeshPro) reference
- ✓ Audio Source
- ✓ Step Complete Sound

## How It Works

1. Simulation starts → Step 1 becomes active
2. Guidance arrows appear, instruction text shows
3. User grabs ticket → Tearing sound plays, trash icon appears
4. User brings ticket to trash zone trigger
5. OnTriggerEnter fires → Ticket parents to trash, both disable
6. Step completes → Simulation moves to Step 2 (when you add it)

## Adding More Steps

1. Create new script inheriting from `StepBase`
2. Override `OnStepEnter()` and `OnStepExit()`
3. Call `CompleteStep()` when step is done
4. Add to SimulationController's steps array in order

## Next Steps

For Step 2 (Teleport), you'll need to:
- Use `TeleportButton.cs` script I created
- Create UI button in world space or screen space
- Show it when Step 1 completes
- Configure teleport target position

Let me know when you're ready to test or if you need any adjustments!
