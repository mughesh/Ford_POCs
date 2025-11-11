using UnityEngine;
using Autohand;

/// <summary>
/// Task 1: Remove the Tele-auto ticket and discard it in the trash
/// </summary>
public class TicketRemovalTask : Task
{
    [Header("References")]
    [SerializeField] private Grabbable ticketGrabbable;
    [SerializeField] private GameObject trashIcon;
    [SerializeField] private AudioClip tearingSound;

    private AudioSource audioSource;
    private bool ticketGrabbed = false;

    public override void OnEnable()
    {
        base.OnEnable();

        // Subscribe to ticket grab event
        if (ticketGrabbable != null)
        {
            ticketGrabbable.OnGrabEvent += OnTicketGrabbed;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // Unsubscribe from events
        if (ticketGrabbable != null)
        {
            ticketGrabbable.OnGrabEvent -= OnTicketGrabbed;
        }
    }

    private void Start()
    {
        // Get or add audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Make sure trash is disabled initially
        if (trashIcon != null)
        {
            trashIcon.SetActive(false);
        }
    }

    private void OnTicketGrabbed(Hand hand, Grabbable grabbable)
    {
        if (ticketGrabbed) return;

        ticketGrabbed = true;

        // Play tearing sound
        if (audioSource != null && tearingSound != null)
        {
            audioSource.PlayOneShot(tearingSound);
        }

        // Enable trash icon
        if (trashIcon != null)
        {
            trashIcon.SetActive(true);
        }

        Debug.Log("Ticket grabbed! Trash icon enabled.");
    }

    // Called by TrashZone when ticket is discarded
    public void OnTicketDiscarded()
    {
        Debug.Log("Ticket discarded! Completing task...");
        CompleteTask(); // This fires TaskEvents and moves to next task
    }

    public override void TriggerComplete()
    {
        // For debug - you can check this in inspector to force complete
        CompleteTask();
    }
}
