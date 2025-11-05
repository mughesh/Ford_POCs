using System;
using UnityEngine;
public static class TaskEvents
{
    public static event Action<TaskID> OnTaskActive;
    public static event Action<TaskID> OnTaskCompleted;
    public static event Action<TaskID> OnTaskPreCompleted;
    public static event Action<int> OnAllTasksCompleted;
    public static event Action<Transform, GuideArrowAxis> OnLookAt;
    public static event Action OnStopLookAtArrow;
    public static event Action<float> OnProgressUpdate;
    public static event Action<Transform, bool> OnProgressUpdateTransform;
    public static void StartTask(TaskID taskID) => OnTaskActive?.Invoke(taskID);
    public static void TaskCompleted(TaskID taskID) => OnTaskCompleted?.Invoke(taskID);
    public static void TaskPreCompleted(TaskID taskID) => OnTaskPreCompleted?.Invoke(taskID);

    public static void LootAt(Transform obj, GuideArrowAxis guideArrowAxis) => OnLookAt?.Invoke(obj, guideArrowAxis);

    public static void StopLookAtArrow() => OnStopLookAtArrow?.Invoke();
    public static void AllTasksCompleted(int listID) => OnAllTasksCompleted?.Invoke(listID);
    public static void UpdateProgressBar(float progress)
    {
        Debug.Log("update progress bar calle");
        OnProgressUpdate?.Invoke(progress);
    }
    public static void UpdateProgressBarTransform(Transform place, bool state) => OnProgressUpdateTransform?.Invoke(place, state);
}
