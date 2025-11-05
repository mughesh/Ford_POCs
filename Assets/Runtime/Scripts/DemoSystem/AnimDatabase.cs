using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimDatabase", menuName = "Tasks/Anim Database", order = 51)]
public class AnimDatabase : ScriptableObject
{
    public List<string> AnimIDs = new();
}
