using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct ObstacleSortData
{
    public int obstacleIndex;
    public float position;
}

public class TimelineDisplay : MonoBehaviour
{
    public bool forceDeath = false;
    private TimelineDataHolder dataHolder;
    private List<BubbleWidget> visibleObstacles = new List<BubbleWidget>();
    private List<ObstacleSortData> sortedObstacles = new List<ObstacleSortData>();
    public float resolveDuration = 1;
    public float resolveShakeIntensity = 1;
    public float playerMovementSpeed = 0.2f;
    public float playerRespawnDelay = 2;

    public BubbleWidget playerBubblePrefab;
    public BubbleWidget slotPrefab;
    public BubbleWidget princessBubblePrefab;
    public Transform timelineObstacleContainer;
    public Transform timelineBackground;
    public float sectionSize = 4;
    private float previousTimelineLength = 0;

    public AnimatedSprite bubbleExplosionPrefab;

    public TimelineConfig config { get { return dataHolder != null ? dataHolder.timelineData : null; } }

    public Vector3 playerEjectionVelocity;
    public Vector3 playerEjectionGravity;
    public float playerEjectionRotationSpeed = 360 * 3;
    private BubbleWidget princessBubble;

    Vector3 startPoint { get { return Vector3.left * (sectionSize * config.timelineLength / 2); } }
    Vector3 endPoint { get { return Vector3.right * (sectionSize * config.timelineLength / 2); } }
    
    public void AddObstacle(TimelineObstacle obstacle)
    {
        config.obstacles.Add(obstacle);
    }


    private void Update()
    {
        if(dataHolder == null)
        {
            dataHolder = GetComponentInParent<TimelineDataHolder>();
            if(dataHolder != null)
            {
                Init();
                previousTimelineLength = config.timelineLength;
            }
        }
        else if(config.timelineLength != previousTimelineLength)
        {
            UpdateDisplay();
        }
        previousTimelineLength = 0;
        if(config != null)
            previousTimelineLength = config.timelineLength;

        bool hasPrincess = (dataHolder.buildingUI.sectionIndex == MowserMovement.instance.furthestCastleSection && dataHolder.buildingUI.checkpointIndex == MowserMovement.instance.furthestCastleCheckpoint);
        princessBubble.gameObject.SetActive(hasPrincess);
    }

    private void Init()
    {
        timelineBackground.localScale = new Vector3(sectionSize * config.timelineLength, timelineBackground.localScale.y, timelineBackground.localScale.z);
        if(dataHolder.timelineData.buildingIconPrefab != null)
        {
            Transform buildingIcon = Instantiate(dataHolder.timelineData.buildingIconPrefab, timelineObstacleContainer);
            buildingIcon.transform.localPosition = startPoint;
        }   
        
        for(int i=0; i<config.obstacles.Count; i++)
        {
            Vector3 characterPosition = Vector3.Lerp(startPoint, endPoint, config.obstacles[i].position / config.timelineLength);
            if(config.obstacles[i].obstacleType == ObstacleType.Slot)
            {
                if(slotPrefab != null)
                {
                    BubbleWidget instantiated = Instantiate(slotPrefab, timelineObstacleContainer);
                    instantiated.transform.localPosition = characterPosition;
                    visibleObstacles.Add(instantiated);
                    instantiated.gameObject.AddComponent<TimelineObstacleDataHolder>().data = config.obstacles[i];
                }
                else visibleObstacles.Add(null);
            }
            else
            {
                BubbleWidget instantiated = Instantiate(config.obstacles[i].bubblePrefab, timelineObstacleContainer);
                instantiated.transform.localPosition = characterPosition;
                visibleObstacles.Add(instantiated);
                if(instantiated.iconRenderer != null)
                    instantiated.iconRenderer.sprite = config.obstacles[i].icon;
                instantiated.gameObject.AddComponent<TimelineObstacleDataHolder>().data = config.obstacles[i];
            }
        }
        princessBubble = Instantiate(princessBubblePrefab, timelineObstacleContainer);
        princessBubble.transform.localPosition = endPoint;
    }

    private void UpdateDisplay()
    {
        timelineBackground.localScale = new Vector3(sectionSize * config.timelineLength, timelineBackground.localScale.y, timelineBackground.localScale.z);
        for(int i=0; i<visibleObstacles.Count; i++)
        {
            Vector3 characterPosition = Vector3.Lerp(startPoint, endPoint, config.obstacles[i].position / config.timelineLength);
            
            if(visibleObstacles[i] != null)
            {
                visibleObstacles[i].transform.localPosition = characterPosition;
            }
        }
        for(int i=visibleObstacles.Count; i<config.obstacles.Count; i++)
        {
            Vector3 characterPosition = Vector3.Lerp(startPoint, endPoint, config.obstacles[i].position / config.timelineLength);
            if(config.obstacles[i].obstacleType == ObstacleType.Slot)
            {
                if(slotPrefab != null)
                {
                    BubbleWidget instantiated = Instantiate(slotPrefab, timelineObstacleContainer);
                    instantiated.transform.localPosition = characterPosition;
                    visibleObstacles.Add(instantiated);
                    instantiated.gameObject.AddComponent<TimelineObstacleDataHolder>().data = config.obstacles[i];
                }
                else visibleObstacles.Add(null);
            }
            else
            {
                BubbleWidget instantiated = Instantiate(config.obstacles[i].bubblePrefab, timelineObstacleContainer);
                    instantiated.transform.localPosition = characterPosition;
                visibleObstacles.Add(instantiated);
                if(instantiated.iconRenderer != null)
                    instantiated.iconRenderer.sprite = config.obstacles[i].icon;
                instantiated.gameObject.AddComponent<TimelineObstacleDataHolder>().data = config.obstacles[i];
            }
        }
        princessBubble.transform.localPosition = endPoint;
    }

    private void SortObstacles()
    {
        sortedObstacles.Clear();
        for(int i=0; i<config.obstacles.Count; i++)
        {
            ObstacleSortData sortData = new ObstacleSortData();
            sortData.position = config.obstacles[i].position;
            sortData.obstacleIndex = i;
            sortedObstacles.Add(sortData);
        } 
        sortedObstacles.Sort((ObstacleSortData a, ObstacleSortData b) => { return (int)((a.position - b.position) * 100); });
    }

    public int GetNextObstacleIndex(float position)
    {
        int resultIndex = -1;
        float resultPosition = config.timelineLength;
        for(int i=0; i<config.obstacles.Count; i++)
        {
            if(config.obstacles[i].position < resultPosition && config.obstacles[i].position >= position)
            {
                resultIndex = i;
                resultPosition = config.obstacles[i].position;
            }
        }
        return resultIndex;
    }

    private List<int> GetObstaclesAfter(float position)
    {
        List<int> result = new List<int>();
        for(int i=0; i<config.obstacles.Count; i++)
        {
            if(config.obstacles[i].position > position)
            {
                result.Add(i);
            }
        }
        return result;
    }

