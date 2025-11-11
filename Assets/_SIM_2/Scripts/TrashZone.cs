using UnityEngine;
using Autohand;

[RequireComponent(typeof(Collider))]
public class TrashZone : MonoBehaviour
{
    [SerializeField] private string targetTag = "Ticket";
    [SerializeField] private TicketRemovalTask ticketTask; // Reference to the task

    private void Awake()
    {
        // Ensure this has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the ticket
        if (other.CompareTag(targetTag))
        {
            DiscardTicket(other.gameObject);
        }

        // Also check if it's a grabbable with the right tag
        Grabbable grabbable = other.GetComponent<Grabbable>();
        if (grabbable != null && grabbable.CompareTag(targetTag))
        {
            DiscardTicket(grabbable.gameObject);
        }
    }

    private void DiscardTicket(GameObject ticket)
    {
        Debug.Log("Ticket discarded!");

        // IMPORTANT: Force release from hand first to avoid stuck hand
        Grabbable grabbable = ticket.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            grabbable.ForceHandsRelease();
            grabbable.enabled = false;
        }

        // Make ticket child of trash
        ticket.transform.SetParent(transform);

        // Disable both ticket and trash
        ticket.SetActive(false);
        gameObject.SetActive(false);

        // Notify the task to complete
        if (ticketTask != null)
        {
            ticketTask.OnTicketDiscarded();
        }
    }
}
