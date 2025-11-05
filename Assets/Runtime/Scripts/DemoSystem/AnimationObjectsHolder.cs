using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnimationObjectsHolder", menuName = "Recorder/AnimationObjectsHolder", order = 51)]
public class AnimationObjectsHolder : ScriptableObject
{
    public List<AnimationClip> recordedClips;
}
