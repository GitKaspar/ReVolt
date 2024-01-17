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

    private List<string> nicolasBios = new List<string>() { "is a computer science student", "his most played video game: NBA2k14", "likes dad jokes (is not a dad)", "rode an electric scooter for the first time in 2023", "big sports guy", "thanks the University of Tartu GameDev course" };
    private List<string> kaspariBios = new List<string>() { "is a history student", "once got a pea stuck up his nose", "will welcome your soul in Oblivion!", "would like to tell you the Good News", "is a bloody peasant", "thanks the peer-reviewers of this game"};
    private List<string> tristanBios = new List<string>() { "is a philosophy student", "sober for 3+ months already", "beverage guy and matcha bf", "likes downloading games more than playing them", "nintendo switch enthusiast", "pc", "thanks people uploading free assets to the internet" };

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

        Events.OnDataLoaded += DataLoaded;
    }

    private void DataLoaded()
    {
        if (ProgressManager.Instance.currentLevel != -1)
        {
            ContinueButton.SetActive(true);
            PlayText.text = "play new";
        }
        else
        {
            ContinueButton.SetActive(false);
            PlayText.text = "play";
        }

        if (ProgressManager.Instance.GetNextLevelName() == null)
        {
            ContinueButton.SetActive(false);
            PlayText.text = "play again";
        }
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

        Events.OnDataLoaded += DataLoaded;
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
        string newText = nicolasBios[Random.Range(0, nicolasBios.Count)];
        while (newText == displayBio.text) 
            { newText = nicolasBios[Random.Range(0, nicolasBios.Count)]; }

        displayBio.text = newText;
    }

    public void KasparButton()
    {
        string newText = kaspariBios[Random.Range(0, kaspariBios.Count)];
        while (newText == displayBio.text)
        { newText = kaspariBios[Random.Range(0, kaspariBios.Count)]; }

        displayBio.text = newText;
    }

    public void TristanButton()
    {
        string newText = tristanBios[Random.Range(0, tristanBios.Count)];
        while (newText == displayBio.text)
        { newText = tristanBios[Random.Range(0, tristanBios.Count)]; }

        displayBio.text = newText;
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
