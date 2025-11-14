using UnityEngine;
using Autohand;

/// <summary>
/// Simple task that shows a confirmation canvas with a button
/// Completes when player presses the button
/// Used for operator confirmation, ready checks, etc.
/// </summary>
public class ConfirmationButtonTask : Task
{
    [Header("UI Canvas")]
    [SerializeField] private GameObject confirmationCanvas;

    [Header("Object Detachment (Optional)")]
    [Tooltip("Object to detach from parent after confirmation (e.g., steering deck)")]
    [SerializeField] private Transform objectToDetach;
    [Tooltip("Optional: New parent to assign (leave empty to make it root object)")]
    [SerializeField] private Transform newParent;
    [Tooltip("Should we keep world position/rotation when detaching?")]
    [SerializeField] private bool keepWorldTransform = true;

    [Header("Audio (Optional)")]
    [Tooltip("Audio instruction to play when canvas appears")]
    [SerializeField] private AudioClip instructionAudio;
    [SerializeField] private AudioClip detachSound;
    [SerializeField] private AudioSource audioSource;

    private bool buttonPressed = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= OnTaskActive;
    }

    private void Start()
    {
        // Hide canvas initially
        if (confirmationCanvas != null)
        {
            confirmationCanvas.SetActive(false);
        }
    }

    // Called by TaskEvents when this task starts
    private void OnTaskActive(TaskID activeTaskID)
    {
        if (this.TaskID == activeTaskID)
        {
            ShowConfirmationCanvas();
        }
    }

    private void ShowConfirmationCanvas()
    {
        if (confirmationCanvas != null)
        {
            confirmationCanvas.SetActive(true);
            OnTaskStarted?.Invoke();

            // Play audio instruction
            if (audioSource != null && instructionAudio != null)
            {
                audioSource.PlayOneShot(instructionAudio);
            }

            Debug.Log("Confirmation canvas shown - waiting for button press");
        }
    }

    private void HideConfirmationCanvas()
    {
        if (confirmationCanvas != null)
        {
            confirmationCanvas.SetActive(false);
        }
    }

    // Called by ConfirmationButton helper when button is pressed
    public void OnConfirmationButtonPressed()
    {
        if (buttonPressed)
        {
            Debug.LogWarning("ConfirmationButtonTask: Button already pressed, ignoring duplicate press");
            return;
        }

        buttonPressed = true;

        Debug.Log("Confirmation button pressed! Hiding canvas and completing task...");

        // Hide canvas
        HideConfirmationCanvas();

        Debug.Log($"Canvas hidden. Now calling CompleteTask() for TaskID: {TaskID}");

        // Detach object if specified
        DetachObject();

        // Complete task
        CompleteTask();

        Debug.Log($"CompleteTask() called successfully! TaskEvents.TaskCompleted({TaskID}) should have fired.");
        Debug.Log("TaskManager should now activate the next task in the sequence.");
    }

    private void DetachObject()
    {
        if (objectToDetach == null)
        {
            Debug.Log("ConfirmationButtonTask: No object to detach (objectToDetach is null)");
            return;
        }

        // Play detach sound
        if (audioSource != null && detachSound != null)
        {
            audioSource.PlayOneShot(detachSound);
        }

        // Detach from parent
        objectToDetach.SetParent(newParent, keepWorldTransform);

        Debug.Log($"Detached '{objectToDetach.name}' from parent. New parent: {(newParent != null ? newParent.name : "None (root)")}");
    }

    public override void TriggerComplete()
    {
        // For debug - force complete
        CompleteTask();
    }
}
