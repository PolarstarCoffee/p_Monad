using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    // Reference to gate
    public gateScript gate;
    private Controls inputActions;

    // Flag to check if the player is inside the trigger zone
    private bool isInsideTrigger = false;
    private void Awake()
    {
        inputActions = new Controls();
        inputActions.UI.interact.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        // Only allow the key press if the player is inside the trigger zone
        if (isInsideTrigger && Input.GetKeyDown(KeyCode.E) || inputActions.UI.interact.WasPressedThisFrame())
        {
            AudioManager.Instance().PlaySound("lever");
            transform.RotateAround(transform.position, new Vector3(1, 0, 0), 180);
            gate.LowerGate();
            inputActions.UI.interact.Disable();
        }
    }

    // Set the flag to true when the player enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInsideTrigger = true;
        }
    }

    // Set the flag to false when the player exits the trigger zone
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInsideTrigger = false;
        }
    }
}