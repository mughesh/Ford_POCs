using UnityEngine;

/// <summary>
/// Sets a HingeJoint to a specific angle at start
/// Useful when you want the object to start at min/max angle instead of default
/// </summary>
public class HingeJointInitializer : MonoBehaviour
{
    [Header("Initial Position")]
    [Tooltip("Set to Min to start at minimum angle, Max for maximum angle")]
    public InitialPosition initialPosition = InitialPosition.Min;

    [Header("Optional - Manual Angle")]
    [Tooltip("If you want a specific angle, enable this and set the angle")]
    public bool useCustomAngle = false;
    [Range(-180f, 180f)]
    public float customAngle = 0f;

    private HingeJoint hingeJoint;

    public enum InitialPosition
    {
        Min,
        Max,
        Custom
    }

    private void Start()
    {
        hingeJoint = GetComponent<HingeJoint>();

        if (hingeJoint == null)
        {
            Debug.LogError($"HingeJointInitializer: No HingeJoint found on {gameObject.name}!");
            return;
        }

        SetInitialPosition();
    }

    private void SetInitialPosition()
    {
        float targetAngle = 0f;

        if (useCustomAngle)
        {
            targetAngle = customAngle;
        }
        else
        {
            switch (initialPosition)
            {
                case InitialPosition.Min:
                    targetAngle = hingeJoint.limits.min;
                    break;
                case InitialPosition.Max:
                    targetAngle = hingeJoint.limits.max;
                    break;
            }
        }

        // Set the rotation
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, hingeJoint.axis);
        transform.localRotation = targetRotation;

        // Wake up rigidbody if present
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.WakeUp();
        }

        Debug.Log($"HingeJoint on {gameObject.name} initialized to angle: {targetAngle}°");
    }

    // Call this to reset to initial position at runtime
    public void ResetToInitialPosition()
    {
        SetInitialPosition();
    }
}
