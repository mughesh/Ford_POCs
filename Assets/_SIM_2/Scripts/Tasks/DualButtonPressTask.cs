using UnityEngine;
using Autohand;

/// <summary>
/// Task for pressing two buttons simultaneously
/// Detects when BOTH buttons are in pressed state at the same time
/// Used for VLRG arm engagement
/// </summary>
public class DualButtonPressTask : Task
{
    [Header("Button References")]
    [SerializeField] private PhysicsGadgetButton button1;
    [SerializeField] private PhysicsGadgetButton button2;

    [Header("Animation")]
    [SerializeField] private Animator vlrgAnimator;
    [Tooltip("Name of the animation trigger parameter (e.g., 'Engage')")]
    [SerializeField] private string animationTriggerName = "Engage";
    [Tooltip("How long to wait before completing task (should match animation length)")]
    [SerializeField] private float animationDuration = 2f;

    [Header("Steering Deck Attachment")]
    [Tooltip("The steering deck/instrument panel object to attach to the tool")]
    [SerializeField] private Transform steeringDeck;
    [Tooltip("The decking tool parent (steering deck will become child of this)")] 
    [SerializeField] private Transform deckingToolParent;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip engagementSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Guidance (Optional)")]
    [Tooltip("Objects to hide when both buttons are pressed")]
    [SerializeField] private GameObject[] hideOnPress;

    private bool taskCompleted = false;
    private bool isTaskActive = false;
    private bool bothButtonsPressed = false;
    private bool engagementStarted = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= OnTaskActive;
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        isTaskActive = (this.TaskID == activeTaskID);

        if (isTaskActive)
        {
            Debug.Log("DualButtonPressTask active - waiting for both buttons to be pressed simultaneously");
        }
    }

    private void Update()
    {
        if (!isTaskActive || taskCompleted || engagementStarted) return;

        // Check if BOTH buttons are currently in pressed state
        if (button1 != null && button2 != null)
        {
            // This uses reflection to access the private 'pressed' field
            // Alternative: You could make the field public in PhysicsGadgetButton
            bool button1Pressed = IsButtonPressed(button1);
            bool button2Pressed = IsButtonPressed(button2);

            if (button1Pressed && button2Pressed && !bothButtonsPressed)
            {
                // Both buttons are pressed simultaneously!
                bothButtonsPressed = true;
                OnBothButtonsPressed();
            }
        }
    }

    // Helper method to check if a PhysicsGadgetButton is currently pressed
    private bool IsButtonPressed(PhysicsGadgetButton button)
    {
        // Use reflection to access the private 'pressed' field
        var fieldInfo = typeof(PhysicsGadgetButton).GetField("pressed",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (fieldInfo != null)
        {
            return (bool)fieldInfo.GetValue(button);
        }

        Debug.LogWarning("Could not access 'pressed' field on PhysicsGadgetButton");
        return false;
    }

    private void OnBothButtonsPressed()
    {
        engagementStarted = true;

        Debug.Log("Both buttons pressed simultaneously! Engaging VLRG arm...");

        // Hide guidance
        HideGuidance();

        // Play engagement sound
        if (audioSource != null && engagementSound != null)
        {
            audioSource.PlayOneShot(engagementSound);
        }

        // Start VLRG arm engagement animation
        EngageVLRGArm();
    }

    private void EngageVLRGArm()
    {
        if (vlrgAnimator == null)
        {
            Debug.LogError("DualButtonPressTask: Missing Animator reference!");
            taskCompleted = true;
            CompleteTask();
            return;
        }

        // Trigger the engagement animation
        vlrgAnimator.SetTrigger(animationTriggerName);
        Debug.Log($"Triggered animation: {animationTriggerName}");

        // Wait for animation to complete before finishing task
        StartCoroutine(WaitForAnimationComplete());
    }

    private System.Collections.IEnumerator WaitForAnimationComplete()
    {
        // Wait for the animation duration
        yield return new UnityEngine.WaitForSeconds(animationDuration);

        // Animation complete
        OnEngagementComplete();
    }

    private void OnEngagementComplete()
    {
        Debug.Log("VLRG arm engaged with instrument panel!");

        // Attach steering deck to decking tool
        AttachSteeringDeck();

        taskCompleted = true;
        CompleteTask();
    }

    private void AttachSteeringDeck()
    {
        if (steeringDeck == null || deckingToolParent == null)
        {
            Debug.LogWarning("DualButtonPressTask: Missing steering deck or decking tool parent reference!");
            return;
        }

        // Make steering deck a child of the decking tool
        steeringDeck.SetParent(deckingToolParent);

        Debug.Log($"Steering deck '{steeringDeck.name}' attached to '{deckingToolParent.name}'");
    }

    private void HideGuidance()
    {
        foreach (var obj in hideOnPress)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Hidden guidance: {obj.name}");
            }
        }
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
