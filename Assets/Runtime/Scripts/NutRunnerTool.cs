using Autohand;
using System.Collections;
using UnityEngine;

public class NutRunnerTool : MonoBehaviour
{
    [SerializeField] Grabbable grabbable;
    [SerializeField] PlacePoint placePoint;
    [SerializeField] Transform cylinder;
    [SerializeField] Attacher attachment;
    public bool isPressing;
    public bool isPlaced;
    public bool isTightened;
    public float tightenedTime;
    public float timeToTighten;
    Vector3 startPos, startRot;
    Hand hand;
    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.localEulerAngles;
    }
    private void OnEnable()
    {
        grabbable.onSqueeze.AddListener(OnSqueeze);
        grabbable.onRelease.AddListener(OnRelease);
        grabbable.onUnsqueeze.AddListener(OnUnSqueeze);
        grabbable.OnPlacePointAddEvent += OnPlaced;
    }


    private void OnDisable()
    {
        grabbable.onSqueeze.RemoveListener(OnSqueeze);
        grabbable.onRelease.RemoveListener(OnRelease);
        grabbable.onUnsqueeze.RemoveListener(OnUnSqueeze);
        grabbable.OnPlacePointAddEvent -= OnPlaced;
    }
    private void OnRelease(Hand arg0, Grabbable arg1)
    {
        isPlaced = false;
    }
    private void OnPlaced(PlacePoint arg0, Grabbable arg1)
    {
        placePoint = arg0;
        isPlaced = true;
    }
    private void OnSqueeze(Hand arg0, Grabbable arg1)
    {
        isPressing = true;
    }
    private void OnUnSqueeze(Hand arg0, Grabbable arg1)
    {
        isPressing = false;
    }
    private void Update()
    {
        if (isPressing)
        {
            cylinder.Rotate(Vector3.up, 10f);
            if (isPlaced)
                tightenedTime += Time.deltaTime;
            if (tightenedTime >= timeToTighten)
            {
                if (placePoint != null)
                    placePoint.Remove(grabbable);
                isPlaced = false;
                tightenedTime = 0;
                attachment.EnableGravity(null, null);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            StartCoroutine(GotoStart());
        }
    }
    IEnumerator GotoStart()
    {
        yield return new WaitForSeconds(2);
        transform.position = startPos;
        transform.localEulerAngles = startRot;
    }
}
