using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowserMovement : MonoBehaviour
{
    public LevelGenerator level;
    private AnimatedSprite animatedSprite;
    public int startCheckpoint;
    public float movementSpeed = 1;
    public float maxMovementSpeed = 2;
    public float increaseDuration = 60;
    private float time = 0;
    public BuildingConfig castleConfig;
    public int furthestCastleSection;
    public int furthestCastleCheckpoint;
    private bool newCastleBuilt;
    public Transform gameOverPrefab;
    public static MowserMovement instance;
    public GameObject bubblePrefab;
    public DifficultyConfig difficulty;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animatedSprite = GetComponent<AnimatedSprite>();
        level.castleBuiltDelegate += OnCastleBuilt;
        level.castleDestroyedDelegate += OnCastleDestroyed;
        StartCoroutine(MovementCoroutine());
    }

    void Update()
    {
        time += Time.deltaTime;
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
                    float fadeDuration = 0.5f;
                    for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                    {
                        transform.localScale = Vector3.one * (1 - t/fadeDuration);
                        yield return null;
                    }
                    GameObject bubble = Instantiate(bubblePrefab, nextCheckpoint.transform);
                    for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                    {
                        bubble.transform.localScale = bubblePrefab.transform.localScale * (t/fadeDuration);
                        yield return null;
                    }
                    while(!newCastleBuilt)
                        yield return null;
                    newCastleBuilt = false;
                    for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                    {
                        bubble.transform.localScale = bubblePrefab.transform.localScale * (1 - t/fadeDuration);
                        yield return null;
                    }
                    for(float t=0; t<fadeDuration; t+=Time.deltaTime)
                    {
                        transform.localScale = Vector3.one * (t/fadeDuration);
                        yield return null;
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

    public void OnCastleBuilt(int sectionIndex, int checkpointIndex)
    {
        if(sectionIndex > furthestCastleSection || (sectionIndex == furthestCastleSection && checkpointIndex > furthestCastleCheckpoint))
        {
            newCastleBuilt = true;
            furthestCastleSection = sectionIndex;
            furthestCastleCheckpoint = checkpointIndex;
        }
    }

    public void OnCastleDestroyed(int sectionIndex, int checkpointIndex)
    {
        if(sectionIndex > furthestCastleSection || (sectionIndex == furthestCastleSection && checkpointIndex > furthestCastleCheckpoint))
        {
            Debug.Log("Last Castled Destroyed");
            Instantiate(gameOverPrefab);
        }
    }
}
