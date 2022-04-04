using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowserMovement : MonoBehaviour
{
    public LevelGenerator level;
    private AnimatedSprite animatedSprite;
    public int startCheckpoint;
    public float movementSpeed = 1;
    public BuildingConfig castleConfig;
    public int furthestCastleSection;
    public int furthestCastleCheckpoint;
    private bool newCastleBuilt;
    void Start()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        level.castleBuiltDelegate += OnCastleBuilt;
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        yield return null;
        Vector3 previousPosition = level.generatedTiles[0].path.GetChild(startCheckpoint).position;
        for(int sectionIndex = 0; sectionIndex <level.generatedTiles.Count; sectionIndex++)
        {
            Transform checkpointContainer = level.generatedTiles[sectionIndex].path;
            for(int i=(sectionIndex == 0 ? startCheckpoint : 1); i<checkpointContainer.childCount; i++)
            {
                Transform nextCheckpoint = checkpointContainer.GetChild(i);
                Vector3 nextPosition = nextCheckpoint.position;
                yield return StepMovementCoroutine(previousPosition, nextPosition);
                ImprovementUI improvementUI = nextCheckpoint.GetComponentInChildren<ImprovementUI>();
                if(improvementUI != null && improvementUI.currentConfig == castleConfig && (sectionIndex > furthestCastleSection || (sectionIndex == furthestCastleSection && i >= furthestCastleCheckpoint)))
                {
                    while(!newCastleBuilt)
                        yield return null;
                    newCastleBuilt = false;
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

    public void OnCastleBuilt(int sectionIndex, int checkpointIndex)
    {
        if(sectionIndex > furthestCastleSection || (sectionIndex == furthestCastleSection && checkpointIndex > furthestCastleCheckpoint))
        {
            newCastleBuilt = true;
            furthestCastleSection = sectionIndex;
            furthestCastleCheckpoint = checkpointIndex;
        }
    }
}
