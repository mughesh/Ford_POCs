using UnityEngine;

/// <summary>
/// Final task that shows completion screen
/// Auto-completes after showing the screen
/// </summary>
public class CompletionTask : Task
{
    [Header("Completion Screen")]
    [SerializeField] private GameObject completionScreen;

    [Header("Optional - Audio")]
    [SerializeField] private AudioClip completionSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Optional - Display Time")]
    [SerializeField] private Timer timer;
    [SerializeField] private TMPro.TextMeshProUGUI timeDisplayText;

    private bool taskCompleted = false;
    private bool isTaskActive = false;

    public override void OnEnable()
    {
        base.OnEnable();
        TaskEvents.OnTaskActive += OnTaskActive;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TaskEvents.OnTaskActive -= OnTaskActive;
    }

    private void Start()
    {
        // Hide completion screen initially
        if (completionScreen != null)
        {
            completionScreen.SetActive(false);
        }
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        if (this.TaskID == activeTaskID)
        {
            isTaskActive = true;
            ShowCompletionScreen();
        }
    }

    private void ShowCompletionScreen()
    {
        if (taskCompleted) return;

        Debug.Log("CompletionTask: Showing completion screen!");

        // Show completion screen
        if (completionScreen != null)
        {
            completionScreen.SetActive(true);
        }

        // Display final time
        if (timer != null && timeDisplayText != null)
        {
            timeDisplayText.text = $"Completion Time: {timer.GetTime()}";
        }

        // Play completion sound
        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }

        taskCompleted = true;

        // Task completes immediately (this triggers timer stop in TaskManager)
        TriggerComplete();
    }

    public override void TriggerComplete()
    {
        // Complete the task
        TaskEvents.TaskCompleted(this.TaskID);
    }
}
