using UnityEngine;

/// <summary>
/// Task for teleportation steps
/// Shows teleport canvas when task starts, completes when player teleports
/// </summary>
public class TeleportTask : Task
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private Transform playerTransform;

    [Header("UI - Drag the Canvas GameObject here")]
    [SerializeField] private GameObject teleportCanvasUI;

    private bool hasTeleported = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
    }

    private void Start()
    {
        // Hide canvas initially
        if (teleportCanvasUI != null)
        {
            teleportCanvasUI.SetActive(false);
        }
    }

    // Called by TaskEvents when this task starts
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (this.TaskID == obj)
        {
            ShowTeleportCanvas();
        }
    }

    private void ShowTeleportCanvas()
    {
        if (teleportCanvasUI != null)
        {
            teleportCanvasUI.SetActive(true);
            Debug.Log("Teleport canvas shown!");
        }
    }

    private void HideTeleportCanvas()
    {
        if (teleportCanvasUI != null)
        {
            teleportCanvasUI.SetActive(false);
        }
    }

    // Called by TeleportButton when pressed
    public void OnTeleportButtonPressed()
    {
        if (hasTeleported) return;

        if (teleportTarget == null || playerTransform == null)
        {
            Debug.LogWarning("TeleportTask: Missing teleport target or player transform!");
            return;
        }

        // Teleport player
        playerTransform.position = teleportTarget.position;
        playerTransform.rotation = teleportTarget.rotation;

        hasTeleported = true;

        Debug.Log($"Player teleported to: {teleportTarget.name}");

        // Hide canvas
        HideTeleportCanvas();

        // Complete task
        CompleteTask();
    }

    public override void TriggerComplete()
    {
        // For debug - force complete
        CompleteTask();
    }
}
