using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LookAt : MonoBehaviour
{
    public Transform Target;
    [SerializeField]
    Transform cam;
    [SerializeField]
    GuideArrowAxis guideArrowAxis;
    [SerializeField]
    float crossProduct;
    [SerializeField]
    float dotProduct;
    [SerializeField]
    float angle;

    [SerializeField]
    Image ArrowImage;
    [SerializeField]
    TextMeshProUGUI arrowText;
    private void Awake()
    {
        if (cam == null)
            cam = Camera.main.transform;
    }

    private void OnEnable()
    {
        TaskEvents.OnLookAt += SolisEvents_OnLookAt;
        TaskEvents.OnStopLookAtArrow += SolisEvents_OnStopLookAtArrow;
    }

    private void OnDisable()
    {
        TaskEvents.OnLookAt -= SolisEvents_OnLookAt;
        TaskEvents.OnStopLookAtArrow -= SolisEvents_OnStopLookAtArrow;
    }
    private void SolisEvents_OnStopLookAtArrow()
    {
        ArrowImage.gameObject.SetActive(false);
    }
    private void SolisEvents_OnLookAt(Transform obj,GuideArrowAxis guideArrow)
    {
        ArrowImage.gameObject.SetActive(true);
        Target = obj;
        guideArrowAxis = guideArrow;
    }

    void Update()
    {
        try
        {
            if (Target == null)
                return;
            switch (guideArrowAxis)
            {
                case GuideArrowAxis.X:
                    crossProduct = Vector3.Cross(cam.forward, Target.right).magnitude;
                    dotProduct = Vector3.Dot(cam.forward, Target.right);
                    angle = Vector3.SignedAngle(cam.forward, Target.right, Vector3.up);
                    break;
                case GuideArrowAxis.NegX:
                    crossProduct = Vector3.Cross(cam.forward, -1 * Target.right).magnitude;
                    dotProduct = Vector3.Dot(cam.forward, -1 * Target.right);
                    angle = Vector3.SignedAngle(cam.forward, -1 * Target.right, Vector3.up);
                    break;
                case GuideArrowAxis.Z:
                    crossProduct = Vector3.Cross(cam.forward, Target.forward).magnitude;
                    dotProduct = Vector3.Dot(cam.forward, Target.forward);
                    angle = Vector3.SignedAngle(cam.forward, Target.forward, Vector3.up);
                    break;
                case GuideArrowAxis.NegZ:
                    crossProduct = Vector3.Cross(cam.forward, -1 * Target.forward).magnitude;
                    dotProduct = Vector3.Dot(cam.forward, -1 * Target.forward);
                    angle = Vector3.SignedAngle(cam.forward, -1 * Target.forward, Vector3.up);
                    break;
            }
            //crossProduct = Vector3.Cross(Camera.forward, Target.forward).magnitude;
            //dotProduct = Vector3.Dot(Camera.forward, Target.forward);
            //angle = Vector3.SignedAngle(Camera.forward, Target.forward, Vector3.up);

            if (Target != null && angle > 0)
            {
                ArrowImage.transform.localEulerAngles = new Vector3(ArrowImage.transform.localEulerAngles.x, ArrowImage.transform.localEulerAngles.y, 180);
                arrowText.transform.localEulerAngles = new Vector3(arrowText.transform.localEulerAngles.x, arrowText.transform.localEulerAngles.y, 180);
            }
            else
            {
                ArrowImage.transform.localEulerAngles = new Vector3(ArrowImage.transform.localEulerAngles.x, ArrowImage.transform.localEulerAngles.y, 0);
                arrowText.transform.localEulerAngles = new Vector3(arrowText.transform.localEulerAngles.x, arrowText.transform.localEulerAngles.y, 0);
            }

            if (angle < -120 || angle > 110 || !Target.gameObject.activeSelf)
            {
                ArrowImage.enabled = false;
                arrowText.enabled = false;
            }
            else if (Target.gameObject.transform.parent != null && Target.gameObject.transform.parent.gameObject.activeSelf == false)
            {
                ArrowImage.enabled = false;
                arrowText.enabled = false;
            }
            else
            {
                ArrowImage.enabled = true;
                arrowText.enabled = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Exception in GuideArrow.cs -> Update() : " + e.Message);
        }
    }

}
public enum GuideArrowAxis
{
    Z,
    X,
    Y,

    NegX,
    NegY,
    NegZ
}