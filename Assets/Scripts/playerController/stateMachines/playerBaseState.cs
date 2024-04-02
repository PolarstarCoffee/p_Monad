using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class playerBaseState : state
{
    //The very bare bones of the player's movement 
    //Reads movement values 
    protected playerStateMachine stateMachine;

    //Constructor
    public playerBaseState(playerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.charController.Move((motion + stateMachine.ForceReciever.Movement) * deltaTime);
    }
}
