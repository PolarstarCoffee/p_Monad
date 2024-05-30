using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldFrameUntilButtonPress : MonoBehaviour
{
    public Sprite[] animationFrames;
    private Image imageComponent;
    private int currentFrameIndex = 0;
    private bool isPaused = true;
    public Controls inputActions = new Controls();

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        SetCurrentFrame();
        inputActions.MainMenu.Enable();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || inputActions.MainMenu.Select.WasPressedThisFrame() && isPaused)
        {
            // Press the Space key to resume the animation.
            isPaused = false;
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        while (!isPaused && currentFrameIndex < animationFrames.Length)
        {
            SetCurrentFrame();
            yield return new WaitForSeconds(0.1f);  // Adjust the delay between frames.
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
