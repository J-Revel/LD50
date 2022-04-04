using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMovement : MonoBehaviour
{
    public LevelGenerator level;
    private AnimatedSprite animatedSprite;
    public int currentCheckpoint;
    public float movementSpeed = 1;
    void Start()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        Vector3 previousPosition = level.generatedTiles[0].path.GetChild(0).position;
        for(int sectionIndex = 0; sectionIndex <level.generatedTiles.Count; sectionIndex++)
        {
            Transform checkpointContainer = level.generatedTiles[sectionIndex].path;
            for(int i=1; i<checkpointContainer.childCount; i++)
            {
                Transform nextCheckpoint = checkpointContainer.GetChild(i);
                Vector3 nextPosition = nextCheckpoint.position;
                yield return StepMovementCoroutine(previousPosition, nextPosition);
                RoutineSequence checkpointSequence = nextCheckpoint.GetComponent<RoutineSequence>();
                if(checkpointSequence != null)
                {
                    animatedSprite.SelectAnim("Idle", true);
                    yield return checkpointSequence.MainCoroutine();
                }
                previousPosition = nextPosition;
            }
            
        }
    }

    private IEnumerator StepMovementCoroutine(Vector3 currentCheckpointPosition, Vector3 targetCheckpointPosition)
    {
        float duration = Vector3.Distance(currentCheckpointPosition, targetCheckpointPosition) / movementSpeed;
        SpriteRenderer spriteRenderer = animatedSprite.spriteRenderer;
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            animatedSprite.SelectAnim("Walk", true);
            spriteRenderer.flipX = (Vector3.Dot(Vector3.right, targetCheckpointPosition - currentCheckpointPosition) < 0);
            transform.position = Vector3.Lerp(currentCheckpointPosition, targetCheckpointPosition, time / duration);
            yield return null;
        }
    }
}
