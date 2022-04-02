using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    Obstacle,
    Checkpoint,
}

[System.Serializable]
public struct TimelineObstacle
{
    public string id;
    public float position;
    public float crossProbability;
    public ObstacleType obstacleType;
    public float crossProbabilityIncrease;
    public GameObject bubblePrefab;
    public Sprite icon;
}

public class TimelineObstacleDataHolder : MonoBehaviour
{
    public TimelineObstacle data;
}