using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskDescription : MonoBehaviour
{
    public TaskID taskid;
    public string description;
    private bool isCompleted;

    [SerializeField] Toggle toggle;
    [SerializeField] TextMeshProUGUI text;

    public bool IsCompleted { get => isCompleted; set
        {
            isCompleted = value;
            toggle.isOn = isCompleted;
        }
    }

    public void Initialize(TaskID id, string des, bool isDone)
    {
        this.taskid = id;
        this.description = des;
        this.IsCompleted = isDone;
        this.text.text = des;
        this.toggle.isOn = isDone;
    }
}
