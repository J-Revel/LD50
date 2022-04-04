using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelinePlayerRoutine : MonoBehaviour
{
    public TimelineDisplay timelinePrefab;
    public Transform timelineContainer;
    public TimelineObstacle[] availableObstacles;
    public TimelineObstacle slotObstacle;
    public TimelineObstacle checkpointObstacle;
    public TimelineConfig config;
    public float obstacleProbability = 0.3f;
    private ImprovementUI improvementUI;

    void Start()
    {
        improvementUI = GetComponent<ImprovementUI>();
        GenerateRandomTimeline(1);
        GetComponentInParent<RoutineSequence>().RegisterRoutine(TimelinePlayerCoroutine(), 0);
        TimelineDataHolder dataHolder = gameObject.AddComponent<TimelineDataHolder>();
        dataHolder.timelineData = config;
    }

    public void ChangeTimelineLength(int sectionCount)
    {
        for(int section = (int)config.timelineLength; section < sectionCount; section++)
        {
            if(section > 0)
            {
                TimelineObstacle obstacle = checkpointObstacle;
                obstacle.position = section;
                
                config.obstacles.Add(obstacle);
            }
            int obstaclePerSection = 3;
            for(int i=0; i<obstaclePerSection; i++)
            {
                if(Random.Range(0f, 1f) < obstacleProbability)
                {
                    TimelineObstacle newObstacle = availableObstacles[Random.Range(0, availableObstacles.Length)];
                    newObstacle.position = section + ((float)i + 1) / (obstaclePerSection + 1);
                    
                    config.obstacles.Add(newObstacle);

                }
                else
                {
                    TimelineObstacle newObstacle = slotObstacle;
                    newObstacle.position = section + ((float)i + 1) / (obstaclePerSection + 1);
                    
                    config.obstacles.Add(newObstacle);
                }
            }
        }
        config.timelineLength = sectionCount;

    }

    public void GenerateRandomTimeline(int sectionCount)
    {
        config = ScriptableObject.CreateInstance<TimelineConfig>();

        config.timelineLength = sectionCount;
        for(int section = 0; section < sectionCount; section++)
        {
            if(section > 0)
            {
                TimelineObstacle obstacle = checkpointObstacle;
                obstacle.position = section;
                
                config.obstacles.Add(obstacle);
            }
            int obstaclePerSection = 3;
            for(int i=0; i<obstaclePerSection; i++)
            {
                if(Random.Range(0f, 1f) < obstacleProbability)
                {
                    TimelineObstacle newObstacle = availableObstacles[Random.Range(0, availableObstacles.Length)];
                    newObstacle.position = section + ((float)i + 1) / (obstaclePerSection + 1);
                    
                    config.obstacles.Add(newObstacle);

                }
                else
                {
                    TimelineObstacle newObstacle = slotObstacle;
                    newObstacle.position = section + ((float)i + 1) / (obstaclePerSection + 1);
                    
                    config.obstacles.Add(newObstacle);
                }
            }
        }
    }

    public IEnumerator TimelinePlayerCoroutine()
    {
        TimelineDisplay timeline = Instantiate(timelinePrefab, timelineContainer);
        TimelineDataHolder dataHolder = timeline.gameObject.AddComponent<TimelineDataHolder>();
        dataHolder.timelineData = config;
        yield return timeline.PlayTimelineCoroutine();
        Destroy(timeline.gameObject);
        yield return improvementUI.DestroyAnimationCoroutine();
    }
}
