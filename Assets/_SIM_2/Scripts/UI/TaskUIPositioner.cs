using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Positions UI canvas at different locations based on active task
/// Useful for moving instruction panel to appropriate positions
/// </summary>
public class TaskUIPositioner : MonoBehaviour
{
    [System.Serializable]
    public class UIPositionRule
    {
        [Tooltip("When this task becomes active")]
        public TaskID taskID;

        [Tooltip("Move UI to this position (uses position & rotation)")]
        public Transform targetPosition;

        [Tooltip("Optional: Copy scale from target as well")]
        public bool copyScale = false;
    }

    [Header("UI Settings")]
    [SerializeField] private RectTransform uiCanvasTransform;

    [Header("Positioning Rules")]
    [Tooltip("List of rules: TaskID → Target Position. UI moves when task becomes active.")]
    [SerializeField] private List<UIPositionRule> positionRules = new List<UIPositionRule>();

    [Header("Options")]
    [SerializeField] private bool moveOnlyOnce = false; // Only move once per task, ignore if already at position
    [SerializeField] private bool resetToOriginalOnDisable = true;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private TaskID lastAppliedTaskID;

    private void OnEnable()
    {
        TaskEvents.OnTaskActive += OnTaskActive;
    }

    private void OnDisable()
    {
        TaskEvents.OnTaskActive -= OnTaskActive;

        if (resetToOriginalOnDisable)
        {
            ResetToOriginal();
        }
    }

    private void Start()
    {
        if (uiCanvasTransform == null)
        {
            uiCanvasTransform = GetComponent<RectTransform>();
        }

        // Store original position
        if (uiCanvasTransform != null)
        {
            originalPosition = uiCanvasTransform.position;
            originalRotation = uiCanvasTransform.rotation;
            originalScale = uiCanvasTransform.localScale;
        }
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        // Check if we should skip (already moved for this task)
        if (moveOnlyOnce && lastAppliedTaskID == activeTaskID)
        {
            return;
        }

        // Find matching rule
        foreach (var rule in positionRules)
        {
            if (rule.taskID == activeTaskID && rule.targetPosition != null)
            {
                MoveUIToPosition(rule);
                lastAppliedTaskID = activeTaskID;
                Debug.Log($"UI moved for task {activeTaskID.ID} to {rule.targetPosition.name}");
                return; // Only apply first matching rule
            }
        }
    }

    private void MoveUIToPosition(UIPositionRule rule)
    {
        if (uiCanvasTransform == null || rule.targetPosition == null)
            return;

        // Instantly move to target position
        uiCanvasTransform.position = rule.targetPosition.position;
        uiCanvasTransform.rotation = rule.targetPosition.rotation;

        if (rule.copyScale)
        {
            uiCanvasTransform.localScale = rule.targetPosition.localScale;
        }
    }

    private void ResetToOriginal()
    {
        if (uiCanvasTransform != null)
        {
            uiCanvasTransform.position = originalPosition;
            uiCanvasTransform.rotation = originalRotation;
            uiCanvasTransform.localScale = originalScale;
        }
    }

    // Public method to manually trigger repositioning for a specific task
    public void MoveToTaskPosition(TaskID taskID)
    {
        OnTaskActive(taskID);
    }

    // Public method to reset to original position
    public void ResetPosition()
    {
        ResetToOriginal();
    }

    // Helper to add rules at runtime if needed
    public void AddPositionRule(TaskID taskID, Transform targetPosition, bool copyScale = false)
    {
        positionRules.Add(new UIPositionRule
        {
            taskID = taskID,
            targetPosition = targetPosition,
            copyScale = copyScale
        });
    }
}
