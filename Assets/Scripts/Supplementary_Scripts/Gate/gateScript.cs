using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class gateScript : MonoBehaviour
{
    //Speed of lowering gate
    public float speed = 1.0f;

    //The lowered gate position
    public float loweredPositionY; // Changed to float as we are only moving along Y-axis

    //Whether gate is currently lowering
    private bool isLowering = false;

    //Sound effect for gate lowering
    public AudioSource gateSound;

    //Virtual Camera for gate "cinematic"
    public CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        loweredPositionY = transform.position.y - 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLowering)
        {
            //If gate has lowered, stop lowering
            float newYPosition = Mathf.MoveTowards(transform.position.y, loweredPositionY, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);

            if (transform.position.y == loweredPositionY)
            {
                isLowering = false;

                //Return Camera Priority
                virtualCamera.Priority = 0;
            }
        }
    }

    public void LowerGate()
    {
        //Activate LowerGate
        isLowering = true;

        //Play SFX for LowerGate
        gateSound.Play();

        // Get the CinemachineBrain component from the main camera
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();

        // Store the current default blend time
        float currentBlendTime = brain.m_DefaultBlend.m_Time;

        // Set the default blend time to 0 for an instant transition
        brain.m_DefaultBlend.m_Time = 0;

        // Switch to the virtual camera
        virtualCamera.Priority = 11;

        // Restore the default blend time
        brain.m_DefaultBlend.m_Time = currentBlendTime;
    }
}