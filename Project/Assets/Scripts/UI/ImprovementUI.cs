using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementUI : MonoBehaviour
{
    public GameObject[] improvementPanels;
    public Button[] panelButtons;
    public SpriteRenderer activeSprite;
    private Vector3 startActiveSpritePos;
    public SpriteRenderer improvementSprite;
    private Vector3 startImprovementSpritePos;
    public float undergroundOffset = 1;

    public float buildingTime = 0;
    public float buildingDifficulty = 1;
    public float buildingForce = 0;
    public float buildingRatio { get { return buildingTime / buildingDifficulty; }}
    public GameObject buildingFX;

    public Transform focusPositionTransform;
    public float focusZoom = 2;
    public float shakeOffset = 0.05f;
    public UpgradeSubMenu[] upgradeSubMenus;
    public System.Action buildingFinishedDelegate;
    
    void Start()
    {
        buildingFX.SetActive(false);
        startActiveSpritePos = activeSprite.transform.position;
        startImprovementSpritePos = improvementSprite.transform.position;
        for(int i=0; i<improvementPanels.Length; i++)
        {
            int index = i;
            panelButtons[index].onClick.AddListener(() => {
                for(int j=0; j<improvementPanels.Length; j++)
                {
                    improvementPanels[j].SetActive(j == index);
                }
            });
        }
    }

    public void OpenUI()
    {
        CameraFocusManager.instance.TakeFocus(focusPositionTransform.position, focusZoom, true);
        CameraFocusManager.instance.focusLostDelegate += OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate += OnFocusLost;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = true;
        }
    }

    private void OnFocusLost()
    {
        CameraFocusManager.instance.focusLostDelegate -= OnFocusLost;
        CameraFocusManager.instance.focusStolenDelegate -= OnFocusLost;
        for(int i=0; i<upgradeSubMenus.Length; i++)
        {
            upgradeSubMenus[i].visible = false;
        }
    }

    public void StartBuilding(float difficulty)
    {
        buildingDifficulty = difficulty;
        buildingTime = 0;
        buildingFX.SetActive(true);
    }

    void Update()
    {
        
        buildingTime += Time.deltaTime * buildingForce;
        if(buildingTime >= buildingDifficulty)
        {
            buildingTime = 0;
            buildingFinishedDelegate?.Invoke();
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
