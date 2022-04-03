using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRoutine : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent action;
    public int priority;
    void Start()
    {
        GetComponentInParent<RoutineSequence>().RegisterRoutine(ApplyActionCoroutine(), priority);
    }

    IEnumerator ApplyActionCoroutine()
    {
        action.Invoke();
        yield return null;
    }
}
