using System;
using UnityEngine;

public class ScrewTriggerEvent : MonoBehaviour
{
    public event Action<Screw> OnTriggerEnterEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Screw>(out Screw currentScrew))
        {
            OnTriggerEnterEvent?.Invoke(currentScrew);
        }
    }
}
