using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] TaskID taskID;
    [SerializeField] GameObject arrow;
    [SerializeField] List<GameObject> optional;
    bool taskPreCompleted = false;
    bool canShow = true;
#if UNITY_EDITOR
    private void OnValidate()
    {
        this.gameObject.name = taskID.ToString() + "_Arrow";
    }
#endif
    private void Awake()
    {
        SetState(false);
        TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
        TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
        TaskEvents.OnTaskPreCompleted += TaskEvents_OnTaskPreCompleted;
        TaskEvents.OnArrowState += TaskEvents_OnArrowState;
    }

    private void OnDestroy()
    {
        TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
        TaskEvents.OnTaskCompleted -= TaskEvents_OnTaskCompleted;
        TaskEvents.OnTaskPreCompleted -= TaskEvents_OnTaskPreCompleted;
        TaskEvents.OnArrowState -= TaskEvents_OnArrowState;
    }
    private void TaskEvents_OnArrowState(bool obj)
    {
        canShow = obj;
    }
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (obj.Equals(taskID) && !taskPreCompleted)
            SetState(true);
    }
    private void TaskEvents_OnTaskPreCompleted(TaskID obj)
    {
        if (obj.Equals(taskID))
            taskPreCompleted = true;
    }
    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        if (obj.Equals(taskID) && !taskPreCompleted)
            SetState(false);
    }
    void SetState(bool state)
    {
        //if(!canShow) return;
        arrow.SetActive(state);
        if (optional.Count > 0)
            optional.ForEach(x => x.SetActive(state));
    }
}