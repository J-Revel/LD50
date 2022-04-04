using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathTracer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = transform.childCount;
        for(int i=0; i<transform.childCount; i++)
        {
            lineRenderer.SetPosition(i, transform.GetChild(i).position);
        }
    }

    void Update()
    {
        if(!Application.isPlaying)
        {
            lineRenderer.positionCount = transform.childCount;
            for(int i=0; i<transform.childCount; i++)
            {
                lineRenderer.SetPosition(i, transform.GetChild(i).position);
            }
        }
    }
}
