using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Loading : MonoBehaviour
{
    // Array to hold the GameObjects to be loaded
    public GameObject[] objectsToLoad;

    // List to keep track of instantiated GameObjects
    private List<GameObject> instantiatedObjects = new List<GameObject>();

    // Method called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Loop through each GameObject in the array
            foreach (GameObject obj in objectsToLoad)
            {
                // Instantiate the GameObject at its original position and rotation
                GameObject instantiatedObj = Instantiate(obj, obj.transform.position, obj.transform.rotation);
                // Ensure the instantiated GameObject is active
                instantiatedObj.SetActive(true);
                // Add the instantiated object to the list
                instantiatedObjects.Add(instantiatedObj);
            }
        }
    }

    // Method called when another collider exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Loop through each instantiated GameObject in the list
            foreach (GameObject instantiatedObj in instantiatedObjects)
            {
                // Destroy the instantiated GameObjects
                Destroy(instantiatedObj);
            }
            // Clear the list of instantiated objects
            //instantiatedObjects.Clear();
        }
    }
}
