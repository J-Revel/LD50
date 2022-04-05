using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float time = 0;
    TMPro.TextMeshProUGUI text;

    void Start()
    {
        time = 0;
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time) % 60;
        text.text = minutes.ToString("00") + " : " + seconds.ToString("00");
    }
}
