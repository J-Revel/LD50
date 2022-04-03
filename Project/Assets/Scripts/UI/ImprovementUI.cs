using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImprovementUI : MonoBehaviour
{
    public GameObject[] improvementPanels;
    public Button[] panelButtons;
    void Start()
    {
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

    void Update()
    {
        
    }
}
