using TMPro;
using UnityEngine;
public class Timer : MonoBehaviour
{
    [SerializeField] float timeRemainimg;
    bool timerRunning = false;
    [SerializeField] TextMeshProUGUI timertext;
    int currentTime;
    public  TimeHolder time;
    private void OnEnable()
    {
        timeRemainimg = time.time;
    }
    public void StartTimer()
    {
        timerRunning = true;
    }
    public void StopTimer()
    {
        timerRunning = false;
        //timertext.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (timerRunning)
        {
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        timeRemainimg += Time.deltaTime;
        time.time = timeRemainimg;
        UpdateTimeText();
    }
    void UpdateTimeText()
    {
        if (timertext != null)
        {
            int min = Mathf.FloorToInt(timeRemainimg / 60f);
            int secs = Mathf.FloorToInt(timeRemainimg % 60);
            timertext.text = $"Time: {min:00}:{secs:00}";
        }
    }
    public string GetTime()
    {
        return timertext.text;
    }
}
