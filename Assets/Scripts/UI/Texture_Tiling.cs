using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_Tiling : MonoBehaviour
{
    public float xTiling = 1.0f; // Set this to the desired tiling factor

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material; // Creates a new instance of the material
            if (material != null)
            {
                Vector2 textureScale = material.mainTextureScale;
                textureScale.x = xTiling; // Adjust the X tiling
                material.mainTextureScale = textureScale;
            }
        }
    }
}
