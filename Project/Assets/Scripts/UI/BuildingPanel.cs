using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    private ImprovementUI improvementUI;
    public DragTarget[] dragTargets;
    public float buildingDuration = 1;
    public RectTransform barTransform;
    public Transform builtPointPrefab;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        improvementUI = GetComponentInParent<ImprovementUI>();
        improvementUI.StartBuilding(buildingDuration);
    }

    void Update()
    {
        barTransform.anchorMax = new Vector2(improvementUI.buildingRatio, 1);
        float buildingForce = 0;
        for(int i=0; i<dragTargets.Length; i++)
        {
            if(dragTargets[i].containedUnit != null)
            {
                buildingForce += dragTargets[i].containedUnit.buildSpeed;
            }
        }
        improvementUI.buildingForce = buildingForce;
    }
}
