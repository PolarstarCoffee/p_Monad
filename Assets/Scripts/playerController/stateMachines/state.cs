using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class state 
{
    //abstract state, describing the very basics of what a "state" does
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

}
