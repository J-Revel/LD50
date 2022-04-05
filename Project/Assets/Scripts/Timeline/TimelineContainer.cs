using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineContainer : MonoBehaviour
{
    public static TimelineContainer instance;
    void Awake()
    {
        instance = this;
    }
}
