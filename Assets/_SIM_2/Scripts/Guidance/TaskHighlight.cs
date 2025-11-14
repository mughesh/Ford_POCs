using UnityEngine;
using Autohand;

/// <summary>
/// Highlights a mesh during a specific task by applying highlight material
/// Similar to arrow guidance but for mesh highlighting
/// Enhanced: Supports multiple tasks, offset controls, and hide-on-grab option
/// </summary>
public class TaskHighlight : MonoBehaviour
{
    [Header("Task Settings")]
    [Tooltip("DEPRECATED: Use 'Show On Tasks' array instead. Kept for backwards compatibility.")]
    [SerializeField] private TaskID taskID;
    [Tooltip("Show highlight when any of these tasks are active (NEW - use this instead of single taskID)")]
    [SerializeField] private TaskID[] showOnTasks;
    [Tooltip("Leave empty to show during this task. Add task IDs to hide during specific tasks.")]
    [SerializeField] private TaskID[] hideOnTasks;

    [Header("Hide Behavior")]
    [Tooltip("Hide highlight immediately when grabbed (for grab-only steps). If unchecked, hides on task completion.")]
    [SerializeField] private bool hideOnGrab = false;
    [Tooltip("Grabbable component to detect grab (leave empty to auto-find on this GameObject)")]
    [SerializeField] private Grabbable grabbable;

    [Header("Highlight Offset Controls (Editor Only)")]
    [Tooltip("Enable to preview and adjust highlight offset in editor")]
    [SerializeField] private bool enableOffsetPreview = false;
    [Tooltip("Position offset for highlight mesh")]
    [SerializeField] private Vector3 highlightPositionOffset = Vector3.zero;
    [Tooltip("Scale multiplier for highlight mesh")]
    [SerializeField] private Vector3 highlightScaleOffset = Vector3.one;

    [Header("Highlight Settings")]
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private MeshRenderer targetRenderer;
    [SerializeField] private int materialIndex = 0; // Which material slot to replace

    [Header("Options")]
    [SerializeField] private bool addAsOverlay = true; // Add to materials instead of replacing
    [Tooltip("Highlight scale multiplier - makes highlight slightly bigger")]
    [SerializeField] private float highlightScale = 1.02f;

    [Header("Simple GameObject Mode")]
    [Tooltip("If checked, simply enables/disables this GameObject instead of material manipulation")]
    [SerializeField] private bool simpleGameObjectMode = false;

    private Material[] originalMaterials;
    private GameObject highlightObject;
    private bool isHighlighted = false;

    private void OnEnable()
    {
        TaskEvents.OnTaskActive += OnTaskActive;
        TaskEvents.OnTaskCompleted += OnTaskCompleted;

        // Subscribe to grab if enabled
        if (hideOnGrab)
        {
            SetupGrabbableEvents();
        }
    }

    private void OnDisable()
    {
        TaskEvents.OnTaskActive -= OnTaskActive;
        TaskEvents.OnTaskCompleted -= OnTaskCompleted;

        // Unsubscribe from grab
        if (hideOnGrab && grabbable != null)
        {
            grabbable.OnGrabEvent -= OnGrabbed;
        }
    }

    private void SetupGrabbableEvents()
    {
        // Auto-find grabbable if not assigned
        if (grabbable == null)
        {
            grabbable = GetComponent<Grabbable>();
            if (grabbable == null)
            {
                grabbable = GetComponentInChildren<Grabbable>();
            }
        }

        // Subscribe to grab event only (not release)
        if (grabbable != null)
        {
            grabbable.OnGrabEvent += OnGrabbed;
        }
        else
        {
            Debug.LogWarning($"TaskHighlight on {gameObject.name}: hideOnGrab is enabled but no Grabbable component found!");
        }
    }

    private void OnGrabbed(Hand hand, Grabbable grabbable)
    {
        if (isHighlighted)
        {
            Debug.Log($"TaskHighlight: {gameObject.name} grabbed - hiding highlight immediately");
            HideHighlight();
        }
    }

    private void Start()
    {
        if (!simpleGameObjectMode)
        {
            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<MeshRenderer>();
            }

            if (targetRenderer != null)
            {
                originalMaterials = targetRenderer.materials;
            }
        }

