using UnityEngine;
using UnityEngine.UI;

namespace FordSimulation2
{
    public class TeleportButton : MonoBehaviour
    {
        [Header("Teleport Target")]
        [SerializeField] private Transform teleportTarget;
        [SerializeField] private Transform playerTransform;

        [Header("UI")]
        [SerializeField] private Button teleportButton;
        [SerializeField] private GameObject buttonUI;

        [Header("Auto-hide")]
        [SerializeField] private bool hideAfterTeleport = true;

        private void Start()
        {
            if (teleportButton != null)
            {
                teleportButton.onClick.AddListener(OnTeleportButtonClicked);
            }

            // Hide initially
            if (buttonUI != null)
            {
                buttonUI.SetActive(false);
            }
        }

        public void ShowTeleportButton()
        {
            if (buttonUI != null)
            {
                buttonUI.SetActive(true);
            }
        }

        public void HideTeleportButton()
        {
            if (buttonUI != null)
            {
                buttonUI.SetActive(false);
            }
        }

        private void OnTeleportButtonClicked()
        {
            if (teleportTarget == null || playerTransform == null)
            {
                Debug.LogWarning("TeleportButton: Missing teleport target or player transform!");
                return;
            }

            // Teleport player
            playerTransform.position = teleportTarget.position;
            playerTransform.rotation = teleportTarget.rotation;

            Debug.Log($"Player teleported to: {teleportTarget.name}");

            // Hide button after teleport
            if (hideAfterTeleport)
            {
                HideTeleportButton();
            }

            // Complete current step if this is part of simulation flow
            if (SimulationController.Instance != null)
            {
                SimulationController.Instance.CompleteCurrentStep();
            }
        }

        private void OnDestroy()
        {
            if (teleportButton != null)
            {
                teleportButton.onClick.RemoveListener(OnTeleportButtonClicked);
            }
        }
    }
}
