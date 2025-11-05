using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Tasks/Task Sequence")]
public class TaskSequence : ScriptableObject
{
    public int sequenceNumber;
    public List<TaskDefinition> Tasks = new();
}
