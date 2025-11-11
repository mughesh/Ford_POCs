using UnityEngine;

namespace FordSimulation2
{
    public abstract class StepBase : MonoBehaviour
    {
        [Header("Step Info")]
        [SerializeField, TextArea(2, 4)] private string instructionText;

        [Header("Guidance")]
        [SerializeField] protected GameObject[] guidanceArrows;
        [SerializeField] protected GameObject[] highlightObjects;

        protected SimulationController controller;
        protected bool isStepActive = false;

        public string InstructionText => instructionText;

        public virtual void Initialize(SimulationController simController)
        {
            controller = simController;

            // Disable all guidance initially
            SetGuidanceActive(false);
        }

        public virtual void OnStepEnter()
        {
            isStepActive = true;
            SetGuidanceActive(true);
            Debug.Log($"Step Started: {instructionText}");
        }

        public virtual void OnStepExit()
        {
            isStepActive = false;
            SetGuidanceActive(false);
            Debug.Log($"Step Completed: {instructionText}");
        }

        protected void CompleteStep()
        {
            if (isStepActive && controller != null)
            {
                controller.CompleteCurrentStep();
            }
        }

        protected virtual void SetGuidanceActive(bool active)
        {
            // Enable/disable arrows
            if (guidanceArrows != null)
            {
                foreach (var arrow in guidanceArrows)
                {
                    if (arrow != null)
                    {
                        arrow.SetActive(active);
                    }
                }
            }

            // Enable/disable highlights
            if (highlightObjects != null)
            {
                foreach (var highlight in highlightObjects)
                {
                    if (highlight != null)
                    {
                        highlight.SetActive(active);
                    }
                }
            }
        }
    }
}
