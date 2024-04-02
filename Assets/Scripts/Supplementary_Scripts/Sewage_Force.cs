using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sewage_Force : MonoBehaviour
{
    // Strength of Force Applied Public
    public float speed = 10f;
    // Direction of Force Public
    //public Vector3 direction = Vector3.right; 

    private void OnTriggerStay(Collider other)
    {
        // Referance Player Tag
        //Debug.Log("TRIGGERENTER");
        if (other.gameObject.CompareTag("Player"))
            
        {
            // Get Player RB
            CharacterController playerCC = other.gameObject.GetComponent<CharacterController>();
            //Debug.Log("GETRB");

            if (playerCC != null)
            {
                // Apply Force to Player
                Vector3 worldDirection = transform.right;
                playerCC.Move(worldDirection * speed * Time.deltaTime);
                //Debug.Log("FORCEAPPLIED");
            }
        }
    }
}
