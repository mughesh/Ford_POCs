using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [SerializeField] TaskSequence taskSequenceRef;
    [SerializeField] TaskDescription taskDescription;
    [SerializeField] Transform taskSpawnPoint;
    [SerializeField] AudioSource taskAudioSource;
    [SerializeField] AudioSource tickAudioSource;
    [SerializeField] Timer Timer;
    [SerializeField] LastPlayedAudio lastPlayedAudio;
    public Scrollbar scrollbar;      // assign in inspector
    public int itemCount = 10;       // number of list elements (must be >= 2)
    public float stepDuration = 0.25f;

    Coroutine running;


    private TaskSequence taskSequence;
    private Dictionary<TaskID, Task> tasks = new();
    private int currentTaskIndex = 0;
    private int currentSubTaskIndex = 0;
    private bool isStarting;

    public TaskID currentMainTask;
    public TaskID currentSubTask;
    public bool skipTimer;

    public List<TaskDescription> taskList;
    [SerializeField] AudioClip finalMsg;
    //public static TaskManager Instance { get; private set; }
    private void Awake()
    {
        /* if (Instance != null)
             Destroy(Instance);
         Instance = this;*/
        taskSequence = Instantiate(taskSequenceRef);
    }
    private void OnEnable()
    {
        TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
        TaskEvents.OnAllTasksCompleted += TaskEvents_OnAllTasksCompleted;
        Populate();
    }

    private void TaskEvents_OnAllTasksCompleted(int obj)
    {
        if (obj != taskSequenceRef.sequenceNumber)
        {
            Timer.StopTimer();
        }
    }

    private void OnDisable()
    {
        TaskEvents.OnTaskCompleted -= TaskEvents_OnTaskCompleted;
    }
    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        bool newlyCompleted = false;

        if (MarkTaskCompleted(taskList, obj, t => t.taskid, t => t.IsCompleted, (t, v) => t.IsCompleted = v)
            == TaskMarkResult.NewlyCompleted)
        {
            newlyCompleted = true;
            tickAudioSource.Play();
            Next();
        }

        if (MarkTaskCompleted(taskSequence.Tasks, obj, t => t.TaskID, t => t.isCompleted, (t, v) => t.isCompleted = v)
            == TaskMarkResult.NewlyCompleted)
            newlyCompleted = true;

        foreach (var item in taskSequence.Tasks)
        {
            if (item.SubTasks.Count > 0)
            {
                var result = MarkTaskCompleted(item.SubTasks, obj,
                    t => t.TaskID,
                    t => t.IsCompleted,
                    (t, v) => t.IsCompleted = v);

                if (result == TaskMarkResult.NewlyCompleted)
                    newlyCompleted = true;
            }
        }

        // Only move forward if something was newly completed
        if (newlyCompleted)
        {
            StartNextTask();
        }
    }


    void Populate()
    {
        if (taskList.Count > 0)
            taskList.Clear();
        foreach (var item in taskSequence.Tasks)
        {
            TaskDescription taskD = Instantiate(taskDescription, taskSpawnPoint);
            taskD.Initialize(item.TaskID, item.Description, item.isCompleted);
            taskList.Add(taskD);
        }
        itemCount = taskSequence.Tasks.Count;
        Timer.StartTimer();
        StartNextTask();
    }

    private bool StartNextTask()
    {
        if (taskSequence?.Tasks == null || taskSequence.Tasks.Count == 0)
            return false;

        // ensure indices are sane
        if (currentTaskIndex < 0) currentTaskIndex = 0;
        if (isStarting) return false; // guard against re-entrant calls
        isStarting = true;

        try
        {
            while (currentTaskIndex < taskSequence.Tasks.Count)
            {
                var main = taskSequence.Tasks[currentTaskIndex];

                // null main safety
                if (main == null)
                {
                    currentTaskIndex++;
                    currentSubTaskIndex = 0;
                    continue;
                }
                if (main.isCompleted)
                {
                    currentTaskIndex++;
                    currentSubTaskIndex = 0;
                    isStarting = false;
                    StartNextTask();
                    return true;
                }
                var subList = main.SubTasks ?? new List<SubTasks>(); // avoid nulls

                // ensure the main-level start is fired once for this main task
                if (!Equals(main.TaskID, currentMainTask) && !main.isCompleted)
                {
                    TaskEvents.StartTask(main.TaskID);
                    currentMainTask = main.TaskID;
                    currentSubTaskIndex = 0;
                    StartCoroutine(PlayAudio(main.AudioClip, main.audioDelay));
                }

                // main has been started (above) and there are no subtasks to advance to
                if (subList.Count == 0)
                    return true;

                // iterate subtasks of current main
                while (currentSubTaskIndex < subList.Count)
                {
                    var sub = subList[currentSubTaskIndex];

                    // skip null or already completed subtasks
                    if (sub == null || sub.IsCompleted)
                    {
                        currentSubTaskIndex++;
                        continue;
                    }

                    // start this subtask and advance the index
                    TaskEvents.StartTask(sub.TaskID);
                    StartCoroutine(PlayAudio(sub.AudioClip, sub.audioDelay));
                    currentSubTask = sub.TaskID;
                    currentSubTaskIndex++;
                    return true; // we started exactly one subtask
                }
                bool allTrue = false;
                foreach (var item in subList)
                {
                    if (item.IsCompleted)
                        allTrue = true;
                    else
                    {
                        allTrue = false;
                        break;
                    }
                }
                if (!allTrue)
                {
                    currentSubTaskIndex = 0;
                    isStarting = false;
                    StartNextTask();
                    return false;
                }
                // finished all subtasks for this main — move to next main
                TaskEvents.TaskCompleted(currentMainTask);
                currentSubTaskIndex = 0;
                currentTaskIndex++;
            }
            StartCoroutine(PlayAudio(finalMsg, 1.5f));
            if (!skipTimer)
                Timer.StopTimer();
            TaskEvents.AllTasksCompleted(taskSequenceRef.sequenceNumber);
            // no more tasks
            return false;
        }
        finally
        {
            isStarting = false;
        }


    }

    IEnumerator PlayAudio(AudioClip clip, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (clip == null) yield break;
        taskAudioSource.clip = clip;
        taskAudioSource.Play();
        lastPlayedAudio.clip = clip;
    }
    public void Next()
    {
        StartStep(1);
    }

    // Call this to go to previous item (up/left depending on orientation)
    public void Prev()
    {
        StartStep(-1);
    }
    void StartStep(int direction)
    {
        float step = 1f / (itemCount - 1);
        //int currentIndex = Mathf.RoundToInt(scrollbar.value / step); // maps value -> nearest index
        //float targetValue = targetIndex * step;

        // If you find your scrollbar value is inverted (0 = bottom), use:
        int currentIndex = Mathf.RoundToInt((1f - scrollbar.value) / step);
        int targetIndex = Mathf.Clamp(currentIndex + direction, 0, itemCount - 1);
        float targetValue = 1f - (targetIndex * step);

        if (Mathf.Approximately(scrollbar.value, targetValue)) return;

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(LerpToValue(scrollbar, targetValue, stepDuration));
    }

    IEnumerator LerpToValue(Scrollbar sb, float target, float duration)
    {
        if (sb == null || duration <= 0f)
            yield break;

        float startValue = sb.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            sb.value = Mathf.Lerp(startValue, target, t);
            yield return null;
        }

        sb.value = target; // ensure exact
        running = null;
    }
    public enum TaskMarkResult
    {
        NotFound,         // Task ID not found in the list
        AlreadyCompleted, // Found, but IsCompleted was already true
        NewlyCompleted    // Found, and we just marked it completed now
    }

    public static TaskMarkResult MarkTaskCompleted<T>(
        List<T> taskList,
        object taskId,
        Func<T, object> getId,
        Func<T, bool> getCompleted,
        Action<T, bool> setCompleted)
    {
        foreach (var task in taskList)
        {
            if (Equals(getId(task), taskId))
            {
                if (getCompleted(task))
                    return TaskMarkResult.AlreadyCompleted;

                setCompleted(task, true);
                return TaskMarkResult.NewlyCompleted;
            }
        }

        return TaskMarkResult.NotFound;
    }


}
