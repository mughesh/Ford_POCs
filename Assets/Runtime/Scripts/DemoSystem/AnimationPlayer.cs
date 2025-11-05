using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UnityEngine.Animation))]
public class AnimationPlayer : MonoBehaviour
{
    public TaskID AnimID;
    public AnimationClip clip;
    public bool playOnStart;
    public float intervalTime;
    [Range(0, 1)] public float startTime = 0f;
    [Range(0, 1)] public float endTime = 1f;

    [HideInInspector] public bool isEditorPlaying = false;
    [HideInInspector] public float currentTime = 0f;

    private UnityEngine.Animation player;
    private Coroutine playingCoroutine;
    private Coroutine waitCoroutine;
    private Material animMat;
    List<Renderer> renderers;
    private void Awake()
    {
        if (Application.isPlaying)
        {
            TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
            TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
            renderers = new List<Renderer>();
            renderers = GetComponentsInChildren<Renderer>().ToList();
            //renderers.ForEach(x => x.sharedMaterial= animMat);
            if (renderers.Count > 0)
                renderers.ForEach(r => r.enabled = false);
        }
    }

    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        if (AnimID == obj)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
            TaskEvents.OnTaskCompleted -= TaskEvents_OnTaskCompleted;
        }
    }
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (obj == AnimID)
            PlayTrimmed();
    }
    private void Start()
    {
        if (playOnStart && Application.isPlaying)
            PlayTrimmed();
    }
    private void OnEnable()
    {
        if (!player)
            player = GetComponent<UnityEngine.Animation>();
    }

    private void OnValidate()
    {
        if (!player)
            player = GetComponent<UnityEngine.Animation>();
        if (endTime < startTime)
            endTime = startTime;
    }

    /// <summary>
    /// Initialize the AnimationPlayer at runtime
    /// </summary>
    public void Initialize(TaskID id, AnimationClip clip, Material mat)
    {
        AnimID = id;
        this.clip = clip;
        if (!player) player = GetComponent<UnityEngine.Animation>();
        player.clip = clip;
        player.playAutomatically = false;
        intervalTime = 2f;
        animMat = mat;
    }

    /// <summary>
    /// Play the animation only between startTime and endTime
    /// </summary>
    public void PlayTrimmed()
    {
        if (renderers.Count > 0)
            renderers.ForEach(r => r.enabled = true);
        if (!clip || !player) return;

        // Stop any previous playback
        if (playingCoroutine != null)
            StopCoroutine(playingCoroutine);

        playingCoroutine = StartCoroutine(PlayTrimmedRoutine());
    }

    private IEnumerator PlayTrimmedRoutine()
    {
        float clipLength = clip.length;
        float startSec = startTime * clipLength;
        float endSec = endTime * clipLength;
        float duration = endSec - startSec;

        player[clip.name].time = startSec;
        player[clip.name].speed = 1f;

        player.Play(clip.name);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            player[clip.name].time = startSec + timer;
            yield return null;
        }
        player.Stop();
        playingCoroutine = null;
        CheckForLoop();
    }

    private void CheckForLoop()
    {
        if (player.clip.wrapMode == WrapMode.Loop)
        {
            if (waitCoroutine != null) StopCoroutine(waitCoroutine);
            waitCoroutine = StartCoroutine(WaitTime());
        }
    }
    IEnumerator WaitTime()
    {
        if (renderers.Count > 0)
            renderers.ForEach(r => r.enabled = false);
        yield return new WaitForSeconds(intervalTime);
        PlayTrimmed();
    }

#if UNITY_EDITOR
    public void PlayEditorPreview()
    {
        if (!clip) return;

        // Start editor animation mode
        AnimationMode.StartAnimationMode();
        isEditorPlaying = true;
        currentTime = 0f;

        EditorApplication.update += EditorUpdate;
    }

    private void EditorUpdate()
    {
        if (!isEditorPlaying || !clip) return;

        float clipLength = clip.length;
        float playLength = (endTime - startTime) * clipLength;
        currentTime += 0.01f; // ~30 FPS throttle

        if (currentTime >= playLength)
        {
            StopEditorPreview();
            return;
        }

        float normalizedTime = startTime + (currentTime / clipLength);
        AnimationMode.SampleAnimationClip(gameObject, clip, normalizedTime * clipLength);
    }

    public void StopEditorPreview()
    {
        isEditorPlaying = false;
        EditorApplication.update -= EditorUpdate;
        AnimationMode.StopAnimationMode();
    }
#endif

}
