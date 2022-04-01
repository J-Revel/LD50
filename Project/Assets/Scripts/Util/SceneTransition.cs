using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    RoutineSequence routineHandler;
    public int priority = 1000;
    public string sceneName;
    
    void Start()
    {
        routineHandler = GetComponent<RoutineSequence>();
        routineHandler.RegisterRoutine(SceneTransitionCoroutine(), priority);
    }

    void Update()
    {
        
    }

    private IEnumerator SceneTransitionCoroutine()
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

}
