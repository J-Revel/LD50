using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSource : MonoBehaviour, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    public delegate DraggableUnit DraggedElementDelegate();
    public delegate TimelineObstacle TimelineObstacleDelegate();
    public DragSource nextDragSource;
    public DraggedElementDelegate draggedElementPrefabDelegate;
    public DraggedElementDelegate inSlotElementPrefabDelegate;
    public DraggedElementDelegate droppedElementPrefabDelegate;
    public TimelineObstacleDelegate obstacleDataDelegate;
    public System.Action dragStartedDelegate;
    public System.Action dragCanceledDelegate;
    public System.Action dragConfirmedDelegate;
    public DraggableUnit draggedElement;
    public LayerMask raycastMask;
    public float offsetFromGround = 3;
    public bool assignSource = true;
    public bool canTakeElement = true;

    void Awake()
    {
        nextDragSource = this;
    }

    void Update()
    {
    }

    public void OnBeginDrag(PointerEventData data)
    {
        DraggableUnit draggedElementPrefab = draggedElementPrefabDelegate();
        if(draggedElementPrefab != null && canTakeElement)
        {
            draggedElement = Instantiate(draggedElementPrefab, transform.position, draggedElementPrefab.transform.rotation);
            draggedElement.source = nextDragSource;
            draggedElement.lastHolder = this;
            dragStartedDelegate?.Invoke();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000, raycastMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 3);
                draggedElement.transform.position = hit.point - ray.direction * offsetFromGround;
            }
        }
        else draggedElement = null;
    }
    public void OnDrag(PointerEventData data)
    {
        if(draggedElement == null)
            return;
        Debug.Log("OnDrag called.");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, raycastMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 3);
            draggedElement.transform.position = hit.point - ray.direction * offsetFromGround;
        }
        else Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 3);
    }
    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag called.");
        if(draggedElement != null)
        {
            DraggableUnit prefab = draggedElement.source.droppedElementPrefabDelegate();
            Instantiate(prefab, draggedElement.transform.position, prefab.transform.rotation).source = draggedElement.source;
            dragCanceledDelegate?.Invoke();
            Destroy(draggedElement.gameObject);

        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        DraggableUnit draggedElementPrefab = draggedElementPrefabDelegate();
        if(draggedElementPrefab != null && canTakeElement)
        {
            draggedElement = Instantiate(draggedElementPrefab, transform.position, draggedElementPrefab.transform.rotation);
            draggedElement.source = nextDragSource;
            draggedElement.lastHolder = this;
            dragStartedDelegate?.Invoke();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000, raycastMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green, 3);
                draggedElement.transform.position = hit.point - ray.direction * offsetFromGround;
            }
        }
        else draggedElement = null;
    }

    public void OnInitializePotentialDrag(PointerEventData data)
    {
    }

    public void OnDropInTarget(DragTarget target)
    {
        if(draggedElement != null && inSlotElementPrefabDelegate != null)
        {
            dragConfirmedDelegate?.Invoke();
            target.OnContentReceived(inSlotElementPrefabDelegate(), nextDragSource, this);
            Destroy(draggedElement.gameObject);
            draggedElement = null;
        }
    }
    
}
