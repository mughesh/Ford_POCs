using UnityEngine;

namespace FordSimulation2
{
    public class GuidanceArrow : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private bool animate = true;
        [SerializeField] private float bobSpeed = 1f;
        [SerializeField] private float bobAmount = 0.2f;
        [SerializeField] private bool rotateArrow = false;
        [SerializeField] private float rotationSpeed = 50f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.localPosition;
        }

        private void Update()
        {
            if (!animate) return;

            // Bob up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

            // Optional rotation
            if (rotateArrow)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            startPosition = transform.localPosition;
        }
    }
}
