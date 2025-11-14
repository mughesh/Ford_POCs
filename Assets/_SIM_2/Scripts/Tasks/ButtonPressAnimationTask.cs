using UnityEngine;
using Autohand;
using System.Collections;

/// <summary>
/// Simple task for button press that triggers animations
/// Pattern: Press button → Animation plays → Task completes
/// Used for VLRG arm disengagement, machine operations, etc.
/// </summary>
public class ButtonPressAnimationTask : Task
{
    [Header("Button")]
    [SerializeField] private PhysicsGadgetButton button;

    [Header("Animations")]
    [Tooltip("Animators to trigger when button is pressed")]
    [SerializeField] private Animator[] animatorsToTrigger;
    [Tooltip("Animation trigger parameter name")]
    [SerializeField] private string animationTriggerName = "Retract";
    [Tooltip("How long to wait for animations to complete before finishing task")]
    [SerializeField] private float animationDuration = 2f;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private AudioClip animationSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Guidance (Optional)")]
    [Tooltip("Objects to hide when button is pressed")]
    [SerializeField] private GameObject[] hideOnPress;

    [Header("Debug - Test Animation")]
    [Tooltip("Enable this checkbox to test animation in Play mode (will auto-disable after triggering)")]
    [SerializeField] private bool debugTriggerAnimation = false;

    private bool taskCompleted = false;
    private bool isTaskActive = false;
    private bool buttonPressed = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;

        // Subscribe to button press
        if (button != null)
        {
            button.OnPressed.AddListener(OnButtonPressed);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= OnTaskActive;

        // Unsubscribe from button
        if (button != null)
        {
            button.OnPressed.RemoveListener(OnButtonPressed);
        }
    }

    private void Update()
    {
        // Debug: Trigger animation when checkbox is enabled
        if (debugTriggerAnimation)
        {
            debugTriggerAnimation = false; // Auto-disable after triggering
            DebugTriggerAnimation();
        }
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        isTaskActive = (this.TaskID == activeTaskID);
    }

    private void OnButtonPressed()
    {
        if (!isTaskActive || taskCompleted || buttonPressed) return;

        buttonPressed = true;

        Debug.Log("Button pressed - triggering animations!");

        // Play button press sound
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound);
        }

        // Hide guidance
        HideGuidance();

        // Trigger animations
        StartCoroutine(PlayAnimationsAndComplete());
    }

    private IEnumerator PlayAnimationsAndComplete()
    {
        // Trigger animations on all animators
        foreach (var animator in animatorsToTrigger)
        {
            if (animator != null)
            {
                animator.SetTrigger(animationTriggerName);
                Debug.Log($"Triggered animation on: {animator.gameObject.name}");
            }
        }

        // Play animation sound
        if (audioSource != null && animationSound != null)
        {
            audioSource.PlayOneShot(animationSound);
        }

        // Wait for animations to complete
        yield return new WaitForSeconds(animationDuration);

        Debug.Log("Animations completed!");

        // Complete task
        taskCompleted = true;
        CompleteTask();
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

    private void DebugTriggerAnimation()
    {
        if (animatorsToTrigger == null || animatorsToTrigger.Length == 0)
        {
            Debug.LogWarning("ButtonPressAnimationTask: No animators assigned for debug trigger!");
            return;
        }

        Debug.Log("[DEBUG] Triggering animations manually...");

        // Trigger animations on all animators
        foreach (var animator in animatorsToTrigger)
        {
            if (animator != null)
            {
                animator.SetTrigger(animationTriggerName);
                Debug.Log($"[DEBUG] Triggered animation on: {animator.gameObject.name}");
            }
        }

        // Play animation sound
        if (audioSource != null && animationSound != null)
        {
            audioSource.PlayOneShot(animationSound);
        }
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
