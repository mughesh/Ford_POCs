using Autohand;
using System;
using UnityEngine;

public class HoodStructTask : MonoBehaviour
{
    [SerializeField] TaskID grabTask, placeStruct, top_Task, bottom_Task, plugStruct, pushStruct;

    [SerializeField] GameObject grabStruct, topConnection, bottomConnection, plug, connectPlug, pushPlug;
    [SerializeField] Grabbable structGrabbable;
    [SerializeField] PlacePoint structPlacepoint, plugplacePoint;
    [SerializeField] PhysicsGadgetButton triggerEvent, bottomConnectionButton;
    [SerializeField] PhysicsGadgetButton pushPlugbutton;

    private void OnEnable()
    {
        structPlacepoint.OnPlace.AddListener(StructPlaced);
        plugplacePoint.OnPlace.AddListener(PlugStruct);
        pushPlugbutton.OnPressed.AddListener(PushPlug);
        triggerEvent.OnPressed.AddListener(TopConnection);
        bottomConnectionButton.OnPressed.AddListener(BottomConnection);
        structGrabbable.onGrab.AddListener(StructGrabbed);
    }
    private void OnDisable()
    {
        structPlacepoint.OnPlace.RemoveListener(StructPlaced);
        plugplacePoint.OnPlace.RemoveListener(PlugStruct);
        pushPlugbutton.OnPressed.RemoveListener(PushPlug);
        triggerEvent.OnPressed.RemoveListener(TopConnection);
        bottomConnectionButton.OnPressed.RemoveListener(BottomConnection);
        structGrabbable.onGrab.RemoveListener(StructGrabbed);
    }

    private void BottomConnection()
    {
        CompleteTask(bottom_Task);
        bottomConnection.SetActive(false);
        topConnection.SetActive(true);
    }

    public void StructGrabbed(Hand arg0, Grabbable arg1) => CompleteTask(grabTask);
    public void StructPlaced(PlacePoint arg0, Grabbable arg1)
    {
        CompleteTask(placeStruct);
        grabStruct.SetActive(false);
        bottomConnection.SetActive(true);
    }
    public void TopConnection()
    {
        CompleteTask(top_Task);
        topConnection.SetActive(false);
        plug.SetActive(true);
    }

    public void PlugStruct(PlacePoint arg0, Grabbable arg1)
    {
        CompleteTask(plugStruct);
        connectPlug.SetActive(false);
        pushPlug.SetActive(true);
    }
    public void PushPlug() => CompleteTask(pushStruct);
    private void CompleteTask(TaskID id)
    {
        TaskEvents.TaskCompleted(id);
    }

}
