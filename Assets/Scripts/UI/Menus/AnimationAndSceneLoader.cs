using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnimationAndSceneLoader : MonoBehaviour
{
    public Sprite[] animationFrames;
    public string nextSceneName;
    private Image imageComponent;
    private int currentFrameIndex = 0;
    private bool isPaused = true;
    public Controls inputActions;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        SetCurrentFrame();
        inputActions = new Controls();
    }

    private void Update()
    {
        if (inputActions.MainMenu.Select.triggered && isPaused)
        {
            isPaused = false;
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        while (!isPaused && currentFrameIndex < animationFrames.Length)
        {
            SetCurrentFrame();
            yield return new WaitForSeconds(0.03f);  // Adjust the delay between frames.
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