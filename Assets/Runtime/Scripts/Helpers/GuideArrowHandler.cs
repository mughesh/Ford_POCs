using System.Collections;
using UnityEngine;

public class GuideArrowHandler : MonoBehaviour
{
    public float delay;
    public GuideArrowAxis axis;
    Coroutine _coroutine;
   
    public void StartArrow()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(Delay());
    }
    public void StopArrow()
    {
        TaskEvents.StopLookAtArrow();
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        TaskEvents.LootAt(transform,axis);
    }
}
