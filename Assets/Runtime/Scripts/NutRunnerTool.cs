using Autohand;
using UnityEngine;

public class NutRunnerTool : MonoBehaviour
{
    [SerializeField] Grabbable grabbable;
    [SerializeField] ScrewTriggerEvent triggerEvent;
    [SerializeField] Transform tip;
    [SerializeField] TaskID nutrunnerFunc, screwTighened, placement;
    [SerializeField] PlacePoint placePoint;
    public enum Axis { X, Y, Z }
    [SerializeField] private Axis ignoreAxis = Axis.Y;

    [Header("Joint Settings")]
    public float breakForce = 450f;
    [Header("SFX")]
    [SerializeField] AudioSource AudioSource;

    private Screw currentScrew;
    public ConfigurableJoint conJoint;

    private bool isGrabbed;
    private bool isPressed;
    private bool isAttached;
    private bool forceStop;
    private bool alldone;

    private float screwingTime;
    private Rigidbody rb;
    private void OnEnable()
    {
        grabbable.onGrab.AddListener(OnGrabbed);
        grabbable.onRelease.AddListener(OnReleased);
        placePoint.OnPlace.AddListener(OnPlaced);
        triggerEvent.OnTriggerEnterEvent += TriggerEvent_OnTriggerEnterEvent;
        TaskEvents.OnTaskCompleted += TaskEvents_OnTaskCompleted;
    }

    private void OnPlaced(PlacePoint arg0, Grabbable arg1)
    {
        if (alldone)
            TaskEvents.TaskCompleted(placement);
    }

    private void TaskEvents_OnTaskCompleted(TaskID obj)
    {
        if (obj == screwTighened)
            alldone = true;
    }

    private void OnDisable()
    {
        grabbable.onGrab.RemoveListener(OnGrabbed);
        grabbable.onRelease.RemoveListener(OnReleased);
        placePoint.OnPlace.RemoveListener(OnPlaced);
        triggerEvent.OnTriggerEnterEvent -= TriggerEvent_OnTriggerEnterEvent;
        TaskEvents.OnTaskCompleted -= TaskEvents_OnTaskCompleted;
    }

    private void TriggerEvent_OnTriggerEnterEvent(Screw obj)
    {
        currentScrew = obj;
        if (currentScrew.isToolTightened || !currentScrew.isHandTightened)
        {
            currentScrew = null;
            return;
        }
        SetJoint();
    }

    private void OnGrabbed(Hand arg0, Grabbable arg1) => isGrabbed = true;
    private void OnReleased(Hand arg0, Grabbable arg1)
    {
        isGrabbed = false;
        isPressed = false;
        if (conJoint != null)
            Destroy(conJoint);
        conJoint = null;
    }

    private void SetJoint()
    {
        if (currentScrew == null) return;
        if (conJoint != null) Destroy(conJoint);
        conJoint = null;
        //TaskEvents.TaskCompleted(nutrunnerFunc);
        //transform.position = currentScrew.transform.position;

        /*Vector3 targetEuler = currentScrew.transform.rotation.eulerAngles;
        Vector3 currentEuler = transform.rotation.eulerAngles;

        switch (ignoreAxis)
        {
            case Axis.X: targetEuler.x = currentEuler.x; break;
            case Axis.Y: targetEuler.y = currentEuler.y; break;
            case Axis.Z: targetEuler.z = currentEuler.z; break;
        }*/
        conJoint = gameObject.AddComponent<ConfigurableJoint>();
        conJoint.breakForce = breakForce;


        conJoint.xMotion = ConfigurableJointMotion.Locked;
        conJoint.yMotion = ConfigurableJointMotion.Locked;
        conJoint.zMotion = ConfigurableJointMotion.Locked;
        conJoint.angularXMotion = ConfigurableJointMotion.Locked;

        conJoint.autoConfigureConnectedAnchor = false;
        conJoint.connectedAnchor = currentScrew.transform.position;
        //conJoint.angularYMotion = ConfigurableJointMotion.Locked;
        //conJoint.angularZMotion = ConfigurableJointMotion.Locked;

        //transform.rotation = Quaternion.Euler(targetEuler);
        forceStop = false;
        isAttached = true;
    }
    private void Update()
    {
        if (conJoint == null)
        {
            isAttached = false;
            forceStop = false;
        }

        if (isGrabbed)
        {
            foreach (var item in grabbable.GetHeldBy())
            {
                if (!item.left)
                    AnimateTip(item);
            }
        }

        if (isAttached && isPressed && !currentScrew.isToolTightened && grabbable.GetHeldBy().Count > 1)
        {
            if (!AudioSource.isPlaying)
                AudioSource.Play();

            tip.Rotate(Vector3.right, 10f);

            screwingTime += Time.deltaTime;
            if (screwingTime > 2)
            {
                forceStop = true;
                currentScrew.TightenByTool();
                screwingTime = 0;
                if (conJoint != null)
                    conJoint.breakForce = 450f;
            }
        }
        else
        {
            if (AudioSource.isPlaying)
                AudioSource.Stop();
        }
    }
    private void AnimateTip(Hand item)
    {
        float squezze = item.GetSqueezeAxis();
        if (squezze > 0.1f)
            isPressed = true;
        else
            isPressed = false;
    }
}
