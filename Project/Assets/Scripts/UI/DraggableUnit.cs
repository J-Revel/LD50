using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public int buildSpeed = 1;
    public float defense = 0.2f;
    public DragSource source;
    public DragSource lastHolder;
}
