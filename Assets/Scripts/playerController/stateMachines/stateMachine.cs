using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class stateMachine : MonoBehaviour
{
    //this class is the state machine, the methods that would change the game or player state  
    private state currentState;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        //? syntax means if the current state is NOT null (Fancy if statement) Null conditioner operator
        currentState?.Tick(Time.deltaTime);
    }

    public void switchState(state newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
}
