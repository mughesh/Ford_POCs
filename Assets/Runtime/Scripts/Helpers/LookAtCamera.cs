using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class LookAtCamera : MonoBehaviour

{
    [SerializeField] Camera mainCam;
    [SerializeField]  bool isActive;
    [SerializeField] bool clamp;
    [SerializeField] bool3 constraints;

    private void Start()
    {
        mainCam = null;
        if(!mainCam)
        {
            mainCam = Camera.main;
        }
    }
    private void Update()
    {
         if(isActive)
        {
            transform.LookAt(mainCam.transform);
        }
         if(clamp)
        {
            Vector3 lookPos = mainCam.transform.position - transform.position;
            lookPos = new Vector3(
                constraints.x ? 0 : lookPos.x,
                constraints.y ? 0 : lookPos.y,
                constraints.z ? 0 : lookPos.z
                );
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;

        }
    }
}
