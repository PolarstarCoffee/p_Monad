using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class playerFallingState : playerBaseState
{
    public playerFallingState(playerStateMachine stateMachine) : base(stateMachine)
    {
    }
    //Final State before going back to freeLook

    private readonly int FallingHash = Animator.StringToHash("Fall-Landing");
    private readonly int fallHash = Animator.StringToHash("Fall_Final");
    private const float CrossFadeDuration = 0.1f;
    private Vector3 momentum;
    RaycastHit leftWallHit, rightWallHit;
    public override void Enter()
    {
        Debug.Log("Falling");
        Move(momentum, Time.deltaTime);

        stateMachine.Animator.CrossFadeInFixedTime(fallHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        //Air control & Animation Calls 
        Vector3 movement = calculateMovement();
        stateMachine.Animator.CrossFadeInFixedTime(fallHash, CrossFadeDuration);
        Move(movement * stateMachine.jumpForce, deltaTime);
        faceMovementDirection(movement, deltaTime);
        if (stateMachine.charController.isGrounded)
        {
            AudioManager.Instance().PlaySound("Jump2");
            stateMachine.Animator.CrossFadeInFixedTime(FallingHash, CrossFadeDuration);
            stateMachine.switchState(new playerFreeLookState(stateMachine));
        }


        //Sprint checks
        if (stateMachine.InputReader.inputActions.player.Sprint.triggered)
        {
            stateMachine.sprintToggle = true;
        }
        if (stateMachine.InputReader.inputActions.player.Sprint.WasReleasedThisFrame())
        {
            stateMachine.sprintToggle = false;
        }
        if (stateMachine.InputReader.inputActions.player.Boost.triggered)
        {
            onBoost();
        }
        //Wall Run Checks 
        checkforWall();
        stateMachine.horizontalInput = Input.GetAxisRaw("Horizontal");
        stateMachine.verticalInput = Input.GetAxisRaw("Vertical");
        if ((stateMachine.wallLeft || stateMachine.wallRight) && stateMachine.verticalInput > 0 && aboveGround())
        {
            stateMachine.switchState(new wallRunningState(stateMachine));
            Debug.Log("Enter Wall Run");

        }
    }
    public override void Exit()
    {
        Debug.Log("Grounded");
        //stateMachine.InputReader.boostEvent -= onBoost;
    }
    private void faceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (movement != Vector3.zero)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamp);
        }

    }
    private void onBoost()
    {
        if (Time.time - stateMachine.previousBoostTime < stateMachine.boostCooldown) { return; }
        stateMachine.setBoostTime(Time.time);
        stateMachine.switchState(new playerBoostingState(stateMachine, stateMachine.InputReader.movementValue));

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






    /// 
    /// WALL RUNNING METHODS BELOW
    /// 


    //Checks if there is a wall on either side of the player using raycasting (Checks again in wall running state)
    public void checkforWall()
    {
        stateMachine.wallRight = Physics.Raycast(stateMachine.charController.transform.position, stateMachine.orientation.right, out rightWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
        stateMachine.wallLeft = Physics.Raycast(stateMachine.charController.transform.position, -stateMachine.orientation.right, out leftWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
    }

    //checks if the player is at the correct height to walljump via raycasting
    public bool aboveGround()
    {
        return !Physics.Raycast(stateMachine.transform.position, Vector3.down, stateMachine.minJumpHeight, stateMachine.ground);
    }
    //Switch to wallrunning state, where it handles all the forces being acted on the player whilst on a wall
    public void wallRunningSwitch()
    {
        stateMachine.switchState(new wallRunningState(stateMachine));
    }

}


