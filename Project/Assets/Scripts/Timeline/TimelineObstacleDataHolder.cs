using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TimelineObstacle
{
    public string id;
    public float position;
    public float crossProbability;
    public bool killable;
    public float crossProbabilityIncrease;
    public Sprite icon;
}

public class TimelineObstacleDataHolder : MonoBehaviour
{
    public TimelineObstacle data;
}