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
    private TimelineDataHolder dataHolder;
    public float timelineAppearDuration = 0.5f;

    void Start()
    {
        improvementUI = GetComponent<ImprovementUI>();
        GenerateRandomTimeline(improvementUI);
        GetComponentInParent<RoutineSequence>().RegisterRoutine(TimelinePlayerCoroutine(), 0);
        dataHolder = gameObject.AddComponent<TimelineDataHolder>();
        dataHolder.timelineData = config;
        dataHolder.buildingUI = improvementUI;
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

    public void GenerateRandomTimeline(ImprovementUI improvementUI)
    {
        
        int timelineLength = 1;
        if(improvementUI.currentConfig != null)
            timelineLength = (int)improvementUI.currentConfig.timelineLength;
        config = ScriptableObject.CreateInstance<TimelineConfig>();

        config.timelineLength = timelineLength;
        config.buildingIconPrefab = improvementUI.currentConfig.buildingIconPrefab;
        for(int section = 0; section < timelineLength; section++)
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
        TimelineDisplay timeline = Instantiate(timelinePrefab, transform.position, TimelineContainer.instance.transform.rotation);
        TimelineDataHolder dataHolder = timeline.gameObject.AddComponent<TimelineDataHolder>();
        dataHolder.timelineData = config;
        dataHolder.buildingUI = improvementUI;

        timeline.transform.SetParent(TimelineContainer.instance.transform);
        for(float time = 0; time < timelineAppearDuration; time += Time.deltaTime)
        {
            timeline.transform.localPosition = Vector3.Lerp(timeline.transform.parent.InverseTransformPoint(transform.position), Vector3.zero, time / timelineAppearDuration);
            timeline.transform.localScale = Vector3.one * Mathf.Max(0.01f, time / timelineAppearDuration);
            yield return null;
        }
        yield return timeline.PlayTimelineCoroutine();
        for(float time = 0; time < timelineAppearDuration; time += Time.deltaTime)
        {
            timeline.transform.localPosition = Vector3.Lerp(Vector3.zero, timeline.transform.parent.InverseTransformPoint(TimelineContainer.instance.transform.position), 1 - time / timelineAppearDuration);
            timeline.transform.localScale = Vector3.one * (1 - time / timelineAppearDuration);
            yield return null;
        }
        Destroy(timeline.gameObject);
        yield return improvementUI.DestroyAnimationCoroutine();
    }
}
