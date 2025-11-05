using UnityEngine;
using UnityEngine.Events;
public abstract class Task : MonoBehaviour
{
    public TaskID TaskID;
    public UnityEvent OnTaskStarted;
    public UnityEvent OnTaskCompleted;
    public UnityEvent OnTaskPreCompleted;

    [Header("Debug")]
    public bool complete;
#if UNITY_EDITOR_WIN
    private void OnValidate()
    {
        if (complete)
        {
            CompleteTask();
            complete = false;
        }
    }
#endif
    public virtual void OnEnable()
    {
        TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
        TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
        TaskEvents.OnTaskPreCompleted += TaskEvents_OnTaskPreCompleted;
    }
    public virtual void OnDisable()
    {
        TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
        TaskEvents.OnTaskCompleted -= TaskEvents_OnTaskCompleted;
        TaskEvents.OnTaskPreCompleted -= TaskEvents_OnTaskPreCompleted;
    }
    private void TaskEvents_OnTaskPreCompleted(TaskID obj)
    {
        if (this.TaskID == obj)
            OnTaskPreCompleted?.Invoke();
    }
    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        if (this.TaskID == obj)
            OnTaskCompleted?.Invoke();
    }
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (this.TaskID == obj)
            OnTaskStarted?.Invoke();
    }

    public void CompleteTask()
    {
        TaskEvents.TaskCompleted(this.TaskID);
    }
    public abstract void TriggerComplete();
}