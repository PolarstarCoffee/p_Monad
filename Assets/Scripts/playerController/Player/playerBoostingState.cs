using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBoostingState : playerBaseState
{
    //Dash state 
    private Vector3 boostingDirectionInput;
    private float remainingBoostTime;
    RaycastHit leftWallHit, rightWallHit;
    private readonly int boostHash = Animator.StringToHash("Boost-Forward");
    private readonly int FallingHash = Animator.StringToHash("Fall-Landing");
    private const float CrossFadeDuration = 0.1f;

    public playerBoostingState(playerStateMachine stateMachine, Vector3 boostingDirection) : base(stateMachine)
    {
        this.boostingDirectionInput = boostingDirection;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(boostHash, CrossFadeDuration);
        AudioManager.Instance().PlaySound("Goost");
        if (stateMachine.freeRoamMovementSpeed >= 20)
        {
            stateMachine.freeRoamMovementSpeed = 15;
        }
        remainingBoostTime = stateMachine.boostDuration;
       
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * boostingDirectionInput.x * stateMachine.boostDistance / stateMachine.boostDuration;
        movement += stateMachine.transform.forward * boostingDirectionInput.y * stateMachine.boostDistance / stateMachine.boostDuration;
        Move(movement, deltaTime);
        faceMovementDirection(movement, deltaTime);
        remainingBoostTime -= deltaTime;

        if (remainingBoostTime <= 0f)
        {
            stateMachine.switchState(new bufferState(stateMachine));
            
        }

        //Wallrunning detection logic 
        checkforWall();
        stateMachine.horizontalInput = Input.GetAxisRaw("Horizontal");
        stateMachine.verticalInput = Input.GetAxisRaw("Vertical");
        if ((stateMachine.wallLeft || stateMachine.wallRight) && stateMachine.verticalInput > 0 && aboveGround())
        {
            wallRunningSwitch();
            Debug.Log("Enter Wall Run");

        }
    }
    public override void Exit()
    {

    }
    private void faceMovementDirection(Vector3 movement, float deltaTime)
    {
        if (movement != Vector3.zero)
        {
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltaTime * stateMachine.RotationDamp);
        }

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
