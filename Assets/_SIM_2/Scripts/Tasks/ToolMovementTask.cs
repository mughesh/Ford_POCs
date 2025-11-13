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

    [Header("Guidance (Optional)")]
    [Tooltip("Arrows/highlights to hide when button is pressed (not when task completes)")]
    [SerializeField] private GameObject[] hideOnButtonPress;

    private bool handleGrabbed = false;
    private bool buttonPressed = false;
    private bool handleReleased = false;
    private bool taskCompleted = false;
    private bool isTaskActive = false; // NEW: Only respond when this task is active

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;

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

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= OnTaskActive;

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

    // Called when any task becomes active
    private void OnTaskActive(TaskID activeTaskID)
    {
        isTaskActive = (this.TaskID == activeTaskID);
    }

    private void OnHandleGrabbed(Hand hand, Grabbable grabbable)
    {
        if (!isTaskActive || taskCompleted) return; // NEW: Check if active

        handleGrabbed = true;
        handleReleased = false;

        Debug.Log("Handle grabbed!");
    }

    private void OnButtonPressed()
    {
        if (!isTaskActive || taskCompleted) return; // NEW: Check if active

        if (handleGrabbed && !handleReleased)
        {
            buttonPressed = true;
            Debug.Log("Button pressed while holding handle!");

            // Hide guidance immediately when button is pressed
            HideGuidance();
        }
        else
        {
            Debug.LogWarning("You need to grab the handle first!");
        }
    }

    private void HideGuidance()
    {
        foreach (var obj in hideOnButtonPress)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Hidden guidance: {obj.name}");
            }
        }
    }

    private void OnHandleReleased(Hand hand, Grabbable grabbable)
    {
        if (!isTaskActive || taskCompleted) return; // NEW: Check if active

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
