using UnityEngine;

/// <summary>
/// Simple task to detach an object from its parent
/// Used at the end to release the steering deck from the decking tool
/// </summary>
public class DetachObjectTask : Task
{
    [Header("Detachment")]
    [Tooltip("The object to detach from its parent")]
    [SerializeField] private Transform objectToDetach;
    [Tooltip("Optional: New parent to assign (leave empty to make it root object)")]
    [SerializeField] private Transform newParent;
    [Tooltip("Should we keep world position/rotation when detaching?")]
    [SerializeField] private bool keepWorldTransform = true;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip detachSound;
    [SerializeField] private AudioSource audioSource;

    private bool taskCompleted = false;
    private bool isTaskActive = false;

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

    private void OnTaskActive(TaskID activeTaskID)
    {
        isTaskActive = (this.TaskID == activeTaskID);

        if (isTaskActive)
        {
            DetachObject();
        }
    }

    private void DetachObject()
    {
        if (taskCompleted) return;

        if (objectToDetach == null)
        {
            Debug.LogError("DetachObjectTask: Missing object to detach!");
            taskCompleted = true;
            CompleteTask();
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

        taskCompleted = true;
        CompleteTask();
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
