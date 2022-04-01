using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public float fadeinDuration = 0.5f;
    public float fadeoutDuration = 0.5f;

    private float time;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideoCoroutine());
    }

    IEnumerator PlayVideoCoroutine()
    {
        float time = 0;
        videoPlayer.targetCameraAlpha = 0;
        videoPlayer.Play();
        videoPlayer.Pause();
        for(time=0; time < fadeinDuration; time += Time.deltaTime)
        {
            videoPlayer.targetCameraAlpha = time / fadeinDuration;
            yield return null;
        }  
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        for(time=0; time < fadeoutDuration; time += Time.deltaTime)
        {
            videoPlayer.targetCameraAlpha = 1 - time / fadeoutDuration;
            yield return null;
        }   

    }
}