    public IEnumerator PlayTimelineCoroutine()
    {
        yield return null; // To make sure start is played
        float position = 0;
        BubbleWidget player = Instantiate(playerBubblePrefab, timelineObstacleContainer);
        player.transform.localPosition = startPoint;
        
        float checkpointPosition = 0;
        
        bool obstaclePassed = false;
        Dictionary<int, int> defeatCount = new Dictionary<int, int>();
        while(position < config.timelineLength)
        {
            int nextObstacleIndex = GetNextObstacleIndex(position);
            position += playerMovementSpeed * DifficultyService.instance.timelineSpeed * Time.deltaTime;
            
            Vector3 playerLocalPosition = Vector3.Lerp(startPoint, endPoint, position / config.timelineLength);
            if(nextObstacleIndex >= 0 && !obstaclePassed && nextObstacleIndex < config.obstacles.Count && position >= config.obstacles[nextObstacleIndex].position)
            {
                position = config.obstacles[nextObstacleIndex].position;
                switch(config.obstacles[nextObstacleIndex].obstacleType)
                {
                    case ObstacleType.Slot:
                        obstaclePassed = true;
                        break;
                    case ObstacleType.Checkpoint:
                        obstaclePassed = true;
                        checkpointPosition = config.obstacles[nextObstacleIndex].position;
                        defeatCount.Clear();
                        break;
                    case ObstacleType.Obstacle:
                    case ObstacleType.Unit:
                        Vector3 obstacleStartPos = Vector3.Lerp(startPoint, endPoint, config.obstacles[nextObstacleIndex].position / config.timelineLength);
                        float actualResolveDuration = resolveDuration / DifficultyService.instance.timelineSpeed;
                        for(float resolveTime = 0; resolveTime < actualResolveDuration; resolveTime += Time.deltaTime)
                        {
                            playerLocalPosition = Vector3.Lerp(startPoint, endPoint, position / config.timelineLength);
                            visibleObstacles[nextObstacleIndex].transform.localPosition = obstacleStartPos + resolveTime / actualResolveDuration * resolveShakeIntensity * (player.transform.up * Random.Range(-1f, 1f) + player.transform.right * Random.Range(-1f, 1f));
                            player.transform.localPosition = playerLocalPosition + resolveTime / actualResolveDuration * resolveShakeIntensity * (Vector3.up * Random.Range(-1f, 1f) + Vector3.right * Random.Range(-1f, 1f));
                            yield return null;
                        }
                        visibleObstacles[nextObstacleIndex].transform.localPosition = obstacleStartPos;
                        player.transform.localPosition = playerLocalPosition;

                        int probIndex = 0;
                        if(defeatCount.ContainsKey(nextObstacleIndex))
                            probIndex = defeatCount[nextObstacleIndex];
                        float[] crossProbabilities = config.obstacles[nextObstacleIndex].crossProbabilities;
                        float successRatio = crossProbabilities[Mathf.Min(probIndex, crossProbabilities.Length-1)];

                        if(Random.Range(0f, 1f) < successRatio / DifficultyService.instance.successProbabilityFactor && !forceDeath)
                        {
                            obstaclePassed = true;
                            visibleObstacles[nextObstacleIndex].gameObject.SetActive(false);
                            
                            AnimatedSprite explosionFx = Instantiate(bubbleExplosionPrefab, timelineObstacleContainer);
                            explosionFx.transform.position = visibleObstacles[nextObstacleIndex].explosionFxPosition.position;
                            while(!explosionFx.isAnimationFinished)
                                yield return null;
                            Destroy(explosionFx.gameObject);
                        }
                        else
                        {
                            if(!defeatCount.ContainsKey(nextObstacleIndex))
                                defeatCount[nextObstacleIndex] = 1;
                            else defeatCount[nextObstacleIndex]++;

                            AnimatedSprite explosionFx = Instantiate(bubbleExplosionPrefab, timelineObstacleContainer);
                            explosionFx.transform.position = player.explosionFxPosition.position;
                            Vector3 velocity = playerEjectionVelocity;
                            
                            Vector3 initialPos = player.animationCenter.localPosition;
                            Quaternion initialRot = player.animationCenter.localRotation;
                            for(float time = 0; time < 2; time += Time.deltaTime)
                            {
                                player.animationCenter.position += velocity * Time.deltaTime;
                                velocity += playerEjectionGravity * Time.deltaTime;
                                player.animationCenter.localRotation *= Quaternion.AngleAxis(Time.deltaTime * playerEjectionRotationSpeed, Vector3.forward);
                                if(explosionFx != null && explosionFx.isAnimationFinished)
                                {
                                    Destroy(explosionFx.gameObject);
                                    explosionFx = null;
                                }
                                yield return null;
                            }
                            player.animationCenter.localPosition = initialPos;
                            player.animationCenter.localRotation = initialRot;
                            position = checkpointPosition;
                            foreach(int obstacleIndex in GetObstaclesAfter(checkpointPosition))
                            {
                                if(visibleObstacles[obstacleIndex] != null)
                                    visibleObstacles[obstacleIndex].gameObject.SetActive(true);
                            }
                        }
                        break;
                }
                
            }
            else
            {
                obstaclePassed = false;
                player.transform.localPosition = playerLocalPosition;
            }
            yield return null;
        }
    }

    private void OnObstacleAdded()
    {

    }

    public void LockUnits()
    {
        for(int i=0; i<visibleObstacles.Count; i++)
        {
            if(config.obstacles[i].obstacleType == ObstacleType.Slot)
            {
                DragTarget dragTarget = visibleObstacles[i].GetComponentInChildren<DragTarget>();
                if(dragTarget != null && dragTarget.containedUnit != null)
                {
                    TimelineObstacle newObstacle = dragTarget.containedUnit.source.obstacleDataDelegate();
                    newObstacle.position = config.obstacles[i].position;
                    config.obstacles[i] = newObstacle;
                }
            }
        }
    }
}
