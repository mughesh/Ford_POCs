using System;
public static class DemoEvents
{
    public static event Action<TaskID> OnAnimStart;
    public static event Action<TaskID> OnAnimEnd;
    public static event Action OnAnimCancel;

    public static void Startanim(TaskID taskID)
    {
        OnAnimStart?.Invoke(taskID);
    }
    public static void Endanim(TaskID taskID)
    {
        OnAnimEnd?.Invoke(taskID);
    }
    public static void CancelAnim()
    {
        OnAnimCancel?.Invoke();
    }
}