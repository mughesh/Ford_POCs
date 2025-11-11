using UnityEngine;
using Autohand;

/// <summary>
/// Reusable task for decking tool movements
/// Pattern: Grab handle → Press button → Release handle → Tool moves to target
/// </summary>
public class ToolMovementTask : Task
{
    [Header("Tool References")]
    [SerializeField] private DeckingToolController deckingTool;
    [SerializeField] private Transform targetPosition;

    [Header("Handle & Button")]
    [SerializeField] private Grabbable[] handles; // Handles that can be grabbed
    [SerializeField] private PhysicsGadgetButton movementButton; // The button to press

    private bool handleGrabbed = false;
    private bool buttonPressed = false;
    private bool handleReleased = false;
    private bool taskCompleted = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to button press
        if (movementButton != null)
        {
            movementButton.OnPressed.AddListener(OnButtonPressed);
        }

        // Subscribe to handle grab/release
        foreach (var handle in handles)
        {
            if (handle != null)
            {
                handle.OnGrabEvent += OnHandleGrabbed;
                handle.OnReleaseEvent += OnHandleReleased;
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe
        if (movementButton != null)
        {
            movementButton.OnPressed.RemoveListener(OnButtonPressed);
        }

        foreach (var handle in handles)
        {
            if (handle != null)
            {
                handle.OnGrabEvent -= OnHandleGrabbed;
                handle.OnReleaseEvent -= OnHandleReleased;
            }
        }
    }

    private void OnHandleGrabbed(Hand hand, Grabbable grabbable)
    {
        if (taskCompleted) return;

        handleGrabbed = true;
        handleReleased = false;

        Debug.Log("Handle grabbed!");
    }

    private void OnButtonPressed()
    {
        if (taskCompleted) return;

        if (handleGrabbed && !handleReleased)
        {
            buttonPressed = true;
            Debug.Log("Button pressed while holding handle!");
        }
        else
        {
            Debug.LogWarning("You need to grab the handle first!");
        }
    }

    private void OnHandleReleased(Hand hand, Grabbable grabbable)
    {
        if (taskCompleted) return;

        // Check if button was pressed while holding
        if (buttonPressed && handleGrabbed)
        {
            handleReleased = true;
            Debug.Log("Handle released - initiating tool movement!");

            // Start tool movement
            MoveTool();
        }

        handleGrabbed = false;
    }

    private void MoveTool()
    {
        if (deckingTool == null || targetPosition == null)
        {
            Debug.LogError("ToolMovementTask: Missing DeckingToolController or target position!");
            return;
        }

        // Move tool to target
        deckingTool.MoveToTarget(targetPosition, OnToolReachedTarget);
    }

    private void OnToolReachedTarget()
    {
        Debug.Log("Tool reached target position!");
        taskCompleted = true;
        CompleteTask();
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
