using UnityEngine;
using Autohand;

/// <summary>
/// Task for grab and release movement pattern
/// User grabs handle → releases → tool moves to target automatically
/// No button press required
/// </summary>
public class GrabReleaseMovementTask : Task
{
    [Header("Tool References")]
    [SerializeField] private DeckingToolController deckingTool;
    [SerializeField] private Transform targetPosition;

    [Header("Handles")]
    [SerializeField] private Grabbable[] handles; // Handles that can be grabbed

    [Header("Guidance (Optional)")]
    [Tooltip("Arrows/highlights to hide when handle is grabbed")]
    [SerializeField] private GameObject[] hideOnGrab;

    private bool handleGrabbed = false;
    private bool handleReleased = false;
    private bool taskCompleted = false;
    private bool isTaskActive = false; // NEW: Only respond when this task is active

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;

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

        // Hide guidance when grabbed
        HideGuidance();
    }

    private void OnHandleReleased(Hand hand, Grabbable grabbable)
    {
        if (!isTaskActive || taskCompleted) return; // NEW: Check if active

        if (handleGrabbed)
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
            Debug.LogError("GrabReleaseMovementTask: Missing DeckingToolController or target position!");
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

    private void HideGuidance()
    {
        foreach (var obj in hideOnGrab)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Hidden guidance: {obj.name}");
            }
        }
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
