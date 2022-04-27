using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour
{
    public BuildingConfig currentConfig;
    public SpriteRenderer spriteRenderer;
    public float appearRatio = 0;
    public float appearMovementScale = 1;
    public float shakeIntensity = 0;
    private Vector3 localOffset;

    private void Start()
    {
        spriteRenderer.sprite = currentConfig == null ? null : currentConfig.sprite;
        localOffset = transform.localPosition;
    }

    private void Update()
    {
        spriteRenderer.sprite = currentConfig.sprite;
        transform.localPosition = localOffset * (1 - appearRatio) + new Vector3(Random.Range(-shakeIntensity, shakeIntensity), ((appearRatio - 1) * appearMovementScale + Random.Range(-shakeIntensity, shakeIntensity)), 0);
    }

    private void UpdateDisplay()
    {
    }
}
