using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    public CinemachineFreeLook freeLookCamera;
    private bool isOptionsMenuActive = false;
    private bool isControlsMenuActive = false;

    private void Start()
    {
        // Enable the camera input
        // Get the sensitivity from PlayerPrefs
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", (280f - 100f) / 400f);
        // Map the sensitivity to the range 100-500 for X-axis
        float mappedValueX = 100 + (sensitivity * 400);
        // Map the sensitivity to the range 0.5-1.5 for Y-axis
        float mappedValueY = 0.5f + (sensitivity * 1.0f);
        // Apply the mapped sensitivity to the camera speed
        freeLookCamera.m_XAxis.m_MaxSpeed = mappedValueX;
        freeLookCamera.m_YAxis.m_MaxSpeed = mappedValueY; 
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (GameIsPaused)
            {
                if (isOptionsMenuActive)
                {
                    BackToPauseMenu();
                }
                else if (isControlsMenuActive) 
                {
                    BackFromControlsMenu();
                }
                else
                {
                    Resume();
                }
            }
            else if (!isOptionsMenuActive && !isControlsMenuActive) 
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerPrefs.Save();

        //Lock the cursor on resume
        Cursor.lockState = CursorLockMode.Locked;
        //Hide the cursor on resume
        Cursor.visible = false;

        // Enable the camera input
        // Get the sensitivity from PlayerPrefs
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", (280f - 100f) / 400f);
        // Map the sensitivity to the range 100-500 for X-axis
        float mappedValueX = 100 + (sensitivity * 400);
        // Map the sensitivity to the range 0.5-1.5 for Y-axis
        float mappedValueY = 0.5f + (sensitivity * 1.0f);
        // Apply the mapped sensitivity to the camera speed
        freeLookCamera.m_XAxis.m_MaxSpeed = mappedValueX;
        freeLookCamera.m_YAxis.m_MaxSpeed = mappedValueY; 

    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayerPrefs.Save();

        // Disable the camera input
        freeLookCamera.m_XAxis.m_MaxSpeed = 0; 
        freeLookCamera.m_YAxis.m_MaxSpeed = 0; 

    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Application.Quit();
        PlayerPrefs.Save();
    }

    public void OpenOptions() 
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUi.SetActive(false);
        isOptionsMenuActive = true;

        // Disable the camera input
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;

    }

    public void OpenControls()
    {
        controlsMenuUI.SetActive(true);
        pauseMenuUi.SetActive(false);
        isControlsMenuActive = true;

        // Disable the camera input
        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }

    public void BackFromControlsMenu()
    {
        controlsMenuUI.SetActive(false);
        pauseMenuUi.SetActive(true);
        isControlsMenuActive = false;

        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
    }

    public void BackToPauseMenu()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUi.SetActive(true);
        isOptionsMenuActive = false;

        freeLookCamera.m_XAxis.m_MaxSpeed = 0;
        freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        
    }
}
