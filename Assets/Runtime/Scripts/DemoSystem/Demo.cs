using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] GameObject animationsFolder;
    
    public void ShowMe()
    {
        animationsFolder.SetActive(true);
        TaskEvents.SetArrowState(true);
        TaskEvents.canPlayAudio = true;
    }

    public void TryMe()
    {
        animationsFolder.SetActive(false);
        TaskEvents.SetArrowState(true);
        TaskEvents.canPlayAudio = true;
    }
    public void PerformMe()
    {
        animationsFolder.SetActive(false);
        TaskEvents.SetArrowState(false);
        TaskEvents.canPlayAudio = false;
    }
}
