using UnityEngine;

/// <summary>
/// Sets a knob transform to a specific rotation angle at start
/// For use with KnobSwitchTask (non-physics based rotation)
/// Similar to HingeJointInitializer but simpler - just sets transform rotation
/// </summary>
public class KnobSwitchInitializer : MonoBehaviour
{
    [Header("Initial Rotation")]
    [Tooltip("Initial angle to set the knob to")]
    [SerializeField] private float initialAngle = 0f;

    [Tooltip("Axis to rotate around (in local space)")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    [Header("Optional - Store Original")]
    [Tooltip("Store the original rotation to reset later")]
    [SerializeField] private bool storeOriginalRotation = true;

    private Quaternion originalRotation;

    private void Start()
    {
        if (storeOriginalRotation)
        {
            originalRotation = transform.localRotation;
        }

        SetInitialRotation();
    }

    private void SetInitialRotation()
    {
        // Calculate target rotation
        Quaternion targetRotation = Quaternion.AngleAxis(initialAngle, rotationAxis);
        transform.localRotation = targetRotation;

        Debug.Log($"Knob '{gameObject.name}' initialized to angle: {initialAngle}° around axis {rotationAxis}");
    }

    /// <summary>
    /// Call this to reset to initial rotation at runtime
    /// </summary>
    public void ResetToInitialRotation()
    {
        SetInitialRotation();
    }

    /// <summary>
    /// Call this to reset to original rotation (before initialization)
    /// </summary>
    public void ResetToOriginalRotation()
    {
        if (storeOriginalRotation)
        {
            transform.localRotation = originalRotation;
            Debug.Log($"Knob '{gameObject.name}' reset to original rotation");
        }
        else
        {
            Debug.LogWarning($"KnobSwitchInitializer on '{gameObject.name}': Original rotation not stored!");
        }
    }

    /// <summary>
    /// Set knob to specific angle at runtime
    /// </summary>
    public void SetAngle(float angle)
    {
        Quaternion targetRotation = Quaternion.AngleAxis(angle, rotationAxis);
        transform.localRotation = targetRotation;
        Debug.Log($"Knob '{gameObject.name}' set to angle: {angle}°");
    }
}
