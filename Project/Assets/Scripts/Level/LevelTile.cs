using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile : MonoBehaviour
{
    public System.Action<int> castleBuiltDelegate;
    public Transform path;
    public Transform cameraTarget;
    public int sectionIndex;
}
