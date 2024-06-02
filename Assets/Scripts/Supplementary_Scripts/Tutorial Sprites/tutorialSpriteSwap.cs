using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class tutorialSpriteSwap : MonoBehaviour
{
    public string inactiveTag = "keyboard";  // Tag for objects to deactivate
    public string activeTag = "gamepad";      // Tag for objects to activate

    private bool isGamepadConnected;

    void Update()
    {
        // Initial check for gamepad connection
        CheckForGamepad();
        Debug.Log(Gamepad.all.Count);
        // Start the coroutine to check for gamepad connection periodically
        
    }

    void CheckForGamepad()
    {
        // Check if a gamepad is connected
        if (Gamepad.all.Count == 1)
        {
           
            ActivateObjectsWithTag(activeTag);
            DeactivateObjectsWithTag(inactiveTag);
        }
        
        if (Gamepad.all.Count == 0)
        {
            DeactivateObjectsWithTag(activeTag);
            ActivateObjectsWithTag(inactiveTag);
        }
            
        
    }

    

    void ActivateObjectsWithTag(string tag)
    {
        GameObject[] objectsToActivate = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }

    void DeactivateObjectsWithTag(string tag)
    {
        GameObject[] objectsToDeactivate = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
    }
}
