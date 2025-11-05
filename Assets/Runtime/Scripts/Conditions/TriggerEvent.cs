using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] string _tag;
    public UnityEvent OnTriggerEntered;
    public UnityEvent OnTriggerExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tag))
            OnTriggerEntered?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_tag))
            OnTriggerExited?.Invoke();
    }
}
