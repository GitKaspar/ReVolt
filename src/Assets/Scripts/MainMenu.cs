using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject Story;
    public GameObject Keys;

    private void Awake()
    {
        InstructionsPanel.SetActive(false);
        Story.SetActive(false);
        Keys.SetActive(false);
    }
    public void PlayGame()
    {
        UnityEngine.Debug.Log("Play");
        SceneManager.LoadSceneAsync(1);
    }


    public void QuitGame()
    {
        Application.Quit();
        UnityEngine.Debug.Log("Quit the game");
    }

    public void Instructions()
    {
        InstructionsPanel.SetActive(true);
        Story.SetActive(true);
        Keys.SetActive(false);
    }

    public void Next()
    {
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        Keys.SetActive(true);
    }
}
