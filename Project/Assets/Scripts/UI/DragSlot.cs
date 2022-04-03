using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour
{
    private DragSource dragSource;
    public DragTarget dragTarget;
    
    
    private void Start()
    {
        dragSource = GetComponent<DragSource>();
        dragTarget = GetComponent<DragTarget>();
        dragTarget.contentReceivedDelegate += () => { dragSource.nextDragSource = dragTarget.containedUnit.source; };
        dragSource.assignSource = false;
        dragSource.dragStartedDelegate += OnDragStarted;
        dragSource.dragCanceledDelegate += OnDragCanceled;
        dragSource.dragConfirmedDelegate += OnDragConfirmed;
        dragSource.inSlotElementPrefabDelegate += () => {
            if(dragTarget.containedUnit != null)
                return dragTarget.containedUnit.source.inSlotElementPrefabDelegate();
            return dragSource.nextDragSource != dragSource ? dragSource.nextDragSource.inSlotElementPrefabDelegate() : null;
        };
        dragSource.draggedElementPrefabDelegate += () => {
            if(dragTarget.containedUnit != null)
                return dragTarget.containedUnit.source.draggedElementPrefabDelegate();
            return dragSource.nextDragSource != dragSource ? dragSource.nextDragSource.draggedElementPrefabDelegate() : null;
        };
    }
    
    public void OnDragStarted()
    {
        dragSource.draggedElement.source = dragTarget.containedUnit.source;
        dragTarget.OnContentTaken();
        dragSource.draggedElement.lastHolder = dragSource;
    }

    public void OnDragCanceled()
    {
        dragTarget.OnContentReceived(dragSource.draggedElement.lastHolder.inSlotElementPrefabDelegate(), dragSource.nextDragSource, dragSource);
    }

    public void OnDragConfirmed()
    {
    }
}
