using UnityEngine;
using Autohand;
using System.Collections;

/// <summary>
/// Switch-based knob interaction - Grab and release to rotate knob to target angle
/// No physics-based rotation tracking - just simple grab/release toggle
/// Alternative to KnobRotationTask for easier hand tracking interaction
/// </summary>
public class KnobSwitchTask : Task
{
    [Header("Knob Settings")]
    [SerializeField] private Transform knobTransform; // The knob object to rotate
    [SerializeField] private Grabbable knobGrabbable; // The grabbable component

    [Header("Rotation Settings")]
    [Tooltip("Target angle to rotate to when released")]
    [SerializeField] private float targetAngle = 90f;
    [Tooltip("Axis to rotate around (in local space)")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [Tooltip("How fast the knob rotates (higher = faster)")]
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Optional - Lock Knob During Animation")]
    [Tooltip("Disable grabbable during rotation animation")]
    [SerializeField] private bool lockDuringAnimation = true;

    [Header("Optional - Physics")]
    [Tooltip("If knob has HingeJoint, make it kinematic during animation")]
    [SerializeField] private HingeJoint hingeJoint;
    [Tooltip("If knob has Rigidbody, make it kinematic during animation")]
    [SerializeField] private Rigidbody knobRigidbody;

    [Header("What to Unlock (Optional)")]
    [SerializeField] private GameObject[] objectsToUnlock;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip rotationSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Guidance (Optional)")]
    [SerializeField] private GameObject[] hideOnGrab;

    private bool taskCompleted = false;
    private bool isTaskActive = false;
    private bool isRotating = false;
    private bool knobGrabbed = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to task events
        TaskEvents.OnTaskActive += OnTaskActive;

        // Subscribe to knob grab/release
        if (knobGrabbable != null)
        {
            knobGrabbable.OnGrabEvent += OnKnobGrabbed;
            knobGrabbable.OnReleaseEvent += OnKnobReleased;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from task events
        TaskEvents.OnTaskActive -= OnTaskActive;

        // Unsubscribe
        if (knobGrabbable != null)
        {
            knobGrabbable.OnGrabEvent -= OnKnobGrabbed;
            knobGrabbable.OnReleaseEvent -= OnKnobReleased;
        }
    }

    private void OnTaskActive(TaskID activeTaskID)
    {
        isTaskActive = (this.TaskID == activeTaskID);
    }

    private void OnKnobGrabbed(Hand hand, Grabbable grabbable)
    {
        if (!isTaskActive || taskCompleted || isRotating) return;

        knobGrabbed = true;

        Debug.Log("Knob grabbed!");

        // Hide guidance
        HideGuidance();
    }

    private void OnKnobReleased(Hand hand, Grabbable grabbable)
    {
        if (!isTaskActive || taskCompleted || isRotating) return;

        if (knobGrabbed)
        {
            Debug.Log("Knob released - rotating to target angle!");

            // Start rotation animation
            StartCoroutine(RotateToTargetAngle());
        }

        knobGrabbed = false;
    }

    private IEnumerator RotateToTargetAngle()
    {
        isRotating = true;

        // Lock knob during animation
        if (lockDuringAnimation && knobGrabbable != null)
        {
            knobGrabbable.ForceHandsRelease();
            knobGrabbable.enabled = false;
        }

        // Make rigidbody kinematic during animation
        bool wasKinematic = false;
        if (knobRigidbody != null)
        {
            wasKinematic = knobRigidbody.isKinematic;
            knobRigidbody.isKinematic = true;
        }

        // Play rotation sound
        if (audioSource != null && rotationSound != null)
        {
            audioSource.PlayOneShot(rotationSound);
        }

        // Calculate start and target rotations
        Quaternion startRotation = knobTransform.localRotation;
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, rotationAxis) * Quaternion.identity;

        // Animate rotation
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            float t = rotationCurve.Evaluate(Mathf.Clamp01(elapsedTime));

            knobTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        // Ensure final rotation is exact
        knobTransform.localRotation = targetRotation;

        Debug.Log($"Knob rotated to target angle: {targetAngle}°");

        // Restore rigidbody state
        if (knobRigidbody != null)
        {
            knobRigidbody.isKinematic = wasKinematic;
        }

        // Unlock grabbable
        if (lockDuringAnimation && knobGrabbable != null)
        {
            knobGrabbable.enabled = true;
        }

        isRotating = false;

        // Unlock objects
        UnlockObjects();

        // Complete task
        taskCompleted = true;
        CompleteTask();
    }

    private void UnlockObjects()
    {
        foreach (var obj in objectsToUnlock)
        {
            if (obj != null)
            {
                Debug.Log($"Unlocked: {obj.name}");
                // Add specific unlock logic here if needed
            }
        }
    }

    private void HideGuidance()
    {
        foreach (var obj in hideOnGrab)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"Hidden guidance: {obj.name}");
            }
        }
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
