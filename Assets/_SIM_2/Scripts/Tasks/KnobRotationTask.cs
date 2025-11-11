using UnityEngine;
using Autohand;

/// <summary>
/// Task for rotating a knob to unlock something
/// Completes when the knob reaches target angle
/// </summary>
public class KnobRotationTask : Task
{
    [Header("Knob Settings")]
    [SerializeField] private HingeJoint knob;
    [SerializeField] private float targetAngle = 90f; // Angle needed to complete task
    [SerializeField] private float angleTolerance = 5f; // How close to target angle

    [Header("What to Unlock")]
    [SerializeField] private GameObject[] objectsToUnlock; // What gets unlocked when knob turned

    private bool taskCompleted = false;

    private void Update()
    {
        if (taskCompleted || knob == null) return;

        // Check if knob has reached target angle
        float currentAngle = knob.angle;

        if (Mathf.Abs(currentAngle - targetAngle) <= angleTolerance)
        {
            OnKnobUnlocked();
        }
    }

    private void OnKnobUnlocked()
    {
        taskCompleted = true;

        Debug.Log($"Knob rotated to unlock position! Angle: {knob.angle}°");

        // Unlock objects
        foreach (var obj in objectsToUnlock)
        {
            if (obj != null)
            {
                // You can add specific unlock logic here
                // For now, just log
                Debug.Log($"Unlocked: {obj.name}");
            }
        }

        CompleteTask();
    }

    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
