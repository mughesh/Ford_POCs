#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [SerializeField] AnimationObjectsHolder animHolder;
    [SerializeField] Material animMat;
    public string savePath = "Assets/RecordedAnimations/";
    public TaskID clipName;
    public List<AnimationRecorder> recorderList = new();

    private bool isRecording = false;
    public static Recorder Instance { get; private set; }
    public static event Action<bool> OnRecording;
    public bool IsRecording => isRecording;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
        animHolder.recordedClips.Clear();
       // recorderList.Clear();
    }
    public void StartRecording()
    {
        foreach (var item in recorderList)
        {
            item.transform.SetParent(null);
        }
        OnRecording?.Invoke(true);
        isRecording = true;
    }
    public void StopRecording()
    {
        OnRecording?.Invoke(false);
        isRecording = false;
        SaveAnimClip();
    }
    private void SaveAnimClip()
    {
        foreach (var item in recorderList)
        {
            AnimationClip tempclip = new AnimationClip();
            item.recorder.SaveToClip(tempclip);
            item.recorder = null;

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            string fullPath = Path.Combine(savePath, clipName.ID + "_" + item.gameObject.name + ".anim");
            tempclip.legacy = true;
            tempclip.wrapMode = WrapMode.Loop;

            AssetDatabase.CreateAsset(tempclip, fullPath);
            AssetDatabase.SaveAssets();
            animHolder.recordedClips.Add(tempclip);
        }
    }
    public void DuplicateObjects()
    {
        //recorderList.Clear();
        //recorderList = FindObjectsByType<AnimationRecorder>(FindObjectsSortMode.None).ToList();

        GameObject parent = GameObjectUtils.CreateEmpty(null, clipName.ID);
        foreach (var item in recorderList)
        {
            GameObject dupe = GameObjectUtils.CreateEmpty(item.gameObject, item.gameObject.name + "_Anim");
            dupe.transform.SetParent(parent.transform, false);
            AnimationPlayer dupeAnim = dupe.AddComponent<AnimationPlayer>();
            foreach (var anim in animHolder.recordedClips)
            {
                if (anim.name == clipName.ID + "_" + item.gameObject.name)
                {
                    dupeAnim.Initialize(clipName.ID, anim, animMat);
                }
            }
        }

        EditorUtility.SetDirty(this);
    }
    public void RegisterRecorder(AnimationRecorder animationRecorder)
    {
        if (!recorderList.Contains(animationRecorder))
            recorderList.Add(animationRecorder);
    }
}
[CustomEditor(typeof(Recorder))]
public class RecorderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Recorder recorder = (Recorder)target;

        if (EditorApplication.isPlaying)
        {
            GUILayout.Space(10);

            GUI.backgroundColor = recorder.IsRecording ? Color.red : Color.green;

            if (GUILayout.Button(recorder.IsRecording ? "Stop Recording" : "Start Recording"))
            {
                if (recorder.IsRecording)
                    recorder.StopRecording();
                else
                    recorder.StartRecording();
            }

            GUI.backgroundColor = Color.white;

            GUILayout.Space(5);
            EditorGUILayout.HelpBox("Start/Stop recording.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Enter Play Mode to record.", MessageType.Warning);
            if (GUILayout.Button("Create Animated Objects"))
            {
                recorder.DuplicateObjects();
            }
        }
    }
}
#endif