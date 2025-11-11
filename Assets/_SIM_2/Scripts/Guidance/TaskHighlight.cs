using UnityEngine;

/// <summary>
/// Highlights a mesh during a specific task by applying highlight material
/// Similar to arrow guidance but for mesh highlighting
/// </summary>
public class TaskHighlight : MonoBehaviour
{
    [Header("Task Settings")]
    [SerializeField] private TaskID taskID;
    [Tooltip("Leave empty to show during this task. Add task IDs to hide during specific tasks.")]
    [SerializeField] private TaskID[] hideOnTasks;

    [Header("Highlight Settings")]
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private MeshRenderer targetRenderer;
    [SerializeField] private int materialIndex = 0; // Which material slot to replace

    [Header("Options")]
    [SerializeField] private bool addAsOverlay = true; // Add to materials instead of replacing
    [Tooltip("Highlight scale multiplier - makes highlight slightly bigger")]
    [SerializeField] private float highlightScale = 1.02f;

    private Material[] originalMaterials;
    private GameObject highlightObject;
    private bool isHighlighted = false;

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
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<MeshRenderer>();
        }

        if (targetRenderer != null)
        {
            originalMaterials = targetRenderer.materials;
        }

        // Start hidden
        HideHighlight();
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        // Check if this is our task
        if (activeTaskID == this.taskID)
        {
            ShowHighlight();
        }
        else
        {
            // Check if we should hide on this task
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

    private void OnTaskCompleted(TaskID completedTaskID)
    {
        // Hide when our task completes
        if (completedTaskID == this.taskID)
        {
            HideHighlight();
        }
    }

    private void ShowHighlight()
    {
        if (isHighlighted || targetRenderer == null || highlightMaterial == null)
            return;

        if (addAsOverlay)
        {
            CreateHighlightOverlay();
        }
        else
        {
            ReplaceHighlightMaterial();
        }

        isHighlighted = true;
        Debug.Log($"Highlight shown on {gameObject.name} for task {taskID.ID}");
    }

    private void HideHighlight()
    {
        if (!isHighlighted)
            return;

        if (addAsOverlay && highlightObject != null)
        {
            Destroy(highlightObject);
        }
        else if (targetRenderer != null && originalMaterials != null)
        {
            targetRenderer.materials = originalMaterials;
        }

        isHighlighted = false;
    }

    private void CreateHighlightOverlay()
    {
        // Create a duplicate mesh with highlight material slightly scaled up
        highlightObject = new GameObject($"{gameObject.name}_Highlight");
        highlightObject.transform.SetParent(transform);
        highlightObject.transform.localPosition = Vector3.zero;
        highlightObject.transform.localRotation = Quaternion.identity;
        highlightObject.transform.localScale = Vector3.one * highlightScale;

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
