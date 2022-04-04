using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    Obstacle,
    Checkpoint,
    Unit,
    Slot,
}

[System.Serializable]
public struct TimelineObstacle
{
    public string id;
    public float position;
    public float crossProbability;
    public ObstacleType obstacleType;
    public float crossProbabilityIncrease;
    public BubbleWidget bubblePrefab;
    public Sprite icon;
}

public class TimelineObstacleDataHolder : MonoBehaviour
{
    public TimelineObstacle data;
}