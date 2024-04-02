using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraSensitivity : MonoBehaviour
{
    //Inspector slot for Sense Slider
    public Slider sensitivitySlider;

    //Inspector slot for "baseCamera"
    public CinemachineFreeLook freeLookCamera;

    void Start()
    {
        //Set initial value of slider if no PlayerPrefs
        float defaultValue = (PlayerPrefs.HasKey("MouseSensitivity")) ? PlayerPrefs.GetFloat("MouseSensitivity") : 1f;
        sensitivitySlider.value = defaultValue;
    }

    // Update is called once per frame
    void Update()
    {
        //Save value of slider to PlayerPrefs
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);

        //Map the slider value to the range 100-500 for X
        float mappedValueX = 100 + (sensitivitySlider.value * 400);

        //Map the slider value to the range 0.5-2 for Y
        float mappedValueY = 0.5f + (sensitivitySlider.value * 1.5f);

        //Apply sensitivity to baseCamera X and Y Axis speed
        freeLookCamera.m_XAxis.m_MaxSpeed = mappedValueX;
        freeLookCamera.m_YAxis.m_MaxSpeed = mappedValueY;
    }
}