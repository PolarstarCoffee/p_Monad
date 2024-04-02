using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class forceReciever : MonoBehaviour
{
    //Forces that act on our player controller ((might be obsolete now))
    //Grab controller's y axis in relation to physics
    [SerializeField] private CharacterController controller;
    private float verticalVelocity;
    public Vector3 Movement => Vector3.up * verticalVelocity;
    public Controls inputActions;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //DialogueDisplay.Instance().OnDialogueCompletion.AddListener(convoEnd);
    }

    private void Update()
    {
        //Simulates gravity
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += 1.25f * Physics.gravity.y * Time.deltaTime; //Previously 1x gravity (Jay Jay)
        }
       
        
    }

    //adds jump force to vertical velocity 
    public void jump(float jumpFoce)
    {
        verticalVelocity += jumpFoce;
    }

   
    





}
