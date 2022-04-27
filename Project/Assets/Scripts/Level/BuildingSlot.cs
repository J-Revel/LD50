using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlot : MonoBehaviour
{
    public BuildingDisplay currentDisplay;
    public BuildingDisplay pendingDisplay;
    public float buildingTime;
    private UpgradeConfig currentUpgrade = null;
    public bool doUpgrade = false;
    public float buildingStrength = 1;

    void Start()
    {
        
    }
    public void DoRandomUpgrade()
    {
        if(currentDisplay.currentConfig != null && currentDisplay.currentConfig.upgrades.Length > 0)
        {
            var upgrades = currentDisplay.currentConfig.upgrades;
            currentUpgrade = upgrades[Random.Range(0, upgrades.Length)];
            pendingDisplay.currentConfig = currentUpgrade.result;
            buildingTime = 0;
        }
    }

    void Update()
    {
        if(doUpgrade)
        {
            DoRandomUpgrade();
            doUpgrade = false;
        }
        if(currentUpgrade != null)
        {
            buildingTime += Time.deltaTime * buildingStrength;
            float buildingRatio = buildingTime / currentUpgrade.buildingTime;
            if(buildingRatio >= 1)
            {
                currentDisplay.currentConfig = currentUpgrade.result;
                currentDisplay.appearRatio = 1;
                pendingDisplay.appearRatio = 0;
                currentUpgrade = null;
            }
            else
            {
                currentDisplay.appearRatio = 1 - buildingRatio;
                pendingDisplay.appearRatio = buildingRatio;
            }
        }
    }
}
