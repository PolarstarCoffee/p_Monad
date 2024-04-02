using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerGate : MonoBehaviour
{
    public GameObject gate; // Assign your gate object here in the inspector
    public float loweringSpeed = 1f; // Speed at which the gate lowers
    public float loweringDistance = 5f; // Distance the gate lowers

    private Vector3 initialPosition;
    private Vector3 loweredPosition;
    private bool isLowering = false;

    void Start()
    {
        initialPosition = gate.transform.position;
        loweredPosition = new Vector3(initialPosition.x, initialPosition.y - loweringDistance, initialPosition.z);
    }

    void Update()
    {
        if (isLowering)
        {
            gate.transform.position = Vector3.MoveTowards(gate.transform.position, loweredPosition, loweringSpeed * Time.deltaTime);
        }
    }

    public void OnButtonPressed() // Call this function when your button is pressed
    {
        isLowering = true;
    }
}