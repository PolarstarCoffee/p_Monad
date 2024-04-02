using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchMusicSet : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance().PlayMusic("Church");
    }
}
