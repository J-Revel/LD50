using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator CoroutineDelegate();
public class SceneTransition : MonoBehaviour
{
    public CoroutineDelegate launchDelegate;
    void Start()
    {
        
    }

    public void LaunchTransition()
    {
        List<IEnumerator> invocationList = new List<IEnumerator>();
        foreach(IEnumerator routine in launchDelegate.GetInvocationList())
            StartCoroutine(routine);
    }

    private IEnumerator SceneTransitionCoroutine(System.Delegate[] invocationList)
    {
        for(int i=0; i<invocationList.Length; i++)
        {
            yield return ((CoroutineDelegate)invocationList[i])?.Invoke();
        }
    }
}
