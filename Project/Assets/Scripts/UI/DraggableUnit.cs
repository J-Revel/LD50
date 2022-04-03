using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public int buildSpeed = 1;
    public int defense = 1;
    public DragSource source;
    public DragSource lastHolder;
}
