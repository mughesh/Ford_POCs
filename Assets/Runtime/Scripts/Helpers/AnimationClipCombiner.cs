#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AnimationClipCombiner
{
    /// <summary>
    /// Combines multiple AnimationClips into one.
    /// Keeps each GameObject's animation path and properties intact.
    /// </summary>
    /// <param name="clips">Array of clips to combine</param>
    /// <param name="combinedName">Optional name for the new combined clip</param>
    /// <returns>A single combined AnimationClip</returns>
    public static AnimationClip CombineClips(List<AnimationClip> clips, string combinedName = "CombinedClip")
    {
        if (clips == null || clips.Count == 0)
        {
            Debug.LogWarning("No clips provided for combination.");
            return null;
        }

        AnimationClip combinedClip = new AnimationClip
        {
            name = combinedName,
            frameRate = clips[0].frameRate // assume same fps
        };

        HashSet<string> addedCurves = new HashSet<string>();

        foreach (AnimationClip clip in clips)
        {
            if (clip == null)
                continue;

            // Get all animated curve bindings (position, rotation, scale, etc.)
            EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(clip);

            foreach (EditorCurveBinding binding in curveBindings)
            {
                AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

                // Create a unique identifier for each path + property
                string key = $"{binding.path}_{binding.type}_{binding.propertyName}";

                if (!addedCurves.Contains(key))
                {
                    AnimationUtility.SetEditorCurve(combinedClip, binding, curve);
                    addedCurves.Add(key);
                }
                else
                {
                    Debug.LogWarning($"Duplicate curve skipped: {binding.path}/{binding.propertyName}");
                }
            }

            // Handle object reference curves (for materials, sprites, etc.)
            var objectBindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            foreach (var objBinding in objectBindings)
            {
                var objCurves = AnimationUtility.GetObjectReferenceCurve(clip, objBinding);
                AnimationUtility.SetObjectReferenceCurve(combinedClip, objBinding, objCurves);
            }
        }

        Debug.Log($" Combined {clips.Count} clips into '{combinedClip.name}'");
        return combinedClip;
    }
}
#endif