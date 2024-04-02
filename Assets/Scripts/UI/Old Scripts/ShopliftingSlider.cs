using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLiftingSlider : MonoBehaviour
{
    public Slider greenSlider; 

    void Start()
    {
        // Add a listener to the slider's onValueChanged event
        greenSlider.onValueChanged.AddListener(ChangeGreenValue);
    }

    void ChangeGreenValue(float value)
    {
        // Get a reference to all renderers
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in allRenderers)
        {
            // Get the current color
            Color currentColor = renderer.material.color;

            // Set the green value to the slider's value
            currentColor.g = value;

            // Apply the new color
            renderer.material.color = currentColor;
        }
    }
}
