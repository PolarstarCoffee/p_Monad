using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessSlider : MonoBehaviour
{
    public Slider brightnessSlider;

    private void Start()
    {
        // Set the slider's value to the current brightness at the start
        brightnessSlider.value = RenderSettings.ambientLight.grayscale;
        brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
    }

    public void ChangeBrightness(float value)
    {
        // Change the ambient light based on the slider's value
        RenderSettings.ambientLight = new Color(value, value, value, 1);
    }
}
