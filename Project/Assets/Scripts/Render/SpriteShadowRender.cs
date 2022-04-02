using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowRender : MonoBehaviour
{
    void Start()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    void Update()
    {
        
    }
}
