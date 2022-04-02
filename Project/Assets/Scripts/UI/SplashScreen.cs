using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class SplashScreen : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public float fadeinDuration = 0.5f;
    public float fadeoutDuration = 0.5f;
    public float duration = 5;

    private float time;
    private RoutineSequence routineSequence;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        routineSequence = GetComponent<RoutineSequence>();
        routineSequence.RegisterRoutine(PlayVideoCoroutine(), 0);
        // StartCoroutine(PlayVideoCoroutine());
    }

    IEnumerator PlayVideoCoroutine()
    {
        float time = 0;
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "SplashScreen.mp4");
        videoPlayer.targetCameraAlpha = 0;
        videoPlayer.Play();
        videoPlayer.Pause();
        for(time=0; time < fadeinDuration; time += Time.deltaTime)
        {
            videoPlayer.targetCameraAlpha = time / fadeinDuration;
            yield return null;
        }  
        videoPlayer.Play();
        yield return new WaitForSeconds((float)duration);
        for(time=0; time < fadeoutDuration; time += Time.deltaTime)
        {
            videoPlayer.targetCameraAlpha = 1 - time / fadeoutDuration;
            yield return null;
        }   

    }
}
