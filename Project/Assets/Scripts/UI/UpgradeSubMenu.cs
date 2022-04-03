using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSubMenu : MonoBehaviour
{
    private Vector3 startLocalScale;
    private Vector3 startLocalPosition;
    public bool visible = false;
    private float animTime = 0;
    public float animDuration = 0.5f;

    public UpgradeConfig upgradeConfig;
    public DragTarget[] dragTargets;
    public float buildingDuration = 1;
    public RectTransform barTransform;
    public Transform builtPointPrefab;
    public Image image;
    public RectTransform slotPanel;
    public RectTransform loadingPanel;
    private Vector3 slotPanelPosition;
    private Vector3 slotPanelSize;
    private Vector3 loadingPanelPosition;
    private Vector3 loadingPanelSize;
    public float buildingForce;
    public float buildingRatio;
    public System.Action<UpgradeConfig> buildingStartDelegate;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        startLocalScale = transform.localScale;
        slotPanelPosition = slotPanel.anchoredPosition3D;
        slotPanelSize = slotPanel.localScale;
        loadingPanelPosition = loadingPanel.anchoredPosition3D;
        loadingPanelSize = loadingPanel.localScale;
        loadingPanel.localScale = Vector3.zero;
        slotPanel.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        
    }

    void Update()
    {
        barTransform.anchorMax = new Vector2(buildingRatio, 1);
        buildingForce = 0;
        for(int i=0; i<dragTargets.Length; i++)
        {
            if(dragTargets[i].containedUnit != null)
            {
                buildingForce += dragTargets[i].containedUnit.buildSpeed;
            }
        }

        animTime = Mathf.Clamp(animTime + (visible ? 1:-1) * Time.deltaTime, 0, animDuration);
        float animRatio = 1 - (1 - animTime / animDuration) * (1 -  animTime / animDuration);
        transform.localPosition = Vector3.Lerp(Vector3.zero, startLocalPosition, animRatio);
        transform.localScale = Vector3.Lerp(Vector3.zero, startLocalScale, animRatio);
    }

    public void SetUgradeConfig(UpgradeConfig config)
    {
        upgradeConfig = config;
        image.sprite = config.result.sprite;
    }

    public void StartBuilding()
    {
        buildingStartDelegate?.Invoke(upgradeConfig);
        StartCoroutine(ShowSlotsAnimation());
    }

    public void ClosePanel()
    {
        StartCoroutine(HideSlotsAnimation());
    } 

    public IEnumerator ShowSlotsAnimation()
    {
        for(float animTime=0; animTime<animDuration; animTime+=Time.deltaTime)
        {
            float animRatio = 1 - (1 - animTime / animDuration) * (1 -  animTime / animDuration);
            loadingPanel.anchoredPosition3D = Vector3.Lerp(Vector3.zero, loadingPanelPosition, animRatio);
            loadingPanel.transform.localScale = Vector3.Lerp(Vector3.zero, loadingPanelSize, animRatio);
            slotPanel.anchoredPosition3D = Vector3.Lerp(Vector3.zero, slotPanelPosition, animRatio);
            slotPanel.transform.localScale = Vector3.Lerp(Vector3.zero, slotPanelSize, animRatio);
            yield return null;
        }
    }
    public IEnumerator HideSlotsAnimation()
    {
        for(int i=0; i<dragTargets.Length; i++)
        {
            dragTargets[i].DropContent();
        }
        for(float animTime=0; animTime<animDuration; animTime+=Time.deltaTime)
        {
            float animRatio = 1 - (1 - animTime / animDuration) * (1 -  animTime / animDuration);
            loadingPanel.anchoredPosition3D = Vector3.Lerp(Vector3.zero, loadingPanelPosition, 1 - animRatio);
            loadingPanel.transform.localScale = Vector3.Lerp(Vector3.zero, loadingPanelSize, 1 - animRatio);
            slotPanel.anchoredPosition3D = Vector3.Lerp(Vector3.zero, slotPanelPosition, 1 - animRatio);
            slotPanel.transform.localScale = Vector3.Lerp(Vector3.zero, slotPanelSize, 1 - animRatio);
            yield return null;
        }
    }
}
