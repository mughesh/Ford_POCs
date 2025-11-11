using Autohand;
using UnityEngine;

public class ScrewPlacementHandler : Task
{
    [SerializeField] PlacePoint placePoint;
    [SerializeField] TaskID toolTightenedID;
    Screw screw;
    [SerializeField] Vector3 finalPos;
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
        screw.OnToolTightened += Screw_OnToolTightened;
    }

    private void Screw_OnToolTightened()
    {
        screw.transform.localPosition = finalPos;
        screw.OnToolTightened -= Screw_OnToolTightened;
        TaskEvents.TaskCompleted(toolTightenedID);
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
