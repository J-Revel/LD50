using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementUI : MonoBehaviour
{
    public BuildingConfig currentConfig;
    public BuildingConfig pendingBuildingConfig;
    // public Button[] panelButtons;
    public SpriteRenderer activeSprite;
    private Vector3 startActiveSpritePos;
    public SpriteRenderer improvementSprite;
    private Vector3 startImprovementSpritePos;
    public float undergroundOffset = 1;

    public float buildingTime = 0;
    public float buildingDifficulty = 1;
    public float buildingRatio { get { return buildingTime / buildingDifficulty; }}
    public GameObject buildingFX;

    public Transform focusPositionTransform;
    public float focusZoom = 2;
    public float shakeOffset = 0.05f;
    public UpgradeSubMenu[] upgradeSubMenus;
    private int currentUpgradeIndex;
    
    public System.Action buildingFinishedDelegate;
    private bool hasFocus = false;
    
    void Start()
    {
        if(currentConfig.sprite != null)
            activeSprite.sprite = currentConfig.sprite;
        else
            activeSprite.enabled = false;
            
        buildingFX.SetActive(false);
        startActiveSpritePos = activeSprite.transform.position;
        startImprovementSpritePos = improvementSprite.transform.position;
        improvementSprite.transform.position = startImprovementSpritePos + undergroundOffset * -improvementSprite.transform.up;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            int index = i;
            upgradeSubMenus[i].buildingStartDelegate += (UpgradeConfig config) => {
                pendingBuildingConfig = config.result;
                buildingDifficulty = config.buildingTime;
                buildingTime = 0;
                buildingFX.SetActive(true);
                improvementSprite.sprite = config.result.sprite;
                if(currentUpgradeIndex != index && currentUpgradeIndex >= 0)
                {
                    upgradeSubMenus[currentUpgradeIndex].ClosePanel();
                }
                currentUpgradeIndex = index;
                UpdateDisplay();
            };
        }
        
        UpdateDisplay();
    }

    public void OpenUI()
    {
        CameraFocusManager.instance.TakeFocus(focusPositionTransform.position, focusZoom, true);
        CameraFocusManager.instance.focusLostDelegate += OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate += OnFocusLost;
        hasFocus = true;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = true;
        }
    }

    private void OnFocusLost()
    {
        hasFocus = false;
        CameraFocusManager.instance.focusLostDelegate -= OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate -= OnFocusLost;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = false;
        }
    }

    void Update()
    {
        
        if(currentUpgradeIndex >= 0 && pendingBuildingConfig != null)
        {
            float buildingForce = upgradeSubMenus[currentUpgradeIndex].buildingForce;
            buildingTime += Time.deltaTime * buildingForce;
            upgradeSubMenus[currentUpgradeIndex].buildingRatio = buildingTime / buildingDifficulty;
            if(buildingTime >= buildingDifficulty)
            {
                buildingTime = 0;
                buildingFinishedDelegate?.Invoke();
                currentConfig = pendingBuildingConfig;
                pendingBuildingConfig = null;
                buildingFX.SetActive(false);
                upgradeSubMenus[currentUpgradeIndex].ClosePanel();
                if(hasFocus)
                {
                    CameraFocusManager.instance.LoseFocus();
                }
                UpdateDisplay();
            }
            Vector3 offset = Vector3.zero;
            if(buildingForce > 0)
            {
                offset = shakeOffset * (Random.Range(-1, 1) * transform.right + Random.Range(-1, 1) * transform.up);
            }
            improvementSprite.transform.position = startImprovementSpritePos + undergroundOffset * (1 - buildingRatio) * -improvementSprite.transform.up + offset;
            offset = Vector3.zero;
            if(buildingForce > 0)
            {
                offset = shakeOffset * (Random.Range(-1, 1) * transform.right + Random.Range(-1, 1) * transform.up);
                buildingForce = 0;
            }
            activeSprite.transform.position = startActiveSpritePos + undergroundOffset * buildingRatio * -improvementSprite.transform.up + offset;
        }
        
    }

    void UpdateDisplay()
    {
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            if(i<currentConfig.upgrades.Length)
                upgradeSubMenus[i].SetUgradeConfig(currentConfig.upgrades[i]);
            upgradeSubMenus[i].gameObject.SetActive(i<currentConfig.upgrades.Length);
        }
        activeSprite.enabled = currentConfig.sprite != null;
        activeSprite.sprite = currentConfig.sprite;
    }
}
