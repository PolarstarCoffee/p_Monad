using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFreeLookState : playerBaseState
//the freelook state is where the player resides in by default
//To add more states, create new state class, hook it up to playerBaseState
{
    //read only type is assigned at runtime and DOES NOT CHANGE 
    //animator.StingtoHash() is faster than setting a string
    //ints/floats are FASTER 
    private readonly int freeLookSpeedHash = Animator.StringToHash("freeLookSpeed");
    private float AnimatorDampTime = 0.1f;
    bool sprintToggle;

    public playerFreeLookState(playerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        //This method is called first (EVERY state works the same way. Enter, Tick, and Update)
        stateMachine.Animator.SetFloat(freeLookSpeedHash, 0, AnimatorDampTime, Time.deltaTime);
        //Subsribing to event 
        stateMachine.InputReader.boostEvent += onBoost;
        stateMachine.InputReader.jumpEvent += onJump;
        //Sprinting value checks 
        if (stateMachine.freeRoamMovementSpeed >= 9)
        {
            stateMachine.freeRoamMovementSpeed = 4;
        }

        Debug.Log("Enter Freelook");
    }
    public override void Tick(float deltaTime)
    {

        //Acts similarly to Update() 
        //It is called PER FRAME

        Vector3 movement = calculateMovement();
        //Uses playerbaseState move method
        Move(movement * stateMachine.freeRoamMovementSpeed, deltaTime);
        if (stateMachine.InputReader.movementValue == Vector2.zero)
        {
            //Communicating with the freeLookSpeed blend tree set up in Unity, and the playerStateMachine
            stateMachine.Animator.SetFloat(freeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        //Communicating with the freeLookSpeed blend tree set up in Unity, and the playerStateMachine
        stateMachine.Animator.SetFloat(freeLookSpeedHash, (float)0.5, AnimatorDampTime, deltaTime);
        faceMovementDirection(movement, deltaTime);
        //Debug.Log(stateMachine.InputReader.movementValue);

        sprint();


    }



    public override void Exit()
    {
        //As the player is leaving the state, This method gets called  
        //Unsubscribe 

        stateMachine.InputReader.boostEvent -= onBoost;
        stateMachine.InputReader.jumpEvent -= onJump;
        Debug.Log("Exit");
    }

    private Vector3 calculateMovement()
    {
        //grab camera forward and right vectors *BE SURE TO NORMALIZE*
        Vector3 forwardCamera = stateMachine.mainCameraTransform.forward;
        Vector3 rightCamera = stateMachine.mainCameraTransform.right;
        forwardCamera.y = 0f;
        rightCamera.y = 0f;
        forwardCamera.Normalize();
        rightCamera.Normalize();


        //Calculation for actual movement 
        return forwardCamera * stateMachine.InputReader.movementValue.y + rightCamera * stateMachine.InputReader.movementValue.x;
    }

    private void faceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (movement != Vector3.zero)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamp);
        }

    }

    //Process for changing states (Subscribe, unsubscribe, method to switch to state class)
    private void onBoost()
    {
        if (Time.time - stateMachine.previousBoostTime < stateMachine.boostCooldown) { return; }
        stateMachine.setBoostTime(Time.time);
        if (stateMachine.InputReader.movementValue.y > 0)
        {
            stateMachine.switchState(new playerBoostingState(stateMachine, stateMachine.InputReader.movementValue));
        }
    }
    private void onJump()
    {
        stateMachine.switchState(new playerJumpingState(stateMachine));
    }
     public void sprint()
    {
        if (stateMachine.InputReader.inputActions.player.Sprint.triggered || stateMachine.sprintToggle == true)
        {
            stateMachine.sprintToggle = true;
            stateMachine.freeRoamMovementSpeed = stateMachine.sprintSpeed;
            stateMachine.Animator.SetFloat(freeLookSpeedHash, 1, AnimatorDampTime, Time.deltaTime);
        }
        if (stateMachine.InputReader.inputActions.player.Sprint.WasReleasedThisFrame() || stateMachine.sprintToggle == false)
        {
            stateMachine.sprintToggle = false;
            stateMachine.freeRoamMovementSpeed = 4;
        }
    }

}
