using UnityEngine;
using Autohand;

/// <summary>
/// Hides guidance GameObjects (arrows, highlights, etc.) when a button is pressed
/// Instead of waiting for task completion
/// </summary>
public class HideOnButtonPress : MonoBehaviour
{
    [Header("What to Hide")]
    [Tooltip("GameObjects to hide when button is pressed (arrows, highlights, etc.)")]
    [SerializeField] private GameObject[] objectsToHide;

    [Header("Button Reference")]
    [Tooltip("The button that triggers hiding")]
    [SerializeField] private PhysicsGadgetButton button;

    private bool hasHidden = false;

    private void OnEnable()
    {
        if (button != null)
        {
            button.OnPressed.AddListener(OnButtonPressed);
        }

        // Reset state
        hasHidden = false;
    }

    private void OnDisable()
    {
        if (button != null)
        {
            button.OnPressed.RemoveListener(OnButtonPressed);
        }
    }

    private void OnButtonPressed()
    {
        if (hasHidden) return;

        HideGuidance();
        hasHidden = true;
    }

    private void HideGuidance()
    {
        foreach (var obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Hidden guidance: {obj.name}");
            }
        }
    }

    // Public method to manually hide
    public void Hide()
    {
        HideGuidance();
    }

    // Public method to show again (useful for re-enabling on task start)
    public void Show()
    {
        hasHidden = false;

        foreach (var obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
