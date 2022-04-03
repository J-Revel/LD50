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
    public Sprite sprite;
    public UpgradeConfig[] upgrades;

    public float unitProductionDelay;
    public Transform unitProduced;
    public float timelineLength;
}
