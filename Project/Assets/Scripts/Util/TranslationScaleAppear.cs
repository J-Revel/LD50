using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationScaleAppear : MonoBehaviour
{
    public bool visible;
    public Vector3 targetPosition;
    public Vector3 targetScale;
    public Transform origin;
    public float duration = 0.5f;
    private float time;

    void Start()
    {
        targetPosition = transform.position;
        targetScale = transform.localScale;
    }

    void Update()
    {
        time = Mathf.Clamp(time + (visible ? 1 : -1) * Time.deltaTime, 0, duration);
        float ratio = time / duration;
        transform.position = Vector3.Lerp(origin.position, targetPosition, ratio);
        transform.localScale = Vector3.Lerp(new Vector3(0.001f, 0.001f, 0.001f), targetScale, ratio);
    }
}
