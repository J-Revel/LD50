using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadowRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
