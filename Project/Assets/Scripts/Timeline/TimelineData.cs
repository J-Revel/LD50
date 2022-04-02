using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineData : MonoBehaviour
{
    public List<TimelineObstacle> obstacles = new List<TimelineObstacle>();
    public float timelineLength = 1;
    public float resolveDuration = 1;
    public float resolveShakeIntensity = 1;
    public float playerMovementSpeed = 0.2f;

    public System.Action<string> obstacleCrossedDelegate;
    public GameObject timelineObstaclePrefab;
    public GameObject playerBubblePrefab;
    public Transform timelineObstacleContainer;
    public Transform timelineStartPoint;
    public Transform timelineEndPoint;
    
    public void AddObstacle(TimelineObstacle obstacle)
    {
        obstacles.Add(obstacle);
    }

    public void DisplayObstable(TimelineObstacle obstacle)
    {
        GameObject instantiated = Instantiate(timelineObstaclePrefab, Vector3.Lerp(timelineStartPoint.position, timelineEndPoint.position, obstacle.position / timelineLength), timelineObstacleContainer.rotation, timelineObstacleContainer);
        instantiated.AddComponent<TimelineObstacleDataHolder>().data = obstacle;
    }

    private void Start()
    {
        for(int i=0; i<obstacles.Count; i++)
        {
            DisplayObstable(obstacles[i]);
        }
        StartCoroutine(PlayTimelineDelegate());
    }

    public IEnumerator PlayTimelineDelegate()
    {
        float position = 0;
        GameObject player = Instantiate(playerBubblePrefab, timelineStartPoint.position, timelineObstacleContainer.rotation, timelineObstacleContainer);
        int nextObstacleIndex = 0;
        float checkpointPosition = 0;
        int checkpointObstacleIndex = 0;
        
        while(position < timelineLength)
        {
            position += playerMovementSpeed * Time.deltaTime;
            Vector3 playerWorldPosition = Vector3.Lerp(timelineStartPoint.position, timelineEndPoint.position, position / timelineLength);
            if(nextObstacleIndex < obstacles.Count && position >= obstacles[nextObstacleIndex].position)
            {
                position = obstacles[nextObstacleIndex].position;
                for(float resolveTime = 0; resolveTime < resolveDuration; resolveTime += Time.deltaTime)
                {
                    player.transform.position = playerWorldPosition + resolveShakeIntensity * (player.transform.up * Random.Range(-1f, 1f) + player.transform.right * Random.Range(-1f, 1f));
                    yield return null;
                }
                float successRatio = obstacles[nextObstacleIndex].crossProbability;
                if(Random.Range(0, 1) < successRatio)
                {
                    nextObstacleIndex++;
                }
                else
                {
                    position = checkpointPosition;
                    nextObstacleIndex = checkpointObstacleIndex;
                }
            }
            else
            {
                player.transform.position = playerWorldPosition;
            }
            yield return null;
        }
    }

}
