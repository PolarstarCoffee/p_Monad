using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStartText : MonoBehaviour
{
    public SpriteRenderer startButton;
    public SpriteRenderer rightArrow;
    public SpriteRenderer leftArrow;




    private void Start()
    {
        Debug.Log("EXISTS");
        // Make sure the spriteRenderer variable is assigned in the Inspector or find it by tag/name in code
        /* if (spriteRenderer == null)
         {
             spriteRenderer = GetComponent<SpriteRenderer>();
         }

         // Initially hide the sprite */
        startButton.enabled = false;
        rightArrow.enabled = false;
        leftArrow.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED");
        // Check if the entering object has a specific tag or component
        if (other.CompareTag("StartButton"))
        {
            // Show the sprite when the specified object enters the trigger
            startButton.enabled = true;
            rightArrow.enabled = true;
            leftArrow.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXITED");
        // Check if the exiting object has a specific tag or component
        if (other.CompareTag("StartButton"))
        {
            // Hide the sprite when the specified object exits the trigger
            startButton.enabled = false;
            rightArrow.enabled = false;
            leftArrow.enabled = false;
        }
    } 
}