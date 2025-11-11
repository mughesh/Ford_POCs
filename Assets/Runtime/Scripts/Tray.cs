using Autohand;
using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [SerializeField] Grabbable Grabbable;
    [SerializeField] GameObject parentedTrayleft, parentedTrayRight;
    [SerializeField] Renderer _renderer;
    [SerializeField] List<GameObject> forEnable, forDisable;
    [SerializeField] List<PlacePoint> placePointsleft, placePointsright;
    private void OnEnable()
    {
        Grabbable.onHighlight.AddListener(ParentToHand);
    }
    private void OnDisable()
    {
        Grabbable.onHighlight.RemoveListener(ParentToHand);
    }
    private void ParentToHand(Hand arg0, Grabbable arg1)
    {
        if (!_renderer.enabled) return;
        _renderer.enabled = false;

        if (arg0.left)
        {
            placePointsleft.ForEach(x => x.enabled = true);
            parentedTrayleft.SetActive(true);
        }
        else
        {
            placePointsright.ForEach(x => x.enabled = true);
            parentedTrayRight.SetActive(true);
        }

        forEnable.ForEach(x => x.SetActive(true));
        forDisable.ForEach(x => x.SetActive(false));
    }
}
