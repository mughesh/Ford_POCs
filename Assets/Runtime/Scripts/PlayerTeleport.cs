using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] TaskID task;
    [SerializeField] Transform player;
    private XRInputSubsystem xrInput;
    private void OnEnable()
    {
        TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
        StartCoroutine(HandleRecentring());
    }
    private void OnDisable()
    {
        TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
    }
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (obj == task)
        {
            player.SetPositionAndRotation(transform.position, transform.rotation);
            StartCoroutine(HandleRecentring());
        }
    }
    public void DoTeleport()
    {
        player.SetPositionAndRotation(transform.position, transform.rotation);
        StartCoroutine(HandleRecentring());
    }
    private IEnumerator HandleRecentring()
    {
        Debug.Log("Attempting automated recentre.");

        if (xrInput == null)
        {
            List<XRInputSubsystem> _Inputs = new();
            SubsystemManager.GetSubsystems(_Inputs);

            if (_Inputs.Count > 0)
                xrInput = _Inputs[0];
        }
        if (xrInput != null)
        {
            xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
            yield return null;

            // Trigger recenter
            xrInput.TryRecenter();
            yield return null;

            // (Optional) Switch back to Floor/Stage
            xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
            yield return null;
        }
    }

}
