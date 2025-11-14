using UnityEngine;

/// <summary>
/// Resets the player rig to a specific position/rotation when the app starts
/// Based on TeleportTask logic for consistency
/// </summary>
public class ResetPlayerOnStart : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("The player rig root transform (Auto Hand Player)")]
    [SerializeField] private Transform playerTransform;

    [Header("Reset Target")]
    [Tooltip("Position/rotation to reset player to on start")]
    [SerializeField] private Transform resetTarget;

    [Header("Tracking Offset Reset (For Auto Hands)")]
    [Tooltip("Optional: Transforms to reset to zero (TrackerOffsets, Auto Hand Player children)")]
    [SerializeField] private Transform[] resetTransformsToZero;

    private void Start()
    {
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        if (resetTarget == null || playerTransform == null)
        {
            Debug.LogWarning("ResetPlayerOnStart: Missing reset target or player transform!");
            return;
        }

        // Reset player position and rotation
        playerTransform.position = resetTarget.position;
        playerTransform.rotation = resetTarget.rotation;

        Debug.Log($"ResetPlayerOnStart: Player reset to {resetTarget.name} at position {resetTarget.position}");

        // Reset tracking offsets to zero (fixes VR tracking drift issue)
        if (resetTransformsToZero != null && resetTransformsToZero.Length > 0)
        {
            foreach (var t in resetTransformsToZero)
            {
                if (t != null)
                {
                    t.localPosition = Vector3.zero;
                    t.localRotation = Quaternion.identity;
                }
            }
            Debug.Log("ResetPlayerOnStart: Reset tracking offsets to zero");
        }
    }

    // Call this method to reset player at runtime
    public void ResetPlayerPosition()
    {
        ResetPlayer();
    }
}
