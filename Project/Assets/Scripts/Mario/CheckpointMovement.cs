using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMovement : MonoBehaviour
{
    public LevelGenerator level;
    private AnimatedSprite animatedSprite;
    public int currentCheckpoint;
    public DifficultyConfig difficulty;
    public float movementSpeed = 1;
    public float maxMovementSpeed = 2;
    public float increaseDuration = 60;
    private float time = 0;
    public GameObject bubblePrefab;
    void Start()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        StartCoroutine(MovementCoroutine());
    }

    private void Update()
    {
        time += Time.deltaTime;

    }

    private IEnumerator MovementCoroutine()
    {
        yield return null;
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
                    float fadeDuration = 0.5f;
                    ImprovementUI improvementUI = checkpointSequence.GetComponentInChildren<ImprovementUI>();
                    if(improvementUI != null)
                    {
                        improvementUI.LockBuildingForCombat();
                        if(improvementUI.currentConfig.timelineLength > 0)
                        {
                            for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                            {
                                transform.localScale = Vector3.one * (1 - t/fadeDuration);
                                yield return null;
                            }
                            GameObject bubble = Instantiate(bubblePrefab, nextCheckpoint.transform);
                            if(improvementUI != null)
                            {
                                for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                                {
                                    bubble.transform.localScale = bubblePrefab.transform.localScale * (t/fadeDuration);
                                    yield return null;
                                }
                            }
                            yield return checkpointSequence.MainCoroutine();
                            if(improvementUI != null)
                            {
                                for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                                {
                                    bubble.transform.localScale = bubblePrefab.transform.localScale * (1 - t/fadeDuration);
                                    yield return null;
                                }
                            }
                            Destroy(bubble);
                            for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                            {
                                transform.localScale = Vector3.one * (t/fadeDuration);
                                yield return null;
                            }
                        }
                    }
                }
                previousPosition = nextPosition;
            }
            
        }
    }

    private IEnumerator StepMovementCoroutine(Vector3 currentCheckpointPosition, Vector3 targetCheckpointPosition)
    {
        float duration = Vector3.Distance(currentCheckpointPosition, targetCheckpointPosition) / DifficultyService.instance.movementSpeed;
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
