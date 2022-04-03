using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteDragSource : MonoBehaviour
{
    private DragSource dragSource;
    public DraggableUnit draggedElementPrefab;
    public DraggableUnit inSlotElementPrefab;
    
    
    private void Start()
    {
        dragSource = GetComponent<DragSource>();
        dragSource.dragStartedDelegate += OnDragStarted;
        dragSource.dragCanceledDelegate += OnDragCanceled;
        dragSource.dragConfirmedDelegate += OnDragConfirmed;
        dragSource.inSlotElementPrefabDelegate += () => {return inSlotElementPrefab;};
        dragSource.draggedElementPrefabDelegate += () => {return draggedElementPrefab;};
    }
    
    public void OnDragStarted()
    {

    }

    public void OnDragCanceled()
    {

    }

    public void OnDragConfirmed()
    {

    }
}
