using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TaskDefinition
{
    public TaskID TaskID;
    public string Description;
    public List<SubTasks> SubTasks;
    public AudioClip AudioClip;
    public float audioDelay;
    public bool isCompleted;
}
