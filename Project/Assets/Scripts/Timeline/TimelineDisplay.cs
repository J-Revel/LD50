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
    private TimelineDataHolder dataHolder;
    private List<GameObject> visibleObstacles = new List<GameObject>();
    private List<ObstacleSortData> sortedObstacles = new List<ObstacleSortData>();
    public float resolveDuration = 1;
    public float resolveShakeIntensity = 1;
    public float playerMovementSpeed = 0.2f;

    public GameObject playerBubblePrefab;
    public Transform timelineObstacleContainer;
    public Transform timelineStartPoint;
    public Transform timelineEndPoint;

    public TimelineConfig config { get { return dataHolder.timelineData; }}
    
    public void AddObstacle(TimelineObstacle obstacle)
    {
        config.obstacles.Add(obstacle);
    }

    private void Start()
    {
        dataHolder = GetComponent<TimelineDataHolder>();
        for(int i=0; i<config.obstacles.Count; i++)
        {
            Vector3 characterPosition = Vector3.Lerp(timelineStartPoint.position, timelineEndPoint.position, config.obstacles[i].position / config.timelineLength);
            GameObject instantiated = Instantiate(config.obstacles[i].bubblePrefab, characterPosition, timelineObstacleContainer.rotation, timelineObstacleContainer);
            visibleObstacles.Add(instantiated);
            instantiated.AddComponent<TimelineObstacleDataHolder>().data = config.obstacles[i];
        }
        // dataHolder.timelineData.obstacleAddedDelegate += OnObstacleAdded;
        // dataHolder.timelineData.obstacleRemovedDelegate += OnObstacleRemoved;
        // dataHolder.timelineData.obstacleMovedDelegate += OnObstacleMoved;
        // StartCoroutine(PlayTimelineCoroutine());
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
        GameObject player = Instantiate(playerBubblePrefab, timelineStartPoint.position, timelineObstacleContainer.rotation, timelineObstacleContainer);
        
        float checkpointPosition = 0;
        
        bool obstaclePassed = false;
        while(position < config.timelineLength)
        {
            int nextObstacleIndex = GetNextObstacleIndex(position);
            position += playerMovementSpeed * Time.deltaTime;
            Vector3 playerWorldPosition = Vector3.Lerp(timelineStartPoint.position, timelineEndPoint.position, position / config.timelineLength);
            if(nextObstacleIndex >= 0 && !obstaclePassed && nextObstacleIndex < config.obstacles.Count && position >= config.obstacles[nextObstacleIndex].position)
            {
                position = config.obstacles[nextObstacleIndex].position;
                switch(config.obstacles[nextObstacleIndex].obstacleType)
                {
                    case ObstacleType.Checkpoint:
                        obstaclePassed = true;
                        checkpointPosition = config.obstacles[nextObstacleIndex].position;
                        break;
                    case ObstacleType.Obstacle:
                        for(float resolveTime = 0; resolveTime < resolveDuration; resolveTime += Time.deltaTime)
                        {
                            player.transform.position = playerWorldPosition + resolveShakeIntensity * (player.transform.up * Random.Range(-1f, 1f) + player.transform.right * Random.Range(-1f, 1f));
                            yield return null;
                        }
                        float successRatio = config.obstacles[nextObstacleIndex].crossProbability;
                        if(Random.Range(0f, 1f) < successRatio)
                        {
                            obstaclePassed = true;
                            visibleObstacles[nextObstacleIndex].SetActive(false);
                        }
                        else
                        {
                            position = checkpointPosition;
                            foreach(int obstacleIndex in GetObstaclesAfter(checkpointPosition))
                            {
                                visibleObstacles[obstacleIndex].SetActive(true);
                            }
                        }
                        break;
                }
                
            }
            else
            {
                obstaclePassed = false;
                player.transform.position = playerWorldPosition;
            }
            yield return null;
        }
    }

    private void OnObstacleAdded()
    {

    }
}
