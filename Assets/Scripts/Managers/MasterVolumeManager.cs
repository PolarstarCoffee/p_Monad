using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeManager : MonoBehaviour
{
    // Reference to UI Slider
    public Slider volumeSlider;

    void Start()
    {
        // Find the Slider component
        volumeSlider = FindObjectOfType<Slider>();

        float savedVolume;

        // Check if the game is being launched for the first time
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            // Load volume from PlayerPrefs
            savedVolume = PlayerPrefs.GetFloat("MasterVolume");
        }
        else
        {
            // Set initial volume to 0.5f
            savedVolume = 0.5f;
            PlayerPrefs.SetFloat("MasterVolume", savedVolume);
        }

        OnVolumeSliderChanged(savedVolume);

        // Make this object persistent across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    // Called when the slider value changes
    public void OnVolumeSliderChanged(float value)
    {
        // Find all AudioSource components in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        if (audioSources != null)
        {
            // Update the volume of all AudioSources
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.volume = value;
            }

            // Save volume to PlayerPrefs
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
        else
        {
            // Handle the situation when audioSources is null
            Debug.LogError("audioSources is null");
        }
    }

    // New method to update the slider's value
    public void UpdateSliderValue()
    {
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = savedVolume;
        }
        else
        {
            // Handle the situation when volumeSlider is null
            Debug.LogError("volumeSlider is null");
        }
    }

    // Called when the GameObject is enabled
    void OnEnable()
    {
        // Update the slider's value
        UpdateSliderValue();
    }
}