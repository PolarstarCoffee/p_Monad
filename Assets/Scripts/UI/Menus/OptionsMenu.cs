using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    private bool isOptionsMenuActive = false;
    private bool isControlsMenuActive = false;
    EventSystem EventSystem;

   

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance().PlayMusic("MenuMusic");

        
    }

    public void OpenControls()
    {
        controlsMenuUI.SetActive(true);
        isControlsMenuActive = true;
        optionsMenuUI.SetActive(false);
        EventSystem = EventSystem.current;  
    }

    public void BackFromControlsMenu()
    {
        controlsMenuUI.SetActive(false);
        isControlsMenuActive = false;
        optionsMenuUI.SetActive(true);  

    }
}
