using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IDropHandler
{
    public DraggableUnit containedUnit;
    public System.Action contentReceivedDelegate;
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("OnDrop called.");
        DragSource dragSource = data.pointerDrag.GetComponent<DragSource>();
        
        dragSource.OnDropInTarget(this);
    }
    public void OnContentReceived(DraggableUnit elementPrefab, DragSource source, DragSource lastHolder)
    {
        containedUnit = Instantiate(elementPrefab, transform.position, transform.rotation, transform);
        containedUnit.source = source;
        containedUnit.lastHolder = lastHolder;
        enabled = false;
        contentReceivedDelegate?.Invoke();
    }

    public void OnContentTaken()
    {
        enabled = true;
        Destroy(containedUnit.gameObject);
        containedUnit = null;
    }
}
