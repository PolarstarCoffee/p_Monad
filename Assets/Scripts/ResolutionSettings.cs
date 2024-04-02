using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSettings : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    Resolution[] predefinedResolutions = new Resolution[]
    {
        new Resolution() { width = 640, height = 480, refreshRate = 60 }, // Add your desired resolutions here
        new Resolution() { width = 320, height = 240, refreshRate = 60 },
        // Add more resolutions as needed
    };

    void Start()
    {
        PopulateResolutionDropdown();
    }

    void PopulateResolutionDropdown()
    {
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < predefinedResolutions.Length; i++)
        {
            string option = predefinedResolutions[i].width + " x " + predefinedResolutions[i].height;
            options.Add(option);

            if (predefinedResolutions[i].width == Screen.currentResolution.width &&
                predefinedResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = predefinedResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}