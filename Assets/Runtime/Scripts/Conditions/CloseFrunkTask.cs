using Autohand;
using NUnit.Framework;
using UnityEngine;

public class CloseFrunkTask : MonoBehaviour
{
    [SerializeField] TaskID taskID;
    [SerializeField] Grabbable grabbable;
    public float tolerance = 1f; // how close to limit counts as "at limit"

    private HingeJoint hinge;
    private JointLimits limits;
    private bool atMin, atMax;
    private Rigidbody rb;
    [Header("Hinge Settings")]
    [SerializeField] Vector2 JointLimits_Half;
    [SerializeField] Vector2 JointLimits_Full;
    [SerializeField] Collider _collider;
    [SerializeField] AudioSource AudioSource;

    private void Awake()
    {
        TaskEvents.OnTaskActive += TaskEvents_OnTaskActive;
        rb = GetComponent<Rigidbody>();
        rb.Sleep();
        rb.useGravity = false;
        rb.mass = 5f;
        SetJoints(JointLimits_Half);
        gameObject.SetActive(false);
    }
    private void OnDestroy() => TaskEvents.OnTaskActive -= TaskEvents_OnTaskActive;
    private void TaskEvents_OnTaskActive(TaskID obj)
    {
        if (obj == taskID)
        {
            SetJoints(JointLimits_Half);
            //_collider.enabled = true;
        }
    }

    void SetJoints(Vector2 closeLimits)
    {
        hinge = gameObject.AddComponent<HingeJoint>();
        hinge.anchor = Vector3.zero;
        hinge.axis = new Vector3(0, 1, 0);
        hinge.useLimits = true;
        limits = hinge.limits;
        limits.min = limits.min = JointLimits_Half.x;
        limits.max = limits.max = JointLimits_Half.y;
        hinge.limits = limits;
        rb.WakeUp();
    }

    void Update()
    {
        if (hinge == null) return;
        float angle = hinge.angle;
        if (angle > 90f)
            angle -= 180f;
        // Detect near min limit
        bool nowAtMin = Mathf.Abs(angle - limits.min) <= tolerance;
        bool nowAtMax = Mathf.Abs(angle - limits.max) <= tolerance;

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

    void OnReachedMin()
    {
        Debug.Log($"{name} reached MIN limit ({limits.min}°)");
    }

    void OnReachedMax()
    {
        Debug.Log($"{name} reached MAX limit ({limits.max}°)");
        
        if (_collider.enabled == false) return;
        TaskEvents.TaskCompleted(taskID);

        if(grabbable.GetHeldBy().Count>0)
            grabbable.GetHeldBy().ForEach(hand=>grabbable.ForceHandRelease(hand));
        SetJoints(JointLimits_Full);
        //transform.localEulerAngles = new Vector3(-90f, -90f, 90f);
        _collider.enabled = false;
        
        AudioSource.Play();
    }

}
