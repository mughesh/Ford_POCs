using Autohand;
using RopeToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wireharness : MonoBehaviour
{
    [SerializeField] Rope rope;
    [SerializeField] Rope bottomRope;
    [SerializeField] Rope bottomRope_Arranged;
    [Header("Rope")]
    [SerializeField] RopeParams topFlow_1;
    [SerializeField] RopeParams topFlow_2;
    [SerializeField] List<RopeParams> bottomFlow;
    [SerializeField] RopeParams plugHole;
    [SerializeField] RopeParams plugConnecter;

    [Header("PressPoints")]
    [SerializeField] PhysicsGadgetButton top_Camera;
    [SerializeField] PhysicsGadgetButton top_1;
    [SerializeField] PhysicsGadgetButton top_2;
    [SerializeField] PhysicsGadgetButton bottom_1;
    [SerializeField] PhysicsGadgetButton bottom_2;

    [Header("PlugConnection")]
    [SerializeField] PlacePoint plugpoint;
    [SerializeField] PhysicsGadgetButton plugButton;
    [SerializeField] GameObject mainPlug, pushPlug;

    [Header("Tasks")]
    [SerializeField] TaskID plugConnectedTask, plugPushedTask;
    [SerializeField] TaskID seatWireTask;
    [SerializeField] TaskID pushTopPointsTask_1, pushTopPointsTask_2;
    [SerializeField] TaskID pushBottomPointsTask_1, pushBottomPointsTask_2;
    [SerializeField] TaskID closeFrunk;

    [SerializeField] bool preConnect;
    private void OnEnable()
    {
        if (preConnect)
        {
            ConnectTopFlow();
            //ConnectBottomFlow();
            return;
        }
        plugpoint.OnPlace.AddListener(OnPlaced);
        plugButton.OnPressed.AddListener(OnPlugPressed);
        top_Camera.OnPressed.AddListener(OnCameraPressed);
        top_1.OnPressed.AddListener(OnPressedTop_1);
        top_2.OnPressed.AddListener(OnPressedTop_2);
        bottom_1.OnPressed.AddListener(OnPressedBottom_1);
        bottom_2.OnPressed.AddListener(OnPressedBottom_2);
        //TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
        
    }

    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        if (obj != closeFrunk) return;
        rope.simulation.enabled = false;
        bottomRope.simulation.enabled = false;
        bottomRope_Arranged.simulation.enabled = false;
    }

    private void OnDisable()
    {
        if (preConnect) return;
        plugpoint.OnPlace.RemoveListener(OnPlaced);
        plugButton.OnPressed.RemoveListener(OnPlugPressed);
        top_Camera.OnPressed.RemoveListener(OnCameraPressed);
        top_1.OnPressed.RemoveListener(OnPressedTop_1);
        top_2.OnPressed.RemoveListener(OnPressedTop_2);
        bottom_1.OnPressed.RemoveListener(OnPressedBottom_1);
        bottom_2.OnPressed.RemoveListener(OnPressedBottom_2);
    }
    private void OnCameraPressed() => TaskEvents.TaskCompleted(seatWireTask);
    private void OnPressedBottom_2() { TaskEvents.TaskCompleted(pushBottomPointsTask_2); }
    private void OnPressedBottom_1() { TaskEvents.TaskCompleted(pushBottomPointsTask_1); }
    private void OnPressedTop_1() { TaskEvents.TaskCompleted(pushTopPointsTask_1); }
    private void OnPressedTop_2() { TaskEvents.TaskCompleted(pushTopPointsTask_2); }


    private void OnPlugPressed()
    {
        TaskEvents.TaskCompleted(plugPushedTask);
        ConnectBottomFlow();
    }

    private void OnPlaced(PlacePoint arg0, Grabbable arg1)
    {
        mainPlug.SetActive(false);
        pushPlug.SetActive(true);
        TaskEvents.TaskCompleted(plugConnectedTask);

    }

    public void ConnectTopFlow()
    {
        ConnectRope(topFlow_1, rope);
        ConnectRope(topFlow_2, rope);
        ConnectRope(plugHole, rope);

    }
    public void ConnectBottomFlow()
    {
        bottomRope.gameObject.SetActive(false);
        bottomRope_Arranged.gameObject.SetActive(true);
        /*foreach (var item in bottomFlow)
        {
            ConnectRope(item, bottomRope);
        }*/

    }
    private void ConnectRope(RopeParams connection, Rope _rope)
    {
        var newConnection = _rope.gameObject.AddComponent<RopeConnection>();
        newConnection.type = RopeConnectionType.PinRopeToTransform;
        newConnection.ropeLocation = connection.ropeLocation;
        newConnection.rigidbodySettings.body = connection.transform.GetComponent<Rigidbody>();
        newConnection.rigidbodySettings.stiffness = 1f;
        newConnection.transformSettings.transform = connection.transform;
        //rope.transformSettings.transform = target;
    }
}
[Serializable]
public class RopeParams
{
    public Transform transform;
    [Range(0.0f, 1.0f)] public float ropeLocation;
}