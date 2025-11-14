using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Records transform snapshots during Play mode and persists them using ScriptableObject
/// This version ACTUALLY saves data after exiting Play mode!
/// </summary>
public class TransformSnapshotRecorder_SO : MonoBehaviour
{
    [Header("Snapshot Storage (REQUIRED)")]
    [Tooltip("ScriptableObject that stores the snapshots (persists after Play mode)")]
    [SerializeField] private TransformSnapshotData snapshotData;

    [Header("Recording Controls")]
    [Tooltip("Press this button in Play mode to record current transform")]
    [SerializeField] private bool recordSnapshot = false;

    [Header("Apply Snapshot")]
    [Tooltip("Index of snapshot to apply to this GameObject")]
    [SerializeField] private int applySnapshotIndex = 0;
    [Tooltip("Press this button to apply selected snapshot")]
    [SerializeField] private bool applySnapshot = false;

    [Header("Utilities")]
    [Tooltip("Clear all recorded snapshots")]
    [SerializeField] private bool clearAllSnapshots = false;

    private void Update()
    {
        if (snapshotData == null)
        {
            Debug.LogError("[TransformRecorder] No TransformSnapshotData assigned! Create one via Assets > Create > Sim2 > Transform Snapshot Data");
            return;
        }

        // Record snapshot button
        if (recordSnapshot)
        {
            recordSnapshot = false;
            RecordCurrentTransform();
        }

        // Apply snapshot button
        if (applySnapshot)
        {
            applySnapshot = false;
            ApplySnapshotAtIndex(applySnapshotIndex);
        }

        // Clear button
        if (clearAllSnapshots)
        {
            clearAllSnapshots = false;
            ClearSnapshots();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (snapshotData == null)
            return;

        // Handle buttons in Edit mode
        if (!Application.isPlaying)
        {
            // Apply snapshot in edit mode
            if (applySnapshot)
            {
                applySnapshot = false;
                ApplySnapshotAtIndex(applySnapshotIndex);
            }

            // Clear in edit mode
            if (clearAllSnapshots)
            {
                clearAllSnapshots = false;
                ClearSnapshots();
            }
        }
    }
#endif

    private void RecordCurrentTransform()
    {
        string snapshotName = snapshotData.nextNumber.ToString();
        snapshotData.AddSnapshot(snapshotName, transform);

        Debug.Log($"<color=green>[TransformRecorder]</color> Recorded snapshot '{snapshotName}' at index {snapshotData.snapshots.Count - 1}");
        Debug.Log($"Position: {transform.position}, Rotation: {transform.rotation.eulerAngles}, Scale: {transform.localScale}");

        // Auto-increment
        snapshotData.nextNumber++;

#if UNITY_EDITOR
        // Mark ScriptableObject as dirty to save changes
        EditorUtility.SetDirty(snapshotData);
        AssetDatabase.SaveAssets();
        Debug.Log("<color=yellow>[TransformRecorder]</color> Snapshot saved to ScriptableObject!");
#endif
    }

    private void ApplySnapshotAtIndex(int index)
    {
        var snapshot = snapshotData.GetSnapshot(index);
        if (snapshot == null)
        {
            Debug.LogWarning($"<color=yellow>[TransformRecorder]</color> Invalid index {index}. Valid range: 0-{snapshotData.snapshots.Count - 1}");
            return;
        }

        snapshot.ApplyToTransform(transform);

        Debug.Log($"<color=cyan>[TransformRecorder]</color> Applied snapshot '{snapshot.name}' (index {index}) to {gameObject.name}");
        Debug.Log($"Position: {snapshot.position}, Rotation: {snapshot.rotation.eulerAngles}, Scale: {snapshot.scale}");
    }

    private void ClearSnapshots()
    {
        int count = snapshotData.snapshots.Count;
        snapshotData.ClearSnapshots();

        Debug.Log($"<color=red>[TransformRecorder]</color> Cleared {count} snapshots");

#if UNITY_EDITOR
        EditorUtility.SetDirty(snapshotData);
        AssetDatabase.SaveAssets();
#endif
    }

    // Public methods for external use
    public void RecordSnapshot()
    {
        RecordCurrentTransform();
    }

    public void ApplySnapshot(int index)
    {
        ApplySnapshotAtIndex(index);
    }

    public void ApplySnapshot(string name)
    {
        var snapshot = snapshotData.GetSnapshot(name);
        if (snapshot != null)
        {
            snapshot.ApplyToTransform(transform);
            Debug.Log($"<color=cyan>[TransformRecorder]</color> Applied snapshot '{name}' to {gameObject.name}");
        }
        else
        {
            Debug.LogWarning($"<color=yellow>[TransformRecorder]</color> Snapshot with name '{name}' not found!");
        }
    }
}
