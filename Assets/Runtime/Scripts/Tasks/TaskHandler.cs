public class TaskHandler : Task
{
    public override void TriggerComplete()
    {
        CompleteTask();
    }
}
