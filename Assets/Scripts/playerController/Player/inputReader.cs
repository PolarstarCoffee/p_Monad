using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputReader : MonoBehaviour, Controls.IPlayerActions
{
    //Implemented input action asset as an interfa.ce *shift + period hotkey* 
    //The inputs that unity would handle, and use our state machine to switch between states 
    //i.e. input events drive the state machine
    //if you need to write any logic that stops our player from moving, here is where you do it.

    public Controls inputActions;
    public event Action jumpEvent;
    public event Action sprintEvent;
    public event Action boostEvent;
    public event Action wallRun;
    private bool alreadyHit = false;
    private dialogueCollisionSaver collisionSaver;

    //Vector 2 movement variable *Consistently reads movement value. Reads it publicly, yet sets privately 
    public Vector2 movementValue { get; private set; }  
    void Start()
    {
        collisionSaver = GetComponent<dialogueCollisionSaver>();
        //initalizing the input asset at start
        inputActions = new Controls();
        //sets the callback methods (connects moreso)
        inputActions.player.SetCallbacks(this);
        inputActions.player.Enable();
        DialogueDisplay.Instance().OnDialogueCompletion.AddListener(convoEnd);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && collisionSaver.GetLastCollidedNPC() != null)
        {
            collisionSaver.GetLastCollidedNPC().GetComponent<NPCDialogueManager>().StartConversation();
            convoStart();
        }
    }

    //Disables action inputs when player is destroyed
    private void OnDestroy()
    {
        inputActions.player.Disable();
    }
    
    //Jump event
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        jumpEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //sets movement value (x, y or z) 
        movementValue = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
       
        if (!context.performed) {return;}
        sprintEvent?.Invoke();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        boostEvent?.Invoke();
    }
    public void wallRunning(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        wallRun?.Invoke();
    }

    private void convoStart()
    {
        inputActions.player.Disable();
        alreadyHit = true;

    }

    private void convoEnd()
    {
        inputActions.player.Enable();
        alreadyHit = false;
    }
    
}
