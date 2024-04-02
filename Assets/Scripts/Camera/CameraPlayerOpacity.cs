using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerOpacity : MonoBehaviour
{
    //Base Player Material
    public Material OpaqueMaterial;

    //Transparent Player Material
    public Material TransparentMaterial;

    //Transparency Threshold between Player and Camera
    public float TransparencyThreshold = 1.0f;

    //Private Renderer
    private Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);

        if (distanceToCamera < TransparencyThreshold)
        {
            myRenderer.material = TransparentMaterial;
        }
        else
        {
            myRenderer.material = OpaqueMaterial;
        }
    }
}
