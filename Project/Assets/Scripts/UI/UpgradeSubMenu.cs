using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSubMenu : MonoBehaviour
{
    private Canvas canvas;
    private Vector3 startLocalScale;
    private Vector3 startLocalPosition;
    public bool visible = false;
    private float animTime = 0;
    public float animDuration = 0.5f;

    private void Start()
    {
        startLocalPosition = transform.localPosition;
        startLocalScale = transform.localScale;
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        animTime = Mathf.Clamp(animTime + (visible ? 1:-1) * Time.deltaTime, 0, animDuration);
        canvas.enabled = animTime > 0;
        float animRatio = 1 - (1 - animTime / animDuration) * (1 -  animTime / animDuration);
        transform.localPosition = Vector3.Lerp(Vector3.zero, startLocalPosition, animRatio);
        transform.localScale = Vector3.Lerp(Vector3.zero, startLocalScale, animRatio);
    }

}
