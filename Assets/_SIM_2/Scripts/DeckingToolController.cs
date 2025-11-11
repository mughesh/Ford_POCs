using UnityEngine;
using System.Collections;
using Autohand;

/// <summary>
/// Reusable controller for the IP Decking Tool movements
/// Handles position/rotation lerping with smooth transitions
/// </summary>
public class DeckingToolController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0.5f; // Lower = slower, smoother
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Lock Settings")]
    [SerializeField] private Rigidbody toolRigidbody;
    private RigidbodyConstraints originalConstraints;
    private bool isLocked = true;

    private bool isMoving = false;

    private void Start()
    {
        if (toolRigidbody == null)
        {
            toolRigidbody = GetComponent<Rigidbody>();
        }

        if (toolRigidbody != null)
        {
            originalConstraints = toolRigidbody.constraints;
            LockTool();
        }
    }

    public void LockTool()
    {
        if (toolRigidbody != null)
        {
            toolRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            isLocked = true;
            Debug.Log("Decking Tool LOCKED");
        }
    }

    public void UnlockTool()
    {
        if (toolRigidbody != null)
        {
            toolRigidbody.constraints = originalConstraints;
            isLocked = false;
            Debug.Log("Decking Tool UNLOCKED");
        }
    }

    public bool IsLocked => isLocked;
    public bool IsMoving => isMoving;

    /// <summary>
    /// Move tool to target position/rotation smoothly
    /// </summary>
    public void MoveToTarget(Transform target, System.Action onComplete = null)
    {
        if (isMoving)
        {
            Debug.LogWarning("Tool is already moving!");
            return;
        }

        StartCoroutine(MoveToTargetCoroutine(target, onComplete));
    }

    private IEnumerator MoveToTargetCoroutine(Transform target, System.Action onComplete)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 targetPos = target.position;
        Quaternion targetRot = target.rotation;

        float elapsedTime = 0f;

        // Unlock if locked
        bool wasLocked = isLocked;
        if (wasLocked)
        {
            UnlockTool();
        }

        // Make rigidbody kinematic during movement
        if (toolRigidbody != null)
        {
            toolRigidbody.isKinematic = true;
        }

        // Smoothly lerp to target
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            float t = movementCurve.Evaluate(Mathf.Clamp01(elapsedTime));

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // Ensure exact final position
        transform.position = targetPos;
        transform.rotation = targetRot;

        // Restore rigidbody
        if (toolRigidbody != null)
        {
            toolRigidbody.isKinematic = false;
        }

        // Lock again if it was locked
        if (wasLocked)
        {
            LockTool();
        }

        isMoving = false;

        Debug.Log($"Tool reached target: {target.name}");

        onComplete?.Invoke();
    }

    /// <summary>
    /// Check if any hand is holding the handle
    /// </summary>
    public bool IsHandGrabbing()
    {
        // Find all grabbables in children
        Grabbable[] grabbables = GetComponentsInChildren<Grabbable>();

        foreach (var grabbable in grabbables)
        {
            if (grabbable.IsHeld())
            {
                return true;
            }
        }

        return false;
    }
}
