using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CameraFOV : MonoBehaviour
{
    //Slider Inspector Slot
    public Slider fovSlider; 
    //baseCamera inspector Slot
    public CinemachineFreeLook freeLookCamera; 

    private void Start()
    {
        //Max Value
        fovSlider.minValue = 20;
        //Min Value
        fovSlider.maxValue = 80;

        // Set the initial value of the slider to match the camera's FOV
        fovSlider.value = freeLookCamera.m_Lens.FieldOfView;

        // Add a listener to the slider so that the FOV updates when the slider value changes
        fovSlider.onValueChanged.AddListener(UpdateFOV);
    }

    private void UpdateFOV(float newFOV)
    {
        // Update the FOV of the camera when the slider value changes
        freeLookCamera.m_Lens.FieldOfView = newFOV;
    }
}
