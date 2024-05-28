using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotate : MonoBehaviour
{
    // Assign your sphere in the inspector
    public GameObject sphere;

    // Rotation amount in degrees
    private float rotationAmount = 90.0f;

    //Rotation Duration
    private float rotationDuration = 1.0f;

    //Input lock (Check if sphere is rotating)
    private bool isRotating = false;
    public Controls controls;

    void Update()
    {
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Start rotating the sphere 90 degrees to the left
                StartCoroutine(RotateOverTime(sphere, Vector3.up, -rotationAmount, rotationDuration));

                //Play Sound on "A" key press
                AudioManager.Instance().PlaySound("A");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // Start rotating the sphere 90 degrees to the right
                StartCoroutine(RotateOverTime(sphere, Vector3.up, rotationAmount, rotationDuration));

                //Play Sound on "D" key press
                AudioManager.Instance().PlaySound("D");
            }
        }
    }

    IEnumerator RotateOverTime(GameObject target, Vector3 axis, float angle, float duration)
    {
        isRotating = true;
        Quaternion startRotation = target.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(axis * angle);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            target.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }

        // Ensure the rotation is exactly as expected at the end
        target.transform.rotation = endRotation;
        isRotating = false;
    }
}