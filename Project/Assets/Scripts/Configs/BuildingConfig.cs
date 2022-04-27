using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeConfig
{
    public BuildingConfig result;
    public float buildingTime = 1;
}

[CreateAssetMenu()]
public class BuildingConfig : ScriptableObject
{
    public Transform buildingIconPrefab;
    public Sprite sprite;
    public Sprite ruinSprite;
    public BuildingConfig ruinBuilding;
    public UpgradeConfig[] upgrades;

    public string description;

    public float unitProductionDelay;
    public string unitProduced;
    public float timelineLength;
}
