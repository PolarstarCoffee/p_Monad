using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropKnockback : MonoBehaviour
{
    // Adjust this to change the strength of the knockback
    public float knockbackStrength = 0.05f;
    public playerBaseState baseState;
    public stateMachine stateMachine;
    void OnCollisionEnter(Collision collision)
    {
      
        // Get the object that the player collided with
        GameObject otherObject = collision.gameObject;

        // Check if the other object has a Rigidbody (i.e., it can be knocked back)
        if (otherObject.TryGetComponent(out Rigidbody otherRb))
        {
            // Calculate the direction of the knockback: away from the player and slightly up
            Vector3 knockbackDirection = (otherObject.transform.position - transform.position).normalized + Vector3.up;

            // Apply the knockback force to the other object
            otherRb.AddForce(knockbackDirection * knockbackStrength, ForceMode.Impulse);
        }
    }
}