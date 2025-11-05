using Autohand;
using System.Collections;
using UnityEngine;

public class InteractablePosReseter : MonoBehaviour
{
    Vector3 pos;
    Vector3 rot;
    Rigidbody rb;
    Coroutine coroutine;
    Grabbable grabbable;
    bool isDetecting;
    [SerializeField] PlacePoint goTo;
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
    private void OnEnable()
    {
        grabbable.onRelease.AddListener(StartDetecting);
    }
    private void OnDisable()
    {
        grabbable.onRelease.RemoveListener(StartDetecting);
    }
    private void StartDetecting(Hand arg0, Grabbable arg1)
    {
        isDetecting = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"ColliderCheck:{gameObject.name} Collider with {collision.gameObject.name}");
        if (collision.transform.CompareTag("Ground"))
            OnUnGrab();
        if (collision.transform.CompareTag("Table") && isDetecting)
            OnUnGrab();
       /* if (collision.transform.CompareTag("Table") && goTo != null)
            OnUnGrab();*/
    }
    public void OnUnGrab()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(DelayReset());
    }
    public void ManualReset()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        

        if(goTo!=null)
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
            
       
        isDetecting = false;
    }
    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(1f);
        ManualReset();
    }
}
