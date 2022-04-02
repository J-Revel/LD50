using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitRoutine : MonoBehaviour
{
    public int priority = 0;
    public float waitDuration = 1;

    void Start()
    {
        GetComponentInParent<RoutineSequence>().RegisterRoutine(WaitCoroutine(), priority);
    }

    // Update is called once per frame
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(waitDuration);
    }
}
