using UnityEngine;

/// <summary>
/// Helper component to connect PhysicsGadgetButton to TeleportTask
/// Attach this to the same GameObject as PhysicsGadgetButton
/// </summary>
public class TeleportButton : MonoBehaviour
{
    [Header("Task Reference")]
    [SerializeField] private TeleportTask teleportTask;

    // Call this from PhysicsGadgetButton's OnPressed UnityEvent
    public void OnButtonPressed()
    {
        if (teleportTask != null)
        {
            teleportTask.OnTeleportButtonPressed();
        }
        else
        {
            Debug.LogWarning("TeleportButton: No TeleportTask reference assigned!");
        }
    }
}
