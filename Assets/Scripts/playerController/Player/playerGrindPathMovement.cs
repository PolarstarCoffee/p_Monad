using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem.LowLevel;

public class playerGrindPathMovement : playerBaseState
{
    public playerGrindPathMovement(playerStateMachine stateMachine) : base(stateMachine)
    {
    }
    float timeforFullSpline;
    float elapsedTime;
    float lerpSpeed = 10f;
    private readonly int grindHash = Animator.StringToHash("Grind_Final");
    private readonly int jumpHash = Animator.StringToHash("GrindJump_Final");
    private readonly int locomotionHash = Animator.StringToHash("freeLookSpeed");
    private readonly int fallHash = Animator.StringToHash("Fall_Final");
    private float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    int loopG = AudioManager.Instance().PlaySoundAsLoop("grind");

    public override void Enter()
    {
        Debug.Log("grind path detected");
        //return sprint bool to false
        stateMachine.sprintToggle = false;
        //disable player actions upon entering state aside from camera movement 
        stateMachine.InputReader.inputActions.player.Disable();
        stateMachine.onRail = true;
        calculateAndSetRailPOS();
        stateMachine.Animator.CrossFadeInFixedTime(grindHash, AnimatorDampTime);
       
    }


    public override void Tick(float deltaTime)
    {
        if (stateMachine.onRail)
        {

            movePlayerAlongRail();
            stateMachine.Animator.CrossFadeInFixedTime(grindHash, AnimatorDampTime);
            stateMachine.InputReader.inputActions.player.Enable();
            if (stateMachine.InputReader.inputActions.player.Jump.IsPressed())
            {

                jumpOffRail();

            }

        }
        //If the player presses space to jump off of the bezier curve, they switch states
        //this might not work (It does)
       
        if (stateMachine.grindExit)
        {
            if (stateMachine.onRail)
            {
                throwOffRail();
            }
            if (stateMachine.grindExitTimer >= 0)
            {
                stateMachine.grindExitTimer -= Time.deltaTime;
            }
            if (stateMachine.grindExitTimer <= 0)
            {
                stateMachine.grindExit = false;
            }
        }
    }

    public override void Exit()
    {
        //  rotationReset();
        Debug.Log("Exit Rail");
        stateMachine.onRail = false;
        //Enable input action asset upon exiting the state 
        stateMachine.InputReader.inputActions.player.Enable();
        AudioManager.Instance().StopLoop(loopG);


    }



    void movePlayerAlongRail()
    {
        if (stateMachine.gameObj_RailScript != null && stateMachine.onRail)
        {
            //Calculate a 0 to 1 normalised time value which is the progress along the rail.
            //Elapsed time divided by the full time needed to traverse the spline will give you that value.
            float progress = elapsedTime / timeforFullSpline;


            //If progress is less than 0, the player's position is before the start of the rail.
            //If greater than 1, their position is after the end of the rail.
            //In either case, the player has finished their grind.
            if (progress <= 0 || progress >= 1)
            {
                stateMachine.StartCoroutine(throwOffRail());

                //throwOffRail();
                return;
            }
            //The rest of this code will not execute if the player is thrown off.

            //Next Time Normalised is the player's progress value for the next update.
            //This is used for calculating the player's rotation.
            //Depending on the direction of the player on the spline, it will either add or subtract time from the
            //current elapsed time.
            float nextTimeNoramlized;
            if (stateMachine.gameObj_RailScript.normalDir)
                nextTimeNoramlized = (elapsedTime + Time.deltaTime) / timeforFullSpline;
            else
                nextTimeNoramlized = (elapsedTime - Time.deltaTime) / timeforFullSpline;
            //Calculating the local positions of the player's current position and next position
            //using current progress and the progress for the next update.
            float3 pos, tangent, up;
            float3 nextPosfloat, nextTan, nextUp;
            SplineUtility.Evaluate(stateMachine.gameObj_RailScript.railSpline.Spline, progress, out pos, out tangent, out up);
            SplineUtility.Evaluate(stateMachine.gameObj_RailScript.railSpline.Spline, nextTimeNoramlized, out nextPosfloat, out nextTan, out nextUp);

            //Converting the local positions into world positions.
            Vector3 worldPos = stateMachine.gameObj_RailScript.LocalToWorldConversion(pos);
            Vector3 nextPos = stateMachine.gameObj_RailScript.LocalToWorldConversion(nextPosfloat);

            //Setting the player's position and adding a height offset so that they're sitting on top of the rail
            //instead of being in the middle of it.
            stateMachine.transform.position = worldPos + (stateMachine.transform.up * stateMachine.heightOffset);
            //Lerping the player's current rotation to the direction of where they are to where they're going.
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(nextPos - worldPos), lerpSpeed * Time.deltaTime);
            //Lerping the player's up direction to match that of the rail, in relation to the player's current rotation.
            stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.FromToRotation(stateMachine.transform.up, up) * stateMachine.transform.rotation, lerpSpeed * Time.deltaTime);

