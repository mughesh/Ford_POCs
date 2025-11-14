using UnityEngine;

/// <summary>
/// Simple script to enable/disable a GameObject when a specific task becomes active
/// </summary>
public class EnableOnTask : MonoBehaviour
{
    [Header("Task Settings")]
    [Tooltip("Enable this GameObject when this task becomes active")]
    [SerializeField] private TaskID taskID;

    [Header("Target")]
    [Tooltip("GameObject to enable/disable (leave empty to use this GameObject)")]
    [SerializeField] private GameObject targetObject;
    [SerializeField] private GameObject screen1;

    private void OnEnable()
    {
        TaskEvents.OnTaskActive += OnTaskActive;
        TaskEvents.OnTaskCompleted += OnTaskCompleted;
    }

    private void OnDisable()
    {
        TaskEvents.OnTaskActive -= OnTaskActive;
        TaskEvents.OnTaskCompleted -= OnTaskCompleted;
    }

    private void Start()
    {
        // If no target specified, use this GameObject
        if (targetObject == null)
        {
            targetObject = gameObject;
        }

        // Start disabled
        targetObject.SetActive(false);
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        if (activeTaskID == taskID)
        {
            targetObject.SetActive(true);
            //screen1.SetActive(false);
            Debug.Log($"EnableOnTask: Enabled {targetObject.name} for task {taskID.ID}");
        }
    }

    private void OnTaskCompleted(TaskID completedTaskID)
    {
        if (completedTaskID == taskID)
        {
            targetObject.SetActive(false);
            Debug.Log($"EnableOnTask: Disabled {targetObject.name} after task {taskID.ID} completed");
        }
    }
}
