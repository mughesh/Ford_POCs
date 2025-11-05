using System.Collections.Generic;
using UnityEngine;

public class ScrewPlaceDetector : Task
{
    [SerializeField] List<Screw> screws;
    [SerializeField] GameObject tray;
    int screwCount;

    public override void OnEnable()
    {
        base.OnEnable();
        screws.ForEach(x => x.OnPlacedEvent += X_OnPlacedEvent);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        screws.ForEach(x => x.OnPlacedEvent -= X_OnPlacedEvent);
    }

    private void X_OnPlacedEvent()
    {
        screwCount++;
        if (screwCount == screws.Count)
        {
            tray.SetActive(false);
            CompleteTask();
        }
    }

    public override void TriggerComplete() { }
}
