using System.Collections;
using UnityEngine;

public class ColliderOnEnable : MonoBehaviour
{
    [SerializeField] MonoBehaviour mono;
    [SerializeField] private Collider m_Collider;
    Coroutine Coroutine;
    private void OnEnable()
    {
        m_Collider = GetComponent<Collider>();
        if (Coroutine != null) StopCoroutine(Coroutine);
        Coroutine = StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f);
        m_Collider.enabled = true;
        if(mono != null) mono.enabled = true;
    }
}
