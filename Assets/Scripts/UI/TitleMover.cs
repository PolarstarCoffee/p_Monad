using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMover : MonoBehaviour
{
    public Transform startPoint;    // The starting point
    public Transform endPoint;      // The ending point
    public float speed = 2.0f;      // The speed of movement

    private bool movingUp = true;   // Flag to check if the sprite is moving up or down

    void Update()
    {
        // Move the sprite between start and end points
        MoveSprite();
    }

    void MoveSprite()
    {
        // Calculate the new position based on the current direction and speed
        float step = speed * Time.deltaTime;

        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);

            // Check if the sprite has reached the end point
            if (Vector3.Distance(transform.position, endPoint.position) < 0.001f)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, step);

            // Check if the sprite has reached the start point
            if (Vector3.Distance(transform.position, startPoint.position) < 0.001f)
            {
                movingUp = true;
            }
        }
    }
}