using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MuteAudio : MonoBehaviour
{
    public AudioMixer mixer;
    public TMPro.TextMeshProUGUI text;
    public static bool mute = false;

    void Start()
    {
        mixer.SetFloat("Master Volume", mute ? -80 : -10);
        text.text = "Music: " + (mute ? "Off" : "On");
    }

    public void ToggleAudio()
    {
        mute = !mute;
        text.text = "Music: " + (mute ? "Off" : "On");
        mixer.SetFloat("Master Volume", mute ? -80 : -10);
    }
}