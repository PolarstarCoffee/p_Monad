using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse_Sensitivity : MonoBehaviour
{
    public Slider sensitivitySlider;
    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
    }
}
