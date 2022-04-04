using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StockDragSource : MonoBehaviour
{
    private DragSource dragSource;
    public DraggableUnit draggedElementPrefab;
    public DraggableUnit inSlotElementPrefab;
    public DraggableUnit droppedElementPrefab;

    public TMPro.TextMeshProUGUI text;
    public int stock = 5;
    public TimelineObstacle timelineObstacle;
    public static Dictionary<string, StockDragSource> instances = new Dictionary<string, StockDragSource>();
    public string id;
    
    
    private void Start()
    {
        instances[id] = this;
        dragSource = GetComponent<DragSource>();
        dragSource.dragStartedDelegate += OnDragStarted;
        dragSource.dragCanceledDelegate += OnDragCanceled;
        dragSource.dragConfirmedDelegate += OnDragConfirmed;
        dragSource.inSlotElementPrefabDelegate += () => {return inSlotElementPrefab;};
        dragSource.draggedElementPrefabDelegate += () => {return draggedElementPrefab;};
        dragSource.droppedElementPrefabDelegate += () => {return droppedElementPrefab;};
        dragSource.obstacleDataDelegate += () => {return timelineObstacle;};
        text.text = "x" + stock;
    }
    
    public void OnDragStarted()
    {
        stock -= 1;
        if(stock <= 0)
            dragSource.canTakeElement = false;
        text.text = "x" + stock;
    }

    public void OnDragCanceled()
    {

    }

    public void OnStockReceived(int count)
    {
        stock += count;
        text.text = "x" + stock;
        dragSource.canTakeElement = true;
    }

    public void OnDragConfirmed()
    {

    }
}
