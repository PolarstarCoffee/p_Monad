using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTriggerStatic : MonoBehaviour
{
    // Declare the camera for this trigger in the inspector
    public CinemachineVirtualCamera fixedCamera;

    // Referance the CameraController script (Troubleshooting)
    //public CameraController cameraController;

    // Transition time
    public float transitionTime = 0f;

    // Declare Brain
    private CinemachineBrain brain;

    private void Awake()
    {
        //Set fixed Camera starting priority
        fixedCamera.Priority = 0;

    }

    private void Start()
    {
        // Smooth transition time
        brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = transitionTime;
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

        // Add camera and transition time to the CameraController (Troubleshooting)
        //cameraController.AddCamera(fixedCamera, transitionTime);
    }

    private void OnTriggerEnter(Collider Player)
    {
        // Switch to the camera associated with this trigger
        fixedCamera.Priority = 12;
    }

    private void OnTriggerExit(Collider Player)
    {
        // Lower the priority of the fixed camera
        fixedCamera.Priority = 0;
    }
}
