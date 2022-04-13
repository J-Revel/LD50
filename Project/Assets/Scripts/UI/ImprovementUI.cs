using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementUI : MonoBehaviour
{
    public int checkpointIndex;
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
    public TranslationScaleAppear[] toAppearOnFocus;
    public TimelineDisplay timeline;
    private int currentUpgradeIndex;
    
    public System.Action buildingFinishedDelegate;
    private bool hasFocus = false;
    public GameObject productionPanel;
    public RectTransform productionBar;
    private float productionTime = 0;
    public Transform productionPoint;
    public float destroyAnimDuration = 2;
    public BuildingConfig castleConfig;
    public Collider interactionCollider;
    private bool locked;
    
    void Start()
    {
        if(currentConfig.sprite != null)
            activeSprite.sprite = currentConfig.sprite;
        else
            activeSprite.enabled = false;

        productionPanel.SetActive(StockDragSource.instances.ContainsKey(currentConfig.unitProduced));
        productionTime = 0;
            
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
        foreach(TranslationScaleAppear element in toAppearOnFocus)
        {
            element.visible = true;
        }
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = true;
        }
    }

    public void OnFocusLost()
    {
        hasFocus = false;
        CameraFocusManager.instance.focusLostDelegate -= OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate -= OnFocusLost;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = false;
        }
        
        foreach(TranslationScaleAppear element in toAppearOnFocus)
        {
            element.visible = false;
        }
    }

    public void LockBuildingForCombat()
    {
        locked = true;
        productionTime = 0;
        productionPanel.SetActive(false);
        interactionCollider.enabled = false;
        currentUpgradeIndex = -1;
        if(hasFocus)
        {
            CameraFocusManager.instance.LoseFocus();
            hasFocus = false;
        }
        CameraFocusManager.instance.focusLostDelegate -= OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate -= OnFocusLost;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = false;
        }
        
        foreach(TranslationScaleAppear element in toAppearOnFocus)
        {
            element.visible = false;
        }
        timeline.LockUnits();
    }

    void Update()
    {
        if(StockDragSource.instances.ContainsKey(currentConfig.unitProduced) && !locked)
        {
            productionTime += Time.deltaTime;
            productionBar.anchorMax = new Vector2(productionTime / currentConfig.unitProductionDelay, 1);
            if(productionTime > currentConfig.unitProductionDelay)
            {
                productionTime = 0;
                StockDragSource dragSource = StockDragSource.instances[currentConfig.unitProduced];
                Instantiate(dragSource.droppedElementPrefab, productionPoint.position, productionPoint.rotation).source = dragSource.GetComponent<DragSource>();
                productionTime = 0;
            }
        }
        if(currentUpgradeIndex >= 0 && pendingBuildingConfig != null)
        {
            float buildingForce = upgradeSubMenus[currentUpgradeIndex].buildingForce;
            buildingTime += Time.deltaTime * buildingForce;
            upgradeSubMenus[currentUpgradeIndex].buildingRatio = buildingTime / buildingDifficulty;
            if(buildingTime >= buildingDifficulty)
            {
                buildingTime = 0;
                buildingFinishedDelegate?.Invoke();
                UpgradeConfig upgradeConfig = upgradeSubMenus[currentUpgradeIndex].upgradeConfig;
                GetComponent<TimelinePlayerRoutine>().ChangeTimelineLength((int)upgradeConfig.result.timelineLength);
                currentConfig = pendingBuildingConfig;
                pendingBuildingConfig = null;
                buildingFX.SetActive(false);
                if(upgradeConfig.result == castleConfig)
                {
                    GetComponentInParent<LevelTile>().castleBuiltDelegate?.Invoke(checkpointIndex);
                }
                upgradeSubMenus[currentUpgradeIndex].OnBuildFinished();
                productionPanel.SetActive(StockDragSource.instances.ContainsKey(currentConfig.unitProduced));
                productionTime = 0;
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

    public IEnumerator DestroyAnimationCoroutine()
    {
        buildingFX.SetActive(true);
        if(activeSprite.sprite == null)
        {
            yield break;
        }
        for(float time=0; time<destroyAnimDuration; time += Time.deltaTime)
        {
            Vector3 offset = Vector3.zero;
            float animRatio = time / destroyAnimDuration;
            offset = shakeOffset * (Random.Range(-1, 1) * transform.right + Random.Range(-1, 1) * transform.up);
            activeSprite.transform.position = startImprovementSpritePos + undergroundOffset * animRatio * -improvementSprite.transform.up + offset;
            yield return null;
        }
        if(currentConfig == castleConfig)
        {
            GetComponentInParent<LevelTile>().castleDestroyedDelegate?.Invoke(checkpointIndex);
        }
    }
}
