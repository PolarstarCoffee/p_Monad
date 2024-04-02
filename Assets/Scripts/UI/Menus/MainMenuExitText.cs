using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuExitText : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    

    private void Start()
    {
        Debug.Log("EXISTS");
        // Make sure the spriteRenderer variable is assigned in the Inspector or find it by tag/name in code
        /* if (spriteRenderer == null)
         {
             spriteRenderer = GetComponent<SpriteRenderer>();
         }

         // Initially hide the sprite */
        spriteRenderer.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED");
        // Check if the entering object has a specific tag or component
        if (other.CompareTag("ExitButton"))
        {
            // Show the sprite when the specified object enters the trigger
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("EXITED");
        // Check if the exiting object has a specific tag or component
        if (other.CompareTag("ExitButton"))
        {
            // Hide the sprite when the specified object exits the trigger
            spriteRenderer.enabled = false;
        }
    } 
}