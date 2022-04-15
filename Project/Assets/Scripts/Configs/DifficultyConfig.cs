using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DifficultyConfig : ScriptableObject
{
    public float startSuccessProbabilityFactor = 1;
    public float endSuccessProbabilityFactor = 0.2f;
    public float maxDifficultyTime = 500;
    public float startTimelineSpeed = 1;
    public float endTimelineSpeed = 3;
    public float startMovementSpeed = 1;
    public float endMovementSpeed = 5;
}
