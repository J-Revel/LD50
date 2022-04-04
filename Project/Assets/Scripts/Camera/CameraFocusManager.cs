using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusManager : MonoBehaviour
{
    public static CameraFocusManager instance;
    public System.Action focusLostDelegate;
    public System.Action focusStolenDelegate;

    public bool focusTaken = false;
    public Vector3 initialPosition;
    public float initialZoom;

    private bool inTransition;
    private float transitionTime = 0;
    public Vector3 startPosition;
    public float startZoom;
    private float targetZoom;
    private Vector3 targetPosition;
    private bool isTargetLocalPosition;

    public float transitionDuration = 0.5f;

    private void Awake()
    {
        instance = this;
    }

    public bool TakeFocus(Vector3 focusPosition, float focusZoom, bool canSteal)
    {
        if(!focusTaken)
        {
            this.initialPosition = transform.localPosition;
            this.initialZoom = Camera.main.orthographicSize;
        }
        bool steal = focusTaken && canSteal;
        if(!focusTaken || canSteal)
        {
            this.startPosition = transform.localPosition;
            this.startZoom = Camera.main.orthographicSize;
            focusTaken = true;
            targetPosition = focusPosition;
            isTargetLocalPosition = false;
            targetZoom = focusZoom;
            inTransition = true;
            transitionTime = 0;
            if(steal)
                focusStolenDelegate?.Invoke();
            return true;
        }
        return false;
    }

    private void Update()
    {
        if(inTransition)
        {
            transitionTime += Time.deltaTime;
            if(transitionTime >= transitionDuration)
            {
                transitionTime = transitionDuration;
                inTransition = false;
            }
            float transitionRatio = 1 - (1 - transitionTime / transitionDuration) * (1 - transitionTime / transitionDuration);
            Vector3 realTargetPosition = targetPosition;
            if(isTargetLocalPosition)
                realTargetPosition = transform.parent.TransformPoint(targetPosition);
            transform.position = Vector3.Lerp(transform.parent.TransformPoint(startPosition), realTargetPosition, transitionRatio);
            Camera.main.orthographicSize = Mathf.Lerp(startZoom, targetZoom, transitionRatio);
        }
    }

    public void LoseFocus()
    {
        if(focusTaken)
        {
            focusTaken = false;
            inTransition = true;
            targetPosition = initialPosition;
            isTargetLocalPosition = true;
            targetZoom = initialZoom;
            startPosition = transform.localPosition;
            startZoom = Camera.main.orthographicSize;
            transitionTime = 0;
            focusLostDelegate?.Invoke();
        }
    }
}
