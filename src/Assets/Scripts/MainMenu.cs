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
    public GameObject KeysPC;
    public GameObject KeysController;
    public GameObject ContinueButton;
    public TextMeshProUGUI PlayText;


    public static int sceneLoadedNum = 0;

    private AudioSource menuMusicSource;


    private void Awake()
    {
        InstructionsPanel.SetActive(false);
        Story.SetActive(false);
        KeysPC.SetActive(false);
        KeysController.SetActive(false);

        sceneLoadedNum++;

        Time.timeScale = 1f; //bruh, otherwise game frozen
        AudioListener.pause = false;
        menuMusicSource = SoundController.SoundInstance.MenuMusic.Play();
        Cursor.visible = true;
    }

    private void Start()
    {
        if (ProgressManager.Instance.currentLevel != -1)
        {
            ContinueButton.SetActive(true);
            PlayText.text = "play new";
        }
    }

    private void OnDestroy()
    {
        if (menuMusicSource != null)
        {
            menuMusicSource.Stop();
        }

        Cursor.visible = false;
    }

    public void ContinueGame()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        SceneManager.LoadSceneAsync(ProgressManager.Instance.GetNextLevelName());
    }

    public void PlayGame()
    {
        /*
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
        */

        //SoundController.SoundInstance.ButtonClick();
        ProgressManager.Instance.ResetProgress();
        InstructionsPanel.SetActive(true);
        Story.SetActive(true);
        KeysPC.SetActive(false);
        KeysController.SetActive(false);


    }

    public void PlayButton()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        UnityEngine.Debug.Log("Play");
        SceneManager.LoadSceneAsync(ProgressManager.Instance.GetNextLevelName());
    }


    public void QuitGame()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        Application.Quit();
        UnityEngine.Debug.Log("Quit the game");
    }

    public void Instructions()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        if (sceneLoadedNum < 2)
        {
            InstructionsPanel.SetActive(true);
            Story.SetActive(true);
            KeysPC.SetActive(false);
            KeysController.SetActive(false);
        }
        else
        {
            Next();
        }
        
    }

    public void Next()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        KeysPC.SetActive(true);
        KeysController.SetActive(false);
    }

    public void NextAgain()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        KeysPC.SetActive(false);
        KeysController.SetActive(true);
    }

    public void BackToMainMenu()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        //SceneManager.LoadSceneAsync("MainMenu");
        InstructionsPanel.SetActive(false);
    }
}
