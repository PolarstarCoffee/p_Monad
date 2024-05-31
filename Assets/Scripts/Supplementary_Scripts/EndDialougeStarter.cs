using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class EndDialougeStarter : MonoBehaviour
{
    private NonPlayerDialouge collisionSaver;
    public Controls inputActions;
    private bool alreadyHit = false;

    //Animation variables
    public Sprite[] animationFrames;
    private Image imageComponent;
    private int currentFrameIndex = 0;
    private bool isPaused = true;

    private bool actionInProgress = false;



    // Start is called before the first frame update
    void Start()
    {
        inputActions = new Controls();
        collisionSaver = GetComponent<NonPlayerDialouge>();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.MainMenu.Select.WasPressedThisFrame() && !actionInProgress)
        {

            actionInProgress = true;

            collisionSaver.GetNPC().GetComponent<NPCDialogueManager>().StartConversation();
            DialogueDisplay.Instance().OnDialogueCompletion.AddListener(Transition);
        }
    }

    private void Transition()
    {
        DialogueDisplay.Instance().OnDialogueCompletion.RemoveAllListeners();
        // MAKE TRANSITION HAPPEN
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(LoadSceneAfterDelay("Credits Menu", 1.4f));
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
