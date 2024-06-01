using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CursorVisibilityOn : MonoBehaviour
{
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
    private void Start()
    {
        Cursor.visible = true;

    }

    private void Update()
    {
       warpMouseLogic();

    }


    void warpMouseLogic()
    {
        //Get joystick pos
        leftstick = Gamepad.current.leftStick.ReadValue();
        //prevent jitter when not using joystick
        if (leftstick.magnitude < 0.1f) return;
        //get current mouse pOS to add to the joystick movement
        mousePOS = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //percise value for desired cursor pos
        warpPOS = mousePOS + bias + overflow + sensitivity * Time.deltaTime * leftstick;
        //keep cursor in the game screen 
        warpPOS = new Vector2(Mathf.Clamp(warpPOS.x, 0, Screen.width), Mathf.Clamp(warpPOS.y, 0, Screen.height));
        // Store floating point values so they are not lost in WarpCursorPosition (which applies FloorToInt)
        overflow = new Vector2(warpPOS.x % 1, warpPOS.y % 1);

        //Move cursor
        Mouse.current.WarpCursorPosition(warpPOS);
    }

}
