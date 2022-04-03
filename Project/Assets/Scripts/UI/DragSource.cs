using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSource : MonoBehaviour, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler
{
    public delegate DraggableUnit DraggedElementDelegate();
    public DragSource nextDragSource;
    public DraggedElementDelegate draggedElementPrefabDelegate;
    public DraggedElementDelegate inSlotElementPrefabDelegate;
    public System.Action dragStartedDelegate;
    public System.Action dragCanceledDelegate;
    public System.Action dragConfirmedDelegate;
    public DraggableUnit draggedElement;
    public LayerMask raycastMask;
    public float offsetFromGround = 3;
    public bool assignSource = true;

    void Awake()
    {
        nextDragSource = this;
    }

    void Update()
    {
        Debug.Log(draggedElement);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag called.");
    }
    public void OnDrag(PointerEventData data)
    {
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
            dragCanceledDelegate?.Invoke();
            Destroy(draggedElement.gameObject);

        }
    }

    public void OnInitializePotentialDrag(PointerEventData data)
    {
        Debug.Log("OnInitializePotentialDrag called.");
        DraggableUnit draggedElementPrefab = draggedElementPrefabDelegate();
        if(draggedElementPrefab != null)
        {
            draggedElement = Instantiate(draggedElementPrefab, transform.position, draggedElementPrefab.transform.rotation);
            draggedElement.source = nextDragSource;
            draggedElement.lastHolder = this;
            dragStartedDelegate?.Invoke();
        }
    }

    public void OnDropInTarget(DragTarget target)
    {
        if(inSlotElementPrefabDelegate != null)
        {
            dragConfirmedDelegate?.Invoke();
            target.OnContentReceived(inSlotElementPrefabDelegate(), nextDragSource, this);
            Destroy(draggedElement.gameObject);
            draggedElement = null;
        }
    }
    
}
