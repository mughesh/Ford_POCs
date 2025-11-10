using UnityEngine;
using Autohand;

namespace FordSimulation2
{
    public class TicketRemovalStep : StepBase
    {
        [Header("Ticket References")]
        [SerializeField] private Grabbable ticketGrabbable;
        [SerializeField] private GameObject trashIcon;
        [SerializeField] private AudioClip tearingSound;

        private AudioSource audioSource;
        private bool ticketGrabbed = false;

        public override void Initialize(SimulationController simController)
        {
            base.Initialize(simController);

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

            // Subscribe to ticket grab event
            if (ticketGrabbable != null)
            {
                ticketGrabbable.OnGrabEvent += OnTicketGrabbed;
            }
        }

        public override void OnStepEnter()
        {
            base.OnStepEnter();
            ticketGrabbed = false;
        }

        public override void OnStepExit()
        {
            base.OnStepExit();
        }

        private void OnTicketGrabbed(Hand hand, Grabbable grabbable)
        {
            if (!isStepActive || ticketGrabbed) return;

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

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (ticketGrabbable != null)
            {
                ticketGrabbable.OnGrabEvent -= OnTicketGrabbed;
            }
        }
    }
}
