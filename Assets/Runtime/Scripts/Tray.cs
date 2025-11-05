using Autohand;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] Grabbable Grabbable;
    [SerializeField] GameObject parentedTray;
    [SerializeField] Renderer _renderer;
    [SerializeField] List<GameObject> forEnable, forDisable;
    private void OnEnable()
    {
        Grabbable.onGrab.AddListener(ParentToHand);
    }
    private void OnDisable()
    {
        Grabbable.onGrab.RemoveListener(ParentToHand);
    }
    private void ParentToHand(Hand arg0, Grabbable arg1)
    {
        //gameObject.SetActive(false);
        _renderer.enabled = false;

        forEnable.ForEach(x => x.SetActive(true));
        forDisable.ForEach(x => x.SetActive(false));
        parentedTray.SetActive(true);

    }
}
