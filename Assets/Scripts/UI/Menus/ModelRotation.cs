using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModelRotation : MonoBehaviour
{
    //Input action Ref
    private Controls inputActions;
    //Camera Rotation Speed
    public float rotationSpeed = 60f;

    //Assign Main Camera
    public Camera mainCamera;

    // Sound flags
    private bool startSoundPlayed = false;
    private bool exitSoundPlayed = false;
    private bool optionsSoundPlayed = false;
    private bool creditsSoundPlayed = false;

    // Inside threshold flags
    private bool insideThreshold = false;

    //Animation variables
    public Sprite[] animationFrames;
    private Image imageComponent;
    private int currentFrameIndex = 0;
    private bool isPaused = true;

    // Flag to check if the action is currently in progress
    private bool actionInProgress = false;

    private void Start()
    {
        AudioManager.Instance().PlayMusic("MenuMusic");
        //imageComponent = GetComponent<Image>();
        //SetCurrentFrame();
        inputActions = new Controls();
        inputActions.MainMenu.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance from the model to the camera
        float distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);

        // Define a threshold distance within which the model will rotate
        float thresholdDistance = 500.0f;

        // Only rotate the model if it's within the threshold distance to the camera
        if (distanceToCamera <= thresholdDistance)
        {
            if (!insideThreshold)
            {
                // Reset the sound flags when the model enters the threshold distance
                startSoundPlayed = false;
                exitSoundPlayed = false;
                optionsSoundPlayed = false;
                creditsSoundPlayed = false;
                insideThreshold = true;
            }

            Quaternion rotation = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
            transform.localRotation = rotation * transform.localRotation;

            //Play Sound on Specific Model Rotate
            switch (gameObject.tag)
            {
                case "StartButton":
                    // StartButton Sound
                    if (!startSoundPlayed)
                    {
                        AudioManager.Instance().PlaySound("Start");
                        startSoundPlayed = true;
                    }
                    break;
                case "ExitButton":
                    // ExitButton Sound
                    if (!exitSoundPlayed)
                    {
                        AudioManager.Instance().PlaySound("Exit");
                        exitSoundPlayed = true;
                    }
                    break;
                case "OptionsButton":
                    // OptionButton Sound
                    if (!optionsSoundPlayed)
                    {
                        AudioManager.Instance().PlaySound("Options");
                        optionsSoundPlayed = true;
                    }
                    break;
                case "CreditsButton":
                    // CreditsButton Sound
                    if (!creditsSoundPlayed)
                    {
                        AudioManager.Instance().PlaySound("Credits");
                        creditsSoundPlayed = true;
                    }
                    break;
            }

            // Check for Enter key press
            if (inputActions.MainMenu.Select.WasPressedThisFrame() && isPaused && !actionInProgress)
            {
                // Set the flag to indicate that the action is in progress
                actionInProgress = true;
                //Disable input action
                inputActions.MainMenu.Disable();
                //Play Audio Clip on Enter Pres
                AudioManager.Instance().PlaySound("Enter");

                // Perform an action based on the model's tag
                switch (gameObject.tag)
                {
                    case "StartButton":
                        StartCoroutine(LoadSceneAfterDelay("Start Scene", 1.4f)); // Adjust the delay as needed
                        break;
                    case "ExitButton":
                        StartCoroutine(QuitGameAfterDelay(1.4f)); 
                        break;
                    case "OptionsButton":
                        StartCoroutine(LoadSceneAfterDelay("Options Menu", 1.4f)); 
                        break;
                    case "CreditsButton":
                        StartCoroutine(LoadSceneAfterDelay("Credits Menu", 1.4f)); 
                        break;
                }
            }
        }
        else
        {
            // Set the inside threshold flag to false when the model is outside the threshold distance
            insideThreshold = false;
        }
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        isPaused = false;
        StartCoroutine(PlayAnimation());
        SceneManager.LoadScene(sceneName);

        // Reset the action in progress flag after the action is complete
        actionInProgress = false;
    }

    // Coroutine to quit the game after a delay
    private IEnumerator QuitGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();

        // Reset the action in progress flag after the action is complete
        actionInProgress = false;
    }

    private IEnumerator PlayAnimation()
    {
        while (!isPaused && currentFrameIndex < animationFrames.Length)
        {
            SetCurrentFrame();
            yield return new WaitForSeconds(0.04f);  // Adjust the delay between frames.
            currentFrameIndex++;
        }

        isPaused = true;  // Animation completed, set back to paused state.
        currentFrameIndex = 0;  // Reset the frame index for the next play.

    }

    private void SetCurrentFrame()
    {
        imageComponent.sprite = animationFrames[currentFrameIndex];
    }


}