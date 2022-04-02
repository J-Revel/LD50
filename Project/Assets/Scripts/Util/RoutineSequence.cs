using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate IEnumerator CoroutineDelegate();

[System.Serializable]
public class PrioritizedRoutine
{
    public int priority;
    public IEnumerator routine;
}
public class RoutineSequence : MonoBehaviour
{
    public bool playAtStart = false;
    private bool played = false;
    public List<PrioritizedRoutine> registeredRoutines;

    public void RegisterRoutine(IEnumerator routine, int priority)
    {
        PrioritizedRoutine toAdd = new PrioritizedRoutine();
        toAdd.routine = routine;
        toAdd.priority = priority;
        registeredRoutines.Add(toAdd);
    }

    public void LaunchTransition()
    {
        played = true;
        registeredRoutines.Sort((PrioritizedRoutine a, PrioritizedRoutine b) => { return a.priority - b.priority; });
        StartCoroutine(MainCoroutine());
    }

    private void Update()
    {
        if(!played && playAtStart)
        {
            LaunchTransition();
        }
    }

    private IEnumerator MainCoroutine()
    {
        for(int i=0; i<registeredRoutines.Count; i++)
        {
            yield return registeredRoutines[i].routine;
        }
    }
}
