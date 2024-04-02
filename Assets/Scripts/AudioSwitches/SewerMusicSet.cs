using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SewerMusicSet : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance().PlayMusic("Sewer");
    }
}
