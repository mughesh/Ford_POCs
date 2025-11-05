using Autohand;
using Unity.VisualScripting;
using UnityEngine;
public class Attacher : MonoBehaviour
{
    [SerializeField] PlacePoint placePoint;

    [Header("Joint Settings")]
    public Vector3 axis;
    public float limit = float.MaxValue;
    public float breakforce;
    public ConfigurableJointMotion xMotion;
    public ConfigurableJointMotion yMotion;
    public ConfigurableJointMotion zMotion;
    public ConfigurableJointMotion angularXMotion;
    public ConfigurableJointMotion angularYMotion;
    public ConfigurableJointMotion angularZMotion;

    ConfigurableJoint joint;
    Grabbable grabbable;
    private void OnEnable()
    {
        placePoint.OnPlace.AddListener(DetectedPlace);
    }
    private void OnDisable()
    {
        placePoint.OnPlace.RemoveListener(DetectedPlace);
    }
    private void DetectedPlace(PlacePoint arg0, Grabbable arg1)
    {
        grabbable = arg1;
        grabbable.onRelease.AddListener(EnableGravity);

        joint = grabbable.AddComponent<ConfigurableJoint>();
        joint.xMotion = xMotion;
        joint.yMotion = yMotion;
        joint.zMotion = zMotion;
        joint.angularXMotion = angularXMotion;
        joint.angularYMotion = angularYMotion;
        joint.angularZMotion = angularZMotion;
        joint.breakForce = breakforce;
    }

    public void EnableGravity(Hand arg0, Grabbable arg1)
    {
        Destroy(joint);
        grabbable.onRelease.RemoveListener(EnableGravity);
    }
}