using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUi;
    public GameObject optionsMenuUI;
    public GameObject controlsMenuUI;
    
    public CinemachineFreeLook freeLookCamera;
    private bool isOptionsMenuActive = false;
    private bool isControlsMenuActive = false;
    public Controls inputActions;



    //ADDED ADDITONAL LOGIC FOR CURSOR MOVEMENT WITH LEFT JOYSTICK
    [Tooltip("Higher numbers for more mouse movement on joystick press." +
             "Warning: diagonal movement lost at lower sensitivity (<1000)")]
    public Vector2 sensitivity = new Vector2(1500f, 1500f);
    [Tooltip("Counteract tendency for cursor to move more easily in some directions")]
    public Vector2 bias = new Vector2(0f, -1f);
    //cached variables
    Vector2 leftstick;
    Vector2 mousePOS;
    Vector2 warpPOS;
    Vector2 overflow;




    private void Awake()
    {
        inputActions = new Controls();
        //Fancy lambda expression, giving context to the action event 
        inputActions.UI.Pause.performed += ctx => PMenu();
        
    }
    //Enable input actions
    void OnEnable()
    {
        inputActions.UI.Enable();
    }
    void OnDisable()
    {
        inputActions.UI.Disable();
    }

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
     if (GameIsPaused)
        {
            inputActions.player.Disable();
            warpMouseLogic();
        }
       
    }


    public void PMenu()
    {
        Cursor.lockState= CursorLockMode.None;
        Cursor.visible = true;
        inputActions.player.Disable();
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

    public void Resume()
    {
        
        pauseMenuUi.SetActive(false);
        inputActions.player.Enable();
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
        warpMouseLogic();
        Time.timeScale = 0f;
        GameIsPaused = true;
        inputActions.player.Disable();   
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
    void warpMouseLogic()
    {
        if (Gamepad.current.leftStick == null)
        {
            return;
        }
        else
        {
            //Get joystick pos
            leftstick = Gamepad.current.leftStick.ReadValue();
        }
        //prevent jitter when not using joystick
        if (leftstick.magnitude < 0.1f) return;
        //get current mouse pOS to add to the joystick movement
        mousePOS = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //percise value for desired cursor pos
        warpPOS = mousePOS + bias + overflow + sensitivity * Time.unscaledDeltaTime * leftstick;
        //keep cursor in the game screen 
        warpPOS = new Vector2(Mathf.Clamp(warpPOS.x, 0, Screen.width), Mathf.Clamp(warpPOS.y, 0, Screen.height));
        // Store floating point values so they are not lost in WarpCursorPosition (which applies FloorToInt)
        overflow = new Vector2(warpPOS.x % 1, warpPOS.y % 1);

        //Move cursor
        Mouse.current.WarpCursorPosition(warpPOS);
    }
}
