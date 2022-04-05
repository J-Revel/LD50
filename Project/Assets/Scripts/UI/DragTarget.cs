using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTarget : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public DraggableUnit containedUnit;
    public Transform container;
    public System.Action contentReceivedDelegate;
    public GameObject hoverElement;
    public void OnDrop(PointerEventData data)
    {
        Debug.Log("OnDrop called.");
        DragSource dragSource = data.pointerDrag.GetComponent<DragSource>();
        
        dragSource.OnDropInTarget(this);
    }
    public void OnContentReceived(DraggableUnit elementPrefab, DragSource source, DragSource lastHolder)
    {
        containedUnit = Instantiate(elementPrefab, transform.position, transform.rotation, container != null ? container : transform);
        containedUnit.source = source;
        containedUnit.lastHolder = lastHolder;
        enabled = false;
        contentReceivedDelegate?.Invoke();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        hoverElement?.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        hoverElement?.SetActive(false);
    }

    public void OnContentTaken()
    {
        enabled = true;
        Destroy(containedUnit.gameObject);
        containedUnit = null;
    }

    public void DropContent()
    {
        if(containedUnit == null)
            return;
        DraggableUnit prefab = containedUnit.source.droppedElementPrefabDelegate();
        Instantiate(prefab, transform.position, prefab.transform.rotation).source = containedUnit.source;
        Destroy(containedUnit.gameObject);
        containedUnit = null;
        enabled = true;
    }
}
