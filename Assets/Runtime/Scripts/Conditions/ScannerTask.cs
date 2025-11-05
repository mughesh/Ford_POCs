using UnityEngine;

public class ScannerTask : Task
{
    bool isScanned = false;
    [SerializeField] AudioSource AudioSource;

    public override void TriggerComplete()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HighMount") && !isScanned)
        {
            isScanned = true;
            CompleteTask();
            AudioSource.Play();
        }
    }
}
