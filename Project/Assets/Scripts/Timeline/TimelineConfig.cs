using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimelineConfig : ScriptableObject
{
    public Transform buildingIconPrefab;
    public List<TimelineObstacle> obstacles = new List<TimelineObstacle>();
    public float timelineLength = 1;
    public float resolveDuration = 1;

    // TO ENSURE : add always last, move doesn't swap obstacles
    public System.Action<int> obstacleAddedDelegate;
    public System.Action<int> obstacleRemovedDelegate;
    public System.Action<int> obstacleMovedDelegate;
}