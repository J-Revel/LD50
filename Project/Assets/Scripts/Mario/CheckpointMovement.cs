using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMovement : MonoBehaviour
{
    public Transform checkpointContainer;
    public int currentCheckpoint;
    public float movementSpeed = 1;
    void Start()
    {
        StartCoroutine(MovementCoroutine());
    }



    private IEnumerator MovementCoroutine()
    {
        Vector3 previousPosition = checkpointContainer.GetChild(0).position;
        for(int i=1; i<checkpointContainer.childCount; i++)
        {
            Transform nextCheckpoint = checkpointContainer.GetChild(i);
            Vector3 nextPosition = nextCheckpoint.position;
            yield return StepMovementCoroutine(previousPosition, nextPosition);
            RoutineSequence checkpointSequence = nextCheckpoint.GetComponent<RoutineSequence>();
            if(checkpointSequence != null)
            {
                yield return checkpointSequence.MainCoroutine();
            }
            previousPosition = nextPosition;
        }
    }

    private IEnumerator StepMovementCoroutine(Vector3 currentCheckpointPosition, Vector3 targetCheckpointPosition)
    {
        float duration = Vector3.Distance(currentCheckpointPosition, targetCheckpointPosition) / movementSpeed;
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(currentCheckpointPosition, targetCheckpointPosition, time / duration);
            yield return null;
        }
    }
}
