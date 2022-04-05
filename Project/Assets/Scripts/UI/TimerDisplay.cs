using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    float time;
    TMPro.TextMeshProUGUI text;

    void Start()
    {
        time = Timer.time;
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time) % 60;
        text.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
