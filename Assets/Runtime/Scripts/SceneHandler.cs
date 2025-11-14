using STCHSUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : STCLiveUI
{
    [SerializeField] bool reload;
    [SerializeField] bool exit;
    [SerializeField] TimeHolder timeHolder;
    private void Awake()
    {
        if (timeHolder != null)
            timeHolder.time = 0f;
    }
    public void WhenHover() { }

    public void WhenSelect()
    {
        if (reload)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            timeHolder.time = 0f;
        }
        if (exit)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void WhenUnHover() { }

    public void WhenUnSelect() { }
    public override void IncomingFromDataChannel(string _Message)
    {
        if (_Message.Equals("Reset"))
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