            //Finally incrementing or decrementing elapsed time for the next update based on direction.
            if (stateMachine.gameObj_RailScript.normalDir)
                elapsedTime += Time.deltaTime;
            else
                elapsedTime -= Time.deltaTime;
        }
    }







    void calculateAndSetRailPOS()
    {
        //Figure out the amount of time it would take for the player to cover the rail.
        timeforFullSpline = stateMachine.gameObj_RailScript.totalSplineLength / stateMachine.grindSpeed;

        //This is going to be the world position of where the player is going to start on the rail.
        Vector3 splinePoint;

        //The 0 to 1 value of the player's position on the spline. We also get the world position of where that
        //point is.
        float normalizedTime = stateMachine.gameObj_RailScript.CalculateTargetRailPoint(stateMachine.transform.position, out splinePoint);
        elapsedTime = timeforFullSpline * normalizedTime;
        //Multiply the full time for the spline by the normalised time to get elapsed time. This will be used in
        //the movement code.

        //Spline evaluate takes the 0 to 1 normalised time above, 
        //and uses it to give you a local position, a tangent (forward), and up
        float3 pos, forward, up;
        SplineUtility.Evaluate(stateMachine.gameObj_RailScript.railSpline.Spline, normalizedTime, out pos, out forward, out up);
        //Calculate the direction the player is going down the rail
        stateMachine.gameObj_RailScript.CalculateDirection(forward, stateMachine.transform.forward);
        //set player's inital pos on the rail before starting the movement code
        stateMachine.transform.position = splinePoint + (stateMachine.charController.transform.up * stateMachine.heightOffset);
    }

    public IEnumerator throwOffRail()
    {
        stateMachine.onRail = false;
        stateMachine.gameObj_RailScript = null;
        stateMachine.grindExit = true;
        stateMachine.grindExitTimer = stateMachine.exitWallTime;
        //rotationReset();
        stateMachine.Animator.CrossFadeInFixedTime(fallHash, CrossFadeDuration);

        //push player off by endOff
        Vector3 initalPOS = stateMachine.charController.transform.position;
        Vector3 targetPOS = initalPOS + stateMachine.charController.transform.forward * stateMachine.endOff;
        float elapsedTime = 0f;
        float smoothTime = 0.03f;
        while (elapsedTime < smoothTime)
        {
            stateMachine.charController.transform.position = Vector3.Lerp(initalPOS, targetPOS, elapsedTime / smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        stateMachine.charController.transform.position = targetPOS;
        stateMachine.Animator.CrossFadeInFixedTime(fallHash, CrossFadeDuration);

        //stateMachine.transform.position = stateMachine.transform.forward * stateMachine.endOff;

        stateMachine.switchState(new playerFreeLookState(stateMachine));

    }

    void jumpOffRail()
    {
        stateMachine.grindExit = true;
        stateMachine.grindExitTimer = stateMachine.exitWallTime;
        stateMachine.onRail = false;
        stateMachine.gameObj_RailScript = null;
        stateMachine.ForceReciever.jump(stateMachine.jumpForce);
        stateMachine.Animator.CrossFadeInFixedTime(jumpHash, AnimatorDampTime);
        stateMachine.onRail = false;
        stateMachine.gameObj_RailScript = null;
        rotationReset();
        stateMachine.switchState(new playerFreeLookState(stateMachine));
    }

    //Rotation reset method attempt to fix bug with grinding 
    public void rotationReset()
    {
        // Vector3 tempRoation = stateMachine.monadModel.transform.rotation.eulerAngles;
        //  tempRoation.x = 0;
        // stateMachine.monadModel.transform.rotation = Quaternion.Euler(tempRoation);
    }
}
