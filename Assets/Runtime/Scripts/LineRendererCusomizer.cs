using UnityEngine;

public class LineRendererCusomizer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    private void Update()
    {
        lineRenderer.SetPosition(0, pointA.position);
        lineRenderer.SetPosition(1, pointB.position);
    }
}
