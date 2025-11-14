using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject to store transform snapshots
/// This persists data even when exiting Play mode
/// </summary>
[CreateAssetMenu(fileName = "TransformSnapshots", menuName = "Sim2/Transform Snapshot Data")]
public class TransformSnapshotData : ScriptableObject
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
    public List<TransformSnapshot> snapshots = new List<TransformSnapshot>();

    [Header("Auto-Increment Counter")]
    public int nextNumber = 1;

    public void AddSnapshot(string name, Transform t)
    {
        TransformSnapshot snapshot = new TransformSnapshot(name, t);
        snapshots.Add(snapshot);
    }

    public void ClearSnapshots()
    {
        snapshots.Clear();
        nextNumber = 1;
    }

    public TransformSnapshot GetSnapshot(int index)
    {
        if (index >= 0 && index < snapshots.Count)
            return snapshots[index];
        return null;
    }

    public TransformSnapshot GetSnapshot(string name)
    {
        return snapshots.Find(s => s.name == name);
    }
}
