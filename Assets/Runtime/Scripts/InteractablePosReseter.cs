using Autohand;
using System.Collections;
using UnityEngine;

public class InteractablePosReseter : MonoBehaviour
{
    [SerializeField] PlacePoint goTo;

    [Header("This is unsafe use only when needed")]
    public bool setLayer;
    [HideInInspector] public bool disableReset;

    Vector3 pos;
    Vector3 rot;
    Rigidbody rb;
    Coroutine coroutine;
    Grabbable grabbable;
    bool isDetecting;
    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        rb = GetComponent<Rigidbody>();
        pos = transform.position;
        rot = transform.localEulerAngles;
    }
    public void UpdatePos()
    {
        pos = transform.position;
        rot = transform.localEulerAngles;
    }

    private void OnEnable() => grabbable.onRelease.AddListener(StartDetecting);
    private void OnDisable() => grabbable.onRelease.RemoveListener(StartDetecting);
    private void StartDetecting(Hand arg0, Grabbable arg1) => isDetecting = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (disableReset) return;

        if (collision.transform.CompareTag("Ground"))
            OnUnGrab();

        if (collision.transform.CompareTag("Table") && isDetecting)
            OnUnGrab();
    }
    public void OnUnGrab()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(DelayReset());
    }
    public void ManualReset()
    {
        if (disableReset) return;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (goTo != null)
        {
            goTo.makePlacedKinematic = true;
            goTo.disableRigidbodyOnPlace = false;
            goTo.Highlight(grabbable);
            goTo.Place(grabbable);
        }
        else
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            transform.position = pos;
            transform.localEulerAngles = rot;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        if (setLayer)
            gameObject.layer = LayerMask.NameToLayer("Grabbable");

        isDetecting = false;
    }
    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(1f);
        ManualReset();
    }
}
