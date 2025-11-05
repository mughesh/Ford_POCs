using UnityEngine;
public class Attachment : MonoBehaviour
{
    public void LockTransform(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.localEulerAngles = rot;
    }
}