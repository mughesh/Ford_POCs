using UnityEngine;

namespace FordSimulation2
{
    public class SimulationController : MonoBehaviour
    {
        [Header("Step Configuration")]
        [SerializeField] private StepBase[] steps;

        [Header("UI References")]
        [SerializeField] private GameObject instructionPanel;
        [SerializeField] private TMPro.TextMeshProUGUI instructionText;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip stepCompleteSound;

        private int currentStepIndex = -1;

        public static SimulationController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // Initialize all steps
            foreach (var step in steps)
            {
                if (step != null)
                {
                    step.Initialize(this);
                }
            }

            // Start first step
            StartNextStep();
        }

        public void CompleteCurrentStep()
        {
            if (currentStepIndex >= 0 && currentStepIndex < steps.Length)
            {
                steps[currentStepIndex].OnStepExit();

                // Play completion sound
                if (audioSource != null && stepCompleteSound != null)
                {
                    audioSource.PlayOneShot(stepCompleteSound);
                }
            }

            StartNextStep();
        }

        private void StartNextStep()
        {
            currentStepIndex++;

            if (currentStepIndex >= steps.Length)
            {
                OnSimulationComplete();
                return;
            }

            var currentStep = steps[currentStepIndex];
            if (currentStep != null)
            {
                // Update instruction text
                if (instructionText != null)
                {
                    instructionText.text = currentStep.InstructionText;
                }

                // Enter the step
                currentStep.OnStepEnter();
            }
        }

        public void UpdateInstructionText(string text)
        {
            if (instructionText != null)
            {
                instructionText.text = text;
            }
        }

        private void OnSimulationComplete()
        {
            Debug.Log("Simulation Complete!");
            // TODO: Show completion UI
        }

        public int GetCurrentStepIndex()
        {
            return currentStepIndex;
        }
    }
}
