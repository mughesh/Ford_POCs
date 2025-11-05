using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationPlayer))]
public class AnimationPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var player = (AnimationPlayer)target;
        serializedObject.Update();

        // Start watching for changes
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("AnimID"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("intervalTime"), true);

        // Clip field
        var newClip = (AnimationClip)EditorGUILayout.ObjectField("Clip", player.clip, typeof(AnimationClip), false);

        if (newClip != player.clip)
        {
            Undo.RecordObject(player, "Change Clip");
            player.clip = newClip;
            EditorUtility.SetDirty(player);
        }

        if (player.clip)
        {
            float clipLength = player.clip.length;
            EditorGUILayout.LabelField($"Clip Length: {clipLength:F2}s");

            // Convert normalized to seconds for editing
            float startSec = player.startTime * clipLength;
            float endSec = player.endTime * clipLength;

            // Editable fields
            float newStartSec = EditorGUILayout.FloatField("Start Time (s)", startSec);
            float newEndSec = EditorGUILayout.FloatField("End Time (s)", endSec);

            // Clamp
            newStartSec = Mathf.Clamp(newStartSec, 0f, clipLength);
            newEndSec = Mathf.Clamp(newEndSec, newStartSec, clipLength);

            // Convert back to normalized
            float newStartNorm = newStartSec / clipLength;
            float newEndNorm = newEndSec / clipLength;

            if (newStartNorm != player.startTime || newEndNorm != player.endTime)
            {
                Undo.RecordObject(player, "Change Animation Range");
                player.startTime = newStartNorm;
                player.endTime = newEndNorm;
                EditorUtility.SetDirty(player);
            }

            // Slider
            EditorGUILayout.MinMaxSlider(
                new GUIContent("Playback Range (normalized)"),
                ref player.startTime, ref player.endTime,
                0f, 1f
            );

            EditorGUILayout.LabelField(
                $"Start: {player.startTime * clipLength:F2}s    End: {player.endTime * clipLength:F2}s"
            );

            GUILayout.Space(10);

            // Play buttons
            if (!Application.isPlaying)
            {
                if (!player.isEditorPlaying)
                {
                    if (GUILayout.Button("▶ Play Range (Editor)"))
                        player.PlayEditorPreview();
                }
                else
                {
                    if (GUILayout.Button("■ Stop"))
                        player.StopEditorPreview();
                }
            }
            else
            {
                if (GUILayout.Button("▶ Play in Play Mode"))
                    player.PlayTrimmed();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Assign an AnimationClip to preview.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();

        // Final catch-all: if any serialized property changed, record undo + dirty
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(player, "Edit Animation Player");
            EditorUtility.SetDirty(player);
        }
    }
}
