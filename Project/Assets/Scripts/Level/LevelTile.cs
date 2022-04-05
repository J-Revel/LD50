using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTile : MonoBehaviour
{
    public System.Action<int> castleBuiltDelegate;
    public System.Action<int> castleDestroyedDelegate;
    public Transform path;
    public Transform cameraTarget;
    public int sectionIndex;
}
