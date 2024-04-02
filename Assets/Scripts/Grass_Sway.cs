using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass_Sway : MonoBehaviour
{
    public float swayAmount = 1.0f;  // Adjust the sway amount based on the desired intensity
    public float swaySpeed = 1.0f;   // Adjust the sway speed based on the desired frequency

    private void Update()
    {
        // Calculate the sway based on sine function
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        // Apply the sway to the object's rotation
        Quaternion swayRotation = Quaternion.Euler(0f, sway, 0f);
        transform.localRotation = swayRotation;
    }
}