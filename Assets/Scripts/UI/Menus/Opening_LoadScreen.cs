using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Opening_LoadScreen : MonoBehaviour
{
    public Sprite[] animationFrames;
    public string nextSceneName;
    private Image imageComponent;
    private int currentFrameIndex = 0;
    private bool isPaused = true;
    public int spaceCount = 0;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        SetCurrentFrame();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isPaused)
        {
            spaceCount++;
            if(spaceCount == 7) {
                isPaused = false;
                StartCoroutine(PlayAnimation());
            }
            
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
