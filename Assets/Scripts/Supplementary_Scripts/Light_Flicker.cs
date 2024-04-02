using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Flicker : MonoBehaviour
{
    public Light _Light;
    public float minTime;
    public float maxTime;
    public float timer;

    public AudioSource AS;
    public AudioClip LightAudio;

    void Start()
    {
        timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        Light_Flickering(); 
    }

    void Light_Flickering()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            _Light.enabled = !_Light.enabled;
            timer = Random.Range(minTime, maxTime);
            //Commented out to avoid error until we have SFX
            //AS.PlayOneShot(LightAudio);
        }
    }
}
