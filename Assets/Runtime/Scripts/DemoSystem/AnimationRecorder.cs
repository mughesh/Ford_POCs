#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEngine;
public class AnimationRecorder : MonoBehaviour
{
    private GameObject targetToRecord;

    internal GameObjectRecorder recorder;
    private bool isRecording = false;

    public bool IsRecording => isRecording;
    private void Awake() => targetToRecord = gameObject;
    private void Start()
    {
        Recorder.Instance.RegisterRecorder(this);
    }
    private void OnEnable()
    {
        Recorder.OnRecording += Recorder_OnRecording;
    }
    private void OnDisable()
    {
        Recorder.OnRecording -= Recorder_OnRecording;
    }
    private void Recorder_OnRecording(bool obj)
    {
        if (obj)
            StartRecording();
        else
            StopRecording();
    }

    public void StartRecording()
    {
        if (!Application.isPlaying || targetToRecord == null || isRecording)
        {
            Debug.LogWarning("Recording not started. Check Play Mode and target.");
            return;
        }

        recorder = new GameObjectRecorder(targetToRecord);
        recorder.BindComponentsOfType<Transform>(targetToRecord, true);
        isRecording = true;

        Debug.Log("Recording started for " + targetToRecord.name);
    }

    public void StopRecording()
    {
        if (!Application.isPlaying || !isRecording || recorder == null)
        {
            Debug.LogWarning("Recording not stopped properly.");
            return;
        }

        isRecording = false;
    }

    private void Update()
    {
        if (isRecording && recorder != null)
        {
            recorder.TakeSnapshot(Time.deltaTime);
        }
    }
}
#endif