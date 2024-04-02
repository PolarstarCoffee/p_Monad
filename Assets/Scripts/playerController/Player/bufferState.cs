using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bufferState : playerBaseState
{
    /// 
    /// Buffer state for the player inbetween "falling"
    /// Used as a mediator between certain states that need to interact with one another 
    /// 
    public bufferState(playerStateMachine stateMachine) : base(stateMachine) { }
    public Vector3 movement;

    public override void Enter()
    {
        Debug.Log("Buffer");
        // stateMachine.InputReader.wallRun += onWallRunSwitch;
        Move(movement, Time.deltaTime);

        //perforce be on some bullshit 
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movement = calculateMovement();

        Move(movement * stateMachine.jumpForce, deltaTime);
        faceMovementDirection(movement, deltaTime);
        if (stateMachine.charController.isGrounded)
        {
            stateMachine.switchState(new playerFreeLookState(stateMachine));
        }
        //Sprint checks
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stateMachine.sprintToggle = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            stateMachine.sprintToggle = false;
        }
        ///Wall Running detection logic////////////////////////////////////
        //checkforWall();
        //stateMachine.horizontalInput = Input.GetAxisRaw("Horizontal");
        //stateMachine.verticalInput = Input.GetAxisRaw("Vertical");
        //if ((stateMachine.wallLeft || stateMachine.wallRight) && stateMachine.verticalInput > 0 && aboveGround() && !stateMachine.exitWall)
        //{
        //   stateMachine.switchState(new wallRunningState(stateMachine));
        //  Debug.Log("Wallrunning state switch");
        // }
        ///////////////////////////////////////////////////////////////////
    }
    public override void Exit()
    {
        // stateMachine.InputReader.wallRun -= onWallRunSwitch;
        Debug.Log("Buffer end");
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
    public void onWallRunSwitch()
    {
        stateMachine.switchState(new wallRunningState(stateMachine));
    }

    //Checks if there is a wall on either side of the player using raycasting (Checks again in wall running state)
    // public void checkforWall()
    //{
    //  stateMachine.wallRight = Physics.Raycast(stateMachine.charController.transform.position, stateMachine.orientation.right, out rightWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
    //stateMachine.wallLeft = Physics.Raycast(stateMachine.charController.transform.position, -stateMachine.orientation.right, out leftWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
    //}

    //checks if the player is at the correct height to walljump via raycasting
    public bool aboveGround()
    {
        return !Physics.Raycast(stateMachine.transform.position, Vector3.down, stateMachine.minJumpHeight, stateMachine.ground);
    }

}
