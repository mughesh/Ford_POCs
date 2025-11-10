using Autohand;
using System;
using UnityEngine;
public class Screw : MonoBehaviour
{
    [SerializeField] TaskID tool_TightenID;
    [SerializeField] Grabbable Grabbable;
    [SerializeField] HingeJoint joint;
    [SerializeField] Transform parentTo;

    [Header("Poses")]
    [SerializeField] GrabbablePose pickPose;
    [SerializeField] GrabbablePose rotatingPose;
    [SerializeField] PlacePoint ignorepoint;
    [SerializeField] Transform screwPlace;
    [SerializeField] InteractablePosReseter posReseter;


    public Vector2 rotationLimits;
    public int rotatecount;
    public float tolerance = 1f;
    public bool isHandTightened;
    public bool isToolTightened;

    private int currentcount;
    private Rigidbody rb;
    private JointLimits limits;

    private float totalRotation;
    private float lastAngle;

    private bool atMin, atMax;
    private bool isPlaced;
    private bool isGrabbed;
    private bool isRotated;
    private float normalized;
    public event Action OnPlacedEvent;
    public event Action OnTightened;
    public event Action OnToolTightened;
    public void OnEnable()
    {
        Grabbable.OnPlacePointAddEvent += OnPlaced;
        Grabbable.onGrab.AddListener(Grabbed);
        Grabbable.onRelease.AddListener(IncreaseGrabCount);
        rb = GetComponent<Rigidbody>();
    }
    public void OnDisable()
    {
        Grabbable.OnPlacePointAddEvent -= OnPlaced;
        Grabbable.onGrab.RemoveListener(Grabbed);
        Grabbable.onRelease.RemoveListener(IncreaseGrabCount);
    }
    private void Grabbed(Hand arg0, Grabbable arg1)
    {
        isGrabbed = true;
        if (!isPlaced || !isGrabbed) return;
        SetJoint(rotationLimits);
        TaskEvents.UpdateProgressBar(normalized);
        TaskEvents.UpdateProgressBarTransform(transform, true);
    }

    private void IncreaseGrabCount(Hand arg0, Grabbable arg1)
    {
        isGrabbed = false;
        lastAngle = 0f;
        totalRotation = 0f;
        if (isPlaced)
            SetJoint(Vector2.zero);
        TaskEvents.UpdateProgressBarTransform(transform, false);
        if (!isPlaced || !isRotated) return;
        //if (!isPlaced) return;
        currentcount++;
        isRotated = false;
        if (currentcount >= rotatecount)
        {
            TaskEvents.UpdateProgressBar(1);
            transform.localPosition = new Vector3(-0.01f, 0, 0);
            Grabbable.enabled = false;
            pickPose.poseEnabled = false;
            rotatingPose.poseEnabled = false;
            Destroy(joint);
            Destroy(GetComponent<Rigidbody>());
            transform.SetParent(parentTo);
            isHandTightened = true;
            OnTightened?.Invoke();
            rb = null;
            joint = null;
            TaskEvents.UpdateProgressBarTransform(transform, false);
            TaskEvents.UpdateProgressBar(0);
        }
    }


    private void OnPlaced(PlacePoint point, Grabbable grabbable)
    {
        if (isPlaced || point == ignorepoint) return;
        if (point.placeRadius > 0.01)
        {
            SetJoint(Vector2.zero);
            grabbable.handType = HandType.both;
            posReseter.disableReset = true;
            pickPose.poseEnabled = false;
            rotatingPose.poseEnabled = true;
            isPlaced = true;
            OnPlacedEvent?.Invoke();
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }

    }
    private void SetJoint(Vector2 Jointlimits)
    {
        if (joint == null)
            joint = gameObject.AddComponent<HingeJoint>();
        joint.enablePreprocessing = false;
        joint.anchor = Vector3.zero;
        joint.useLimits = true;
        limits = joint.limits;
        limits.min = Jointlimits.x;
        limits.max = Jointlimits.y;
        limits.bounceMinVelocity = 0f;
        joint.limits = limits;
    }

    private void Update()
    {
        if (!isPlaced || !isGrabbed || joint == null)
            return;


        if (rb.angularVelocity.magnitude > tolerance)
        {
            normalized = ((float)currentcount + 1) / rotatecount;
            TaskEvents.UpdateProgressBar(normalized);
            lastAngle = 0f;
            totalRotation = 0f;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            isRotated = true;
        }


            float angle = Mathf.Abs(joint.angle);
        //Debug.Log("Angle: " + angle);
        if (angle > 100f)
            angle -= 90f;

        // Debug.Log("JointAngle: " + angle);

        float delta = Math.Abs(angle - lastAngle);
        lastAngle = angle;

        totalRotation += delta;

        float targetDegrees = currentcount * 75f;
        //float normalized = Mathf.Clamp01(totalRotation / targetDegrees);
        //Debug.Log("Currentcount" + currentcount);
        /*if (totalRotation > tolerance)
        {
            normalized = ((float)currentcount + 1) / rotatecount;
            TaskEvents.UpdateProgressBar(normalized);
            lastAngle = 0f;
            totalRotation = 0f;
            isRotated = true;
        }*/

        /*if (progressBar)
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, normalized, Time.deltaTime * 10f);*/

        // Detect near min limit
        bool nowAtMin = Mathf.Abs(angle - limits.min) <= tolerance;
        bool nowAtMax = Mathf.Abs(angle - (limits.max / 2f)) <= tolerance;

        // Trigger events or actions when crossing state
        if (nowAtMin && !atMin)
        {
            atMin = true;
            atMax = false;
            OnReachedMin();
        }
        else if (nowAtMax && !atMax)
        {
            atMax = true;
            atMin = false;
            OnReachedMax();
        }
        else if (!nowAtMin && !nowAtMax)
        {
            // Reset flags when moving away from limits
            atMin = atMax = false;
        }
    }

    private void OnReachedMax()
    {
        //isRotated = true;
    }

    private void OnReachedMin()
    {

    }

    public void TightenByTool()
    {
        isToolTightened = true;
        OnToolTightened?.Invoke();
        //TaskEvents.TaskCompleted(tool_TightenID);
    }
}
