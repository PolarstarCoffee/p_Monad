using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorVisibilityOff : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;

        // Add method to the sceneLoaded delegate
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check the name of the loaded scene
        if (scene.name == "mainMenu_TestScene")
        {
            // If it's the main menu, hide the cursor
            Cursor.visible = false;
        }
        else
        {
            // If it's any other scene, show the cursor
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        // Remove method from the sceneLoaded delegate
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
