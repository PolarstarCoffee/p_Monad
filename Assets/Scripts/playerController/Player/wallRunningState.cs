using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRunningState : playerBaseState
{
    //Wallrunning state, where the actual forces and logic for wallrunning are (As well as detection)
    //In editor, set wallrunning object layer mask to "wallrun_enable"
    public bool wallRun;
    RaycastHit rightWallHit;
    RaycastHit leftWallHit;
    Vector3 movement;

    private readonly int leftRunHash = Animator.StringToHash("WallRide-L_Final");
    private readonly int leftJumpHash = Animator.StringToHash("WallJump-L_Final");
    private readonly int rightRunHash = Animator.StringToHash("WallRide-R_Final");
    private readonly int rightJumpHash = Animator.StringToHash("WallJump-R_Final");
    private const float CrossFadeDuration = 0.1f;
    private readonly int freeFallHash = Animator.StringToHash("Fall_Final");

    public wallRunningState(playerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.sprintToggle = false;
        checkforWall();
    }
    public override void Tick(float deltaTime)
    {
        checkforWall();

        stateMachine.horizontalInput = Input.GetAxisRaw("Horizontal");
        stateMachine.verticalInput = Input.GetAxisRaw("Vertical");
        if ((stateMachine.wallLeft || stateMachine.wallRight) && stateMachine.verticalInput > 0 && aboveGround() && !stateMachine.exitWall)
        {
            //Animation calls (WALL RUNNING)
            if (stateMachine.wallLeft)
            {
                stateMachine.Animator.CrossFadeInFixedTime(leftRunHash, CrossFadeDuration);
            }
            if (stateMachine.wallRight)
            {
                stateMachine.Animator.CrossFadeInFixedTime(rightRunHash, CrossFadeDuration);
            }
            wallRunStart();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("jump here");
                stateMachine.StartCoroutine(wallJump());
            }


        }
        else if (stateMachine.exitWall)
        {
            if (stateMachine.wallRunning)
                wallRunStop();

            if (stateMachine.exitWallTimer > 0)
                stateMachine.exitWallTimer -= Time.deltaTime;

            if (stateMachine.exitWallTimer <= 0)
                stateMachine.exitWall = false;
        }
        else
        {
            if (stateMachine.wallRunning)
                wallRunStop();
        }

    }
    public override void Exit()
    {
        stateMachine.Animator.CrossFadeInFixedTime(freeFallHash, CrossFadeDuration);
        stateMachine.exitWall = false;

    }

    public void wallRunStart()
    {
        stateMachine.wallRunning = true;

        wallRunningMovement();
    }

    public void wallRunningMovement()
    {

        //storing vector 3 information on wallnormal (direction away from the wall)
        Vector3 wallNormal = stateMachine.wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, stateMachine.charController.transform.up);
        if ((stateMachine.orientation.forward - wallForward).magnitude > (stateMachine.orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        stateMachine.charController.Move(wallForward * stateMachine.wallrunForce * Time.fixedDeltaTime);

    }

    public void wallRunStop()
    {
        stateMachine.wallRunning = false;
        stateMachine.switchState(new playerFreeLookState(stateMachine));
    }

    public void checkforWall()
    {
        stateMachine.wallRight = Physics.Raycast(stateMachine.charController.transform.position, stateMachine.orientation.right, out rightWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
        stateMachine.wallLeft = Physics.Raycast(stateMachine.charController.transform.position, -stateMachine.orientation.right, out leftWallHit, stateMachine.wallCheckDistance, stateMachine.whatisWall);
    }
    public bool aboveGround()
    {
        return !Physics.Raycast(stateMachine.transform.position, Vector3.down, stateMachine.minJumpHeight, stateMachine.ground);
    }


    public IEnumerator wallJump()
    {
        stateMachine.exitWall = true;
        stateMachine.exitWallTimer = stateMachine.exitWallTime;
        Vector3 wallNormal = stateMachine.wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 forceToApplyDir = stateMachine.charController.transform.up * stateMachine.wallJumpUpForce + wallNormal * stateMachine.wallJumpUpSideForce;



        //Left side wall jump animation call
        if (stateMachine.wallLeft)
        {
            stateMachine.Animator.CrossFadeInFixedTime(leftJumpHash, CrossFadeDuration);
        }
        if (stateMachine.wallRight)
        {
            stateMachine.Animator.CrossFadeInFixedTime(rightJumpHash, CrossFadeDuration);
        }




        //Vector 3 lerp method

        Vector3 initalPOS = stateMachine.charController.transform.position;
        Vector3 targetPOS = initalPOS + forceToApplyDir;
        float elapsedTime = 0f;
        float smoothTime = 0.2f;
        while (elapsedTime < smoothTime)
        {
            stateMachine.charController.Move(Vector3.Lerp(initalPOS, targetPOS, elapsedTime / smoothTime) - stateMachine.charController.transform.position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Vector3 jumpZero = new Vector3();

        jumpZero = stateMachine.charController.velocity;
        //Move(movement * stateMachine.jumpForce, Time.deltaTime);





    }
}