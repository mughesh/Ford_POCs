using STCHSUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PassthroughToggle : STCLiveUI
{
    [SerializeField] ARCameraManager passthroughLayer;
    [SerializeField] List<GameObject> envObjs;
    [SerializeField] List<MeshRenderer> envObjsRenderers;
    [SerializeField] bool disableByDefault;
    [SerializeField] Image mrIcon;
    [SerializeField] Sprite mrOnIcon;
    [SerializeField] Sprite mrOffIcon;
    public Settings settings;
    public static event Action OnDisableMR;
    public static void DisableMR()
    {
        OnDisableMR?.Invoke();
    }
    public void SetPass()
    {
        passthroughLayer.enabled = !passthroughLayer.enabled;
        settings.isMrOn = passthroughLayer.enabled;
        envObjs.ForEach(obj => obj.SetActive(!settings.isMrOn));
        envObjsRenderers.ForEach(obj => obj.enabled = !settings.isMrOn);
        mrIcon.sprite = passthroughLayer.enabled ? mrOnIcon : mrOffIcon;
        //if(!settings.isMrOn )
        //LevelEvents.ChangeHand(LevelManager.Instance.levelObj.currentHand);
    }

    public void HandleDisableMR()
    {
        passthroughLayer.enabled = false;
        envObjs.ForEach(obj => obj.SetActive(true));
        envObjsRenderers.ForEach(obj => obj.enabled = true);
        settings.isMrOn = passthroughLayer.enabled;
        mrIcon.sprite = passthroughLayer.enabled ? mrOnIcon : mrOffIcon;
    }

    public void OnEnable()
    {
        OnDisableMR += HandleDisableMR;

        if (disableByDefault)
            return;

        if (settings.isMrOn)
            SetPass();

        mrIcon.sprite = passthroughLayer.enabled ? mrOnIcon : mrOffIcon;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        OnDisableMR -= HandleDisableMR;
    }
    public override void IncomingFromDataChannel(string _Message)
    {
        if(_Message.Equals("Toggle"))
        {
            SetPass();
        }
    }
}