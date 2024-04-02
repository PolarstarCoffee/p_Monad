using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlumsMusicSet : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance().PlayMusic("Slums");
    }
}
