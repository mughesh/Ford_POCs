using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Task Database")]
public class TaskDatabase : ScriptableObject
{
    public List<string> TaskIDs = new();
}