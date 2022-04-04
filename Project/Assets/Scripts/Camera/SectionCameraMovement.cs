using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionCameraMovement : MonoBehaviour
{
    public LevelGenerator level;
    public int visibleSectionIndex;
    public float transitionDuration = 1f;
    public int targetPosition;
    private Vector3 startPosition;
    private float animTime;
    private bool moving = false;
    public GameObject leftButton;
    public GameObject rightButton;

    void Start()
    {
        
    }

    public void MoveRight()
    {
        CameraFocusManager.instance.LoseFocus();
        startPosition = transform.position;
        visibleSectionIndex += 1;
        animTime = 0;
        moving = true;
        rightButton.SetActive(visibleSectionIndex < level.generatedTiles.Count - 1);
        leftButton.SetActive(visibleSectionIndex > 0);
    }

    public void MoveLeft()
    {
        CameraFocusManager.instance.LoseFocus();
        startPosition = transform.position;
        visibleSectionIndex -= 1;
        animTime = 0;
        moving = true;
        rightButton.SetActive(visibleSectionIndex < level.generatedTiles.Count - 1);
        leftButton.SetActive(visibleSectionIndex > 0);
    }

    void Update()
    {
        if(moving)
        {
            Vector3 targetPosition = level.generatedTiles[visibleSectionIndex].cameraTarget.position;
            float f = animTime / transitionDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, 1-(1-f) * (1-f));
            animTime += Time.deltaTime;
            if(animTime > transitionDuration)
            {
                moving = false;
                transform.position = targetPosition;
            }
        }

    }
}
