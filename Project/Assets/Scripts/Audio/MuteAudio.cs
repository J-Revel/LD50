using UnityEngine;
using System.Collections;

public class MuteAudio : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            audioSource.mute = !audioSource.mute;
    }
}