using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyService : MonoBehaviour
{
    public static DifficultyService instance;
    public DifficultyConfig config;
    public float time = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public float movementSpeed { get { return Mathf.Lerp(config.startMovementSpeed, config.endMovementSpeed, time / config.maxDifficultyTime); } }
    public float timelineSpeed { get { return Mathf.Lerp(config.startTimelineSpeed, config.endTimelineSpeed, time / config.maxDifficultyTime); } }
    public float successProbabilityFactor { get { return Mathf.Lerp(config.startSuccessProbabilityFactor, config.endSuccessProbabilityFactor, time / config.maxDifficultyTime); } }
}
