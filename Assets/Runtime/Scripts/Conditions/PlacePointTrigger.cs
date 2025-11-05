using Autohand;
using UnityEngine;

public class PlacePointTrigger : MonoBehaviour
{
    [SerializeField] PlacePoint placePoint;
    [SerializeField] string _tag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tag))
            placePoint.enabled = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_tag))
            placePoint.enabled = false;
    }
}
