using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Records transform snapshots during Play mode and persists them after exiting
/// Useful for recording guidance animation positions for each step
/// IMPORTANT: This component must be on a GameObject that exists in the scene (not runtime-instantiated)
/// </summary>
[ExecuteInEditMode]
public class TransformSnapshotRecorder : MonoBehaviour
{
    [System.Serializable]
    public class TransformSnapshot
    {
        [Tooltip("Name/number for this snapshot (e.g., '17', '18', etc.)")]
        public string name = "1";
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        [Header("Read-Only Info")]
        [TextArea(2, 4)]
        public string info = "";

        public TransformSnapshot(string name, Transform t)
        {
            this.name = name;
            CaptureTransform(t);
        }

        public void CaptureTransform(Transform t)
        {
            position = t.position;
            rotation = t.rotation;
            scale = t.localScale;
            UpdateInfo();
        }

        public void ApplyToTransform(Transform t)
        {
            t.position = position;
            t.rotation = rotation;
            t.localScale = scale;
        }

        private void UpdateInfo()
        {
            info = $"Pos: ({position.x:F3}, {position.y:F3}, {position.z:F3})\n" +
                   $"Rot: ({rotation.eulerAngles.x:F1}, {rotation.eulerAngles.y:F1}, {rotation.eulerAngles.z:F1})\n" +
                   $"Scale: ({scale.x:F3}, {scale.y:F3}, {scale.z:F3})";
        }
    }

    [Header("Recorded Snapshots")]
    [Tooltip("List of all recorded transform snapshots")]
    [SerializeField] private List<TransformSnapshot> snapshots = new List<TransformSnapshot>();

    [Header("Recording Controls")]
    [Tooltip("Name for the next snapshot (auto-increments if numeric)")]
    [SerializeField] private string nextSnapshotName = "1";

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
        // Only process in Play mode
        if (!Application.isPlaying)
            return;

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
        // Handle buttons in Edit mode
        if (!Application.isPlaying)
        {
            // Apply snapshot in edit mode
            if (applySnapshot)
            {
                applySnapshot = false;
                ApplySnapshotAtIndex(applySnapshotIndex);
                EditorUtility.SetDirty(this);
            }

            // Clear in edit mode
            if (clearAllSnapshots)
            {
                clearAllSnapshots = false;
                ClearSnapshots();
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif

    private void RecordCurrentTransform()
    {
        TransformSnapshot snapshot = new TransformSnapshot(nextSnapshotName, transform);
        snapshots.Add(snapshot);

        Debug.Log($"<color=green>[TransformRecorder]</color> Recorded snapshot '{nextSnapshotName}' at index {snapshots.Count - 1}");
        Debug.Log($"Position: {snapshot.position}, Rotation: {snapshot.rotation.eulerAngles}, Scale: {snapshot.scale}");

        // Auto-increment name if it's a number
        if (int.TryParse(nextSnapshotName, out int number))
        {
            nextSnapshotName = (number + 1).ToString();
        }

#if UNITY_EDITOR
        // Mark dirty so Unity saves the changes
        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(gameObject);

        // Force save the scene
        if (!Application.isPlaying)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }

    private void ApplySnapshotAtIndex(int index)
    {
        if (index < 0 || index >= snapshots.Count)
        {
            Debug.LogWarning($"<color=yellow>[TransformRecorder]</color> Invalid index {index}. Valid range: 0-{snapshots.Count - 1}");
            return;
        }

        TransformSnapshot snapshot = snapshots[index];
        snapshot.ApplyToTransform(transform);

        Debug.Log($"<color=cyan>[TransformRecorder]</color> Applied snapshot '{snapshot.name}' (index {index}) to {gameObject.name}");
        Debug.Log($"Position: {snapshot.position}, Rotation: {snapshot.rotation.eulerAngles}, Scale: {snapshot.scale}");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private void ClearSnapshots()
    {
        int count = snapshots.Count;
        snapshots.Clear();
        nextSnapshotName = "1";

        Debug.Log($"<color=red>[TransformRecorder]</color> Cleared {count} snapshots");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
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
        TransformSnapshot snapshot = snapshots.Find(s => s.name == name);
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

    public int GetSnapshotCount()
    {
        return snapshots.Count;
    }

    public List<TransformSnapshot> GetSnapshots()
    {
        return snapshots;
    }
}
