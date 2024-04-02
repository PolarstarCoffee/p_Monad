using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_Anything : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public bool direction;

    void Update()
    {
        if (direction == true)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
            transform.localRotation = rotation * transform.localRotation;
        }
        if (direction == false)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.down);
            transform.localRotation = rotation * transform.localRotation;
        }
    }
}
