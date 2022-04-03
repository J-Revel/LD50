using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelinePlayerRoutine : MonoBehaviour
{
    public TimelineDisplay timelinePrefab;
    public Transform timelineContainer;
    public TimelineObstacle[] availableObstacles;
    public TimelineObstacle checkpointObstacle;
    public TimelineConfig config;

    void Start()
    {
        GenerateRandomTimeline();
        GetComponentInParent<RoutineSequence>().RegisterRoutine(TimelinePlayerCoroutine(), 0);
        TimelineDataHolder dataHolder = gameObject.AddComponent<TimelineDataHolder>();
        dataHolder.timelineData = config;
    }

    public void GenerateRandomTimeline()
    {
        config = ScriptableObject.CreateInstance<TimelineConfig>();

        int sectionCount = Random.Range(1, 4);
        config.timelineLength = sectionCount;
        for(int section = 0; section < sectionCount; section++)
        {
            if(section > 0)
            {
                TimelineObstacle obstacle = checkpointObstacle;
                obstacle.position = section;
                
                config.obstacles.Add(obstacle);
            }
            int obstaclePerSection = Random.Range(0, 4);
            for(int i=0; i<obstaclePerSection; i++)
            {
                TimelineObstacle newObstacle = availableObstacles[Random.Range(0, availableObstacles.Length)];
                newObstacle.position = section + ((float)i + Random.Range(0.2f / sectionCount, 0.7f / sectionCount)) / obstaclePerSection;
                
                config.obstacles.Add(newObstacle);
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
    }
}
