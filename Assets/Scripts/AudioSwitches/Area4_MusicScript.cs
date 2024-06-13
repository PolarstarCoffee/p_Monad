using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area4_MusicScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance().PlayMusic("AWEA4");
    }
}

