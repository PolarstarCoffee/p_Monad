using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitchSlider : MonoBehaviour
{

    public Slider pitchSlider;
    private AudioSource[] audioSources;

    void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
        pitchSlider.minValue = 0.5f;
        pitchSlider.maxValue = 1.5f;
        pitchSlider.value = 1f;
        pitchSlider.onValueChanged.AddListener(ChangePitch);
    }

    void ChangePitch(float value)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch = value;
        }
    }
}
