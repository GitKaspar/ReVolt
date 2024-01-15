using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject CreditsPanel;
    public GameObject MessagePanel;
    public GameObject Story;
    public GameObject KeysPC;
    public GameObject KeysController;
    public GameObject ContinueButton;
    public TextMeshProUGUI PlayText;
    public TextMeshProUGUI displayBio;


    public static int sceneLoadedNum = 0;

    private AudioSource menuMusicSource;


    private void Awake()
    {
        InstructionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        MessagePanel.SetActive(false);
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

        if (ProgressManager.Instance.GetNextLevelName() == null)
        {
            ContinueButton.SetActive(false);
            PlayText.text = "play again";
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
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        KeysPC.SetActive(true);
        KeysController.SetActive(false);
        
    }

    public void OpenCredits()
    {
        displayBio.text = "Who are you curious about?";
        CreditsPanel.SetActive(true);
    }

    public void NicolasButton()
    {
        displayBio.text = "Nicolas is german lol";
    }

    public void KasparButton()
    {
        displayBio.text = "Kaspar studies history lol";
    }

    public void TristanButton()
    {
        displayBio.text = "Tristan is lol";
    }

    public void NextKeysPC()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        MessagePanel.SetActive(false);
        InstructionsPanel.SetActive(true);
        Story.SetActive(false);
        KeysPC.SetActive(true);
        KeysController.SetActive(false);
    }
    public void NextMessage()
    {
        InstructionsPanel.SetActive(false);
        MessagePanel.SetActive(true);
    }

    public void NextKeysController()
    {
        //SoundController.SoundInstance.ButtonClick(); //sound triggered in animated button script
        MessagePanel.SetActive(false);
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
        CreditsPanel.SetActive(false);
        MessagePanel.SetActive(false);
    }
}
