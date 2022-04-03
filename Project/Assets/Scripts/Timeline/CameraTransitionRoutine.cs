using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionRoutine : MonoBehaviour
{
    public Transform targetCameraPosition;
    public float duration = 1;
    public float targetSize = 1;

    void Start()
    {
        GetComponentInParent<RoutineSequence>().RegisterRoutine(TransitionCoroutine(), 0);
    }

    private IEnumerator TransitionCoroutine()
    {
        Vector3 startCameraPosition = Camera.main.transform.position;
        float startOrthographicSize = Camera.main.orthographicSize;
        for(float time=0; time<duration; time += Time.deltaTime)
        {
            float animRatio = (1 - (1 - time) * (1 - time) );
            Camera.main.transform.position = Vector3.Lerp(startCameraPosition, targetCameraPosition.position, animRatio);
            Camera.main.orthographicSize = Mathf.Lerp(startOrthographicSize, targetSize, animRatio);
            yield return null;
        }
    }
}
