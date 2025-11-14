using UnityEngine;
using Autohand;

/// <summary>
/// Helper script to connect a PhysicsGadgetButton to a ConfirmationButtonTask
/// Attach this to the same GameObject as PhysicsGadgetButton
/// </summary>
public class ConfirmationButton : MonoBehaviour
{
    [SerializeField] private ConfirmationButtonTask confirmationTask;

    // Called by PhysicsGadgetButton.OnPressed UnityEvent
    public void OnButtonPressed()
    {
        Debug.Log($"[ConfirmationButton] OnButtonPressed() called on GameObject: {gameObject.name}");

        if (confirmationTask != null)
        {
            Debug.Log($"[ConfirmationButton] Calling confirmationTask.OnConfirmationButtonPressed()");
            confirmationTask.OnConfirmationButtonPressed();
        }
        else
        {
            Debug.LogError("ConfirmationButton: No ConfirmationButtonTask assigned!");
        }
    }
}
