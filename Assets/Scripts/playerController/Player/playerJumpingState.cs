using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class playerJumpingState : playerBaseState
{
    //Jumping state (Also supports wall running detection). Directs player to buffer state when in freefall

    public Vector3 momentum;
    private readonly int JumpHash = Animator.StringToHash("Jump");
    private const float CrossFadeDuration = 0.1f;
    RaycastHit leftWallHit, rightWallHit;




    public playerJumpingState(playerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Player Jump");
        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        checkforWall();
        stateMachine.InputReader.boostEvent += onBoost;
        actualJump();
        AudioManager.Instance().PlaySound("Jump");
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movement = calculateMovement();
        Move(movement * stateMachine.freeRoamMovementSpeed, deltaTime);
        faceMovementDirection(movement, deltaTime);

        //Sprint checks
        if (stateMachine.InputReader.inputActions.player.Sprint.triggered)
        {
            stateMachine.sprintToggle = true;
        }
        if (stateMachine.InputReader.inputActions.player.Sprint.WasReleasedThisFrame())
        {
            stateMachine.sprintToggle = false;
        }
        //WallRunning calls//
        checkforWall();
        stateMachine.horizontalInput = Input.GetAxisRaw("Horizontal");
        stateMachine.verticalInput = Input.GetAxisRaw("Vertical");
        if ((stateMachine.wallLeft || stateMachine.wallRight) && stateMachine.verticalInput > 0 && aboveGround())
        {
            stateMachine.switchState(new wallRunningState(stateMachine));
            Debug.Log("Enter Wall Run");

        }
        //Grounding method 
        if (stateMachine.charController.velocity.y <= 0)
        {
            if (stateMachine.freeRoamMovementSpeed <= 20)
            {
                stateMachine.freeRoamMovementSpeed = 15;
            }

            stateMachine.switchState(new playerFallingState(stateMachine));
            return;

        }
    }
    public override void Exit()
    {
        Debug.Log("Player Leaving Jump");
        stateMachine.InputReader.boostEvent -= onBoost;
    }

    private void onBoost()
    {
        stateMachine.switchState(new playerBoostingState(stateMachine, stateMachine.InputReader.movementValue));
    }

    private void actualJump()
    {
        stateMachine.ForceReciever.jump(stateMachine.jumpForce);
        momentum.y = 0;
        momentum = stateMachine.charController.velocity;
    }

    private void faceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (movement != Vector3.zero)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamp);
        }

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
