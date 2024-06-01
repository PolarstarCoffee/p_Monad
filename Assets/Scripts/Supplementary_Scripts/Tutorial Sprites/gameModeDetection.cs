using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameModeDetection : MonoBehaviour
{
    private bool connected = false;
    public GameObject movementVisual;
    public GameObject jumpVisual;
    public GameObject sprintvisual;
    public GameObject jumpVisual_2;
    public GameObject boostVisual;
    public GameObject boostVisual_2;
    public GameObject sprintJumpVisual;
    public GameObject forwardJumpVisual;
    public Sprite[] spriteArray;
    IEnumerator CheckForControllers()
    {
        while (true)
        {
            var controllers = Input.GetJoystickNames();

            if (!connected && controllers.Length > 0)
            {
                connected = true;
                Debug.Log("Connected");

            }
            else if (connected && controllers.Length == 0)
            {
                connected = false;
                Debug.Log("Disconnected");
            }

            yield return new WaitForSeconds(.5f);
        }
    }

    void Awake()
    {
        StartCoroutine(CheckForControllers());
    }

    private void Start()
    {
        if (connected)
        {

        }
    }
}
