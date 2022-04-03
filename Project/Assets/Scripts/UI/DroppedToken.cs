using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedToken : MonoBehaviour
{
    private DraggableUnit unit;
    public float duration = 2;
    private float time = 0;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    void Start()
    {
        unit = GetComponent<DraggableUnit>();
        
        startPosition = transform.position;
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 target = Camera.main.ScreenToWorldPoint(unit.source.transform.position);
        
        if(Physics.Raycast(target, Camera.main.transform.forward, out hit))
        {
            targetPosition = hit.point;
        }
        time += Time.deltaTime;
        float animRatio = (time / duration) * (time / duration);
        transform.position = Vector3.Lerp(startPosition, targetPosition, animRatio);
        if(time >= duration)
        {
            Destroy(gameObject);
            unit.source.GetComponent<StockDragSource>().OnStockReceived(1);
        }
    }
}