        // Start hidden (unless in editor preview mode)
#if UNITY_EDITOR
        if (!enableOffsetPreview)
        {
            HideHighlight();
        }
#else
        HideHighlight();
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Editor preview: Show/hide highlight based on checkbox
        if (enableOffsetPreview && Application.isPlaying)
        {
            // In play mode with preview enabled, show highlight
            if (!isHighlighted)
            {
                ShowHighlight();
            }
            else
            {
                // Update existing highlight with new offset values
                UpdateHighlightOffset();
            }
        }
        else if (!enableOffsetPreview && isHighlighted && Application.isPlaying)
        {
            // Preview disabled, hide highlight
            HideHighlight();
        }
    }

    private void UpdateHighlightOffset()
    {
        if (highlightObject != null)
        {
            highlightObject.transform.localPosition = highlightPositionOffset;
            highlightObject.transform.localScale = highlightScaleOffset * highlightScale;
        }
    }
#endif

    private void OnTaskActive(TaskID activeTaskID)
    {
        // Check if we should show highlight for this task
        bool shouldShow = false;

        // NEW: Check showOnTasks array (multiple tasks)
        if (showOnTasks != null && showOnTasks.Length > 0)
        {
            foreach (var task in showOnTasks)
            {
                if (activeTaskID == task)
                {
                    shouldShow = true;
                    break;
                }
            }
        }
        // BACKWARDS COMPATIBILITY: Check single taskID if showOnTasks is empty
        else if (activeTaskID == this.taskID)
        {
            shouldShow = true;
        }

        if (shouldShow)
        {
            ShowHighlight();
        }
        else
        {
            // Check if we should hide on this task
            if (hideOnTasks != null)
            {
                foreach (var hideTask in hideOnTasks)
                {
                    if (activeTaskID == hideTask)
                    {
                        HideHighlight();
                        return;
                    }
                }
            }
        }
    }

    private void OnTaskCompleted(TaskID completedTaskID)
    {
        // Hide when any of our tasks complete
        bool shouldHide = false;

        // NEW: Check showOnTasks array
        if (showOnTasks != null && showOnTasks.Length > 0)
        {
            foreach (var task in showOnTasks)
            {
                if (completedTaskID == task)
                {
                    shouldHide = true;
                    break;
                }
            }
        }
        // BACKWARDS COMPATIBILITY: Check single taskID
        else if (completedTaskID == this.taskID)
        {
            shouldHide = true;
        }

        if (shouldHide)
        {
            HideHighlight();
        }
    }

    private void ShowHighlight()
    {
        // if (isHighlighted)
        //     return;

        if (simpleGameObjectMode)
        {
            // Simple mode - just enable this GameObject
            gameObject.SetActive(true);
        }
        else
        {
            if (targetRenderer == null || highlightMaterial == null)
                return;

            if (addAsOverlay)
            {
                CreateHighlightOverlay();
            }
            else
            {
                ReplaceHighlightMaterial();
            }
        }

        isHighlighted = true;
        Debug.Log($"Highlight shown on {gameObject.name} for task {taskID.ID}");
    }

    private void HideHighlight()
    {
        if (!isHighlighted && !simpleGameObjectMode)
            return;

        if (simpleGameObjectMode)
        {
            // Simple mode - just disable this GameObject
            gameObject.SetActive(false);
        }
        else
        {
            if (addAsOverlay && highlightObject != null)
            {
                Destroy(highlightObject);
            }
            else if (targetRenderer != null && originalMaterials != null)
            {
                targetRenderer.materials = originalMaterials;
            }
        }

        isHighlighted = false;
    }

    private void CreateHighlightOverlay()
    {
        // Create a duplicate mesh with highlight material
        highlightObject = new GameObject($"{gameObject.name}_Highlight");
        highlightObject.transform.SetParent(transform);

        // Apply position offset
        highlightObject.transform.localPosition = highlightPositionOffset;
        highlightObject.transform.localRotation = Quaternion.identity;

        // Apply scale: base highlightScale * custom scale offset
        Vector3 finalScale = Vector3.Scale(Vector3.one * highlightScale, highlightScaleOffset);
        highlightObject.transform.localScale = finalScale;

        // Copy mesh
        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();
        if (originalMeshFilter != null)
        {
            MeshFilter meshFilter = highlightObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = originalMeshFilter.sharedMesh;
        }

        // Add highlight material
        MeshRenderer renderer = highlightObject.AddComponent<MeshRenderer>();
        renderer.material = highlightMaterial;
    }

    private void ReplaceHighlightMaterial()
    {
        // Replace material at specified index
        if (targetRenderer != null && originalMaterials != null)
        {
            Material[] newMaterials = (Material[])originalMaterials.Clone();

            if (materialIndex >= 0 && materialIndex < newMaterials.Length)
            {
                newMaterials[materialIndex] = highlightMaterial;
            }

            targetRenderer.materials = newMaterials;
        }
    }

    private void OnDestroy()
    {
        if (highlightObject != null)
        {
            Destroy(highlightObject);
        }
    }
}
