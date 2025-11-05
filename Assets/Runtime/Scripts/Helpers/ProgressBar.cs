using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject bg;
    [SerializeField] Image progressBar;
    private void Awake()
    {
        TaskEvents.OnProgressUpdate += TaskEvents_OnProgressUpdate;
        TaskEvents.OnProgressUpdateTransform += TaskEvents_OnProgressUpdate;
    }

    private void TaskEvents_OnProgressUpdate(float progress)
    {
        //Debug.Log("Progressbar" + progress);
        progressBar.fillAmount = Mathf.Lerp(0, 1, progress);
    }

    private void OnDestroy()
    {
        TaskEvents.OnProgressUpdate -= TaskEvents_OnProgressUpdate;
        TaskEvents.OnProgressUpdateTransform -= TaskEvents_OnProgressUpdate;
    }
    private void TaskEvents_OnProgressUpdate(Transform target, bool state)
    {
        bg.SetActive(state);
        transform.position = target.position;
        //transform.SetPositionAndRotation(target.position, target.rotation);
        //transform.position += Vector3.up * 0.1f;

    }
}
