using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{

    [SerializeField]
    Button startGame;
    [SerializeField]
    Button endGame;

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<AudioManager>().Play("m");
        startGame.onClick.AddListener(StartGame);
        endGame.onClick.AddListener(EndGame);
    }   

    // Update is called once per frame
    public void StartGame()
    {
        ScenesManager.instance.LoadNextScene();
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
