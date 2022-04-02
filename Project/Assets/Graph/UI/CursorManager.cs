using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public float vectorA = 0;
    public float vectorB = 0;
    public Texture2D cursorTex;
    //Vector2 hotSpot = new Vector2(vectorA, vectorB);
   
    void Awake() {

        Vector2 hotSpot = new Vector2(vectorA, vectorB);
        Cursor.SetCursor(cursorTex, hotSpot, CursorMode.ForceSoftware); }

  
}