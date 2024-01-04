using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject Story;
    public GameObject Keys;

    public TextMeshProUGUI Play;

    public static int sceneLoadedNum = 0;


    private void Awake()
    {
        InstructionsPanel.SetActive(false);
        Story.SetActive(false);
        Keys.SetActive(false);

        sceneLoadedNum++;
    }

    public void PlayGame()
    {
        SoundController.SoundInstance.ButtonClick();
        if (sceneLoadedNum < 2)
        {
            InstructionsPanel.SetActive(true);
            Story.SetActive(true);
            Keys.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.Log("Play");
            SceneManager.LoadSceneAsync(ProgressManager.Instance.GetNextLevelName());
        }
       
    }

    public void PlayButton()
    {
        SoundController.SoundInstance.ButtonClick();
        UnityEngine.Debug.Log("Play");
        SceneManager.LoadSceneAsync(ProgressManager.Instance.GetNextLevelName());
    }


    public void QuitGame()
    {
        SoundController.SoundInstance.ButtonClick();
        Application.Quit();
        UnityEngine.Debug.Log("Quit the game");
    }

    public void Instructions()
    {
        SoundController.SoundInstance.ButtonClick();
        if (sceneLoadedNum < 2)
        {
            InstructionsPanel.SetActive(true);
            Story.SetActive(true);
            Keys.SetActive(false);
        }
        else
        {
            Next();
        }
        
    }

    public void Next()
    {
        SoundController.SoundInstance.ButtonClick();
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        Keys.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SoundController.SoundInstance.ButtonClick();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
