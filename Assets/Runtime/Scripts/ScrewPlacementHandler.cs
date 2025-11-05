using Autohand;
using UnityEngine;

public class ScrewPlacementHandler : Task
{
    [SerializeField] PlacePoint placePoint;
    Screw screw;
    public override void OnEnable()
    {
        base.OnEnable();
        placePoint.OnPlace.AddListener(Placed);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        placePoint.OnPlace.RemoveListener(Placed);
    }
    private void Placed(PlacePoint arg0, Grabbable arg1)
    {
        screw = arg1.GetComponent<Screw>();
        if (screw == null) return;
        screw.OnTightened += TaskCompleted;
    }

    private void TaskCompleted()
    {
        screw.OnTightened -= TaskCompleted;
        CompleteTask();
    }

    public override void TriggerComplete()
    {
    }
}
