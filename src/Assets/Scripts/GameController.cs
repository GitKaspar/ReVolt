using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController GameControllerInstance;

    private PlayerControls.PlayerActions actions;

    private bool isRunning;
    private bool isPaused;

    private AudioSource ambienceMusicSource;

    public GameObject FadeBlackPanel;
    public GameObject EndPanel;
    public GameObject PausePanel;
    public GameObject KeyPanel;
    public GameObject WorkshopButton;
    public GameObject RetryButton;
    public GameObject ButtonsParent;
    public TextMeshProUGUI ResultText;
    public GameObject SoundControllerPrefab;

    public GameObject PromtPanel;
    public TextMeshProUGUI PromtText;

    public TextMeshProUGUI dropIndicatorText;

    public Drop[] Drops;
    private int numDropsDone = 0;

    

    private void Awake()
    {
        GameControllerInstance = this;
        EndPanel.SetActive(false);
        PausePanel.SetActive(false);
        PromtPanel.SetActive(false);
        KeyPanel.SetActive(false);
        isPaused = false;
        WorkshopButton.SetActive(false);

        dropIndicatorText.text = numDropsDone.ToString() + " of " + Drops.Length.ToString();

        actions = ControlsInstance.GetActions();
        ControlsInstance.Enable();
        actions.Pause.performed += Pause_performed;

        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;

        if (SoundController.SoundInstance == null)
        {
            GameObject.Instantiate(SoundControllerPrefab, transform);
        }

        ambienceMusicSource = SoundController.SoundInstance.AmbientMusic.Play();
        ambienceMusicSource.ignoreListenerPause = true;

        isRunning = true;
    }

    private void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    private void OnDestroy()
    {
        Cursor.visible = false;
        if (ambienceMusicSource != null)
        {
            ambienceMusicSource.Stop();
            ambienceMusicSource.ignoreListenerPause = false;
        }

        Events.OnEndGame -= OnEndGame;
        Events.OnDropDone -= OnDropDone;
        actions.Pause.performed -= Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if (isRunning)
        {
            isPaused = !isPaused;
            Pause();
        }
    }

    public void BackToGame()
    {
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    public void OnDropDone(Drop drop)
    {
        numDropsDone++;
        dropIndicatorText.text = numDropsDone.ToString() + " of " + Drops.Length.ToString();
        if (numDropsDone == Drops.Length)
        {
            Events.EndGame(true);
        }
    }

    public void OnEndGame(bool isWin)
    {
        isRunning = false;
        AudioListener.pause = true;
        ControlsInstance.Disable(); //Disable player input actions
        Cursor.visible = true;

        if (isWin)
        {
            ResultText.text = "Win";

            if (ProgressManager.Instance.currentLevel == ProgressManager.Instance.GameLevels.Length - 1) //entire game beat -> fade to black ending
            {
                ambienceMusicSource.Stop();
                ambienceMusicSource.ignoreListenerPause = false;
                Time.timeScale = 0.5f;
                ButtonsParent.SetActive(false);
                FadeBlackPanel.SetActive(true);
            }
            else
            {
                Time.timeScale = 0;
                WorkshopButton.SetActive(true);
                RetryButton.SetActive(false);
            }

            //Set up what to load next
            ProgressManager.Instance.workshopVisited = false;
            Events.LevelBeat();
        }
        else
            ResultText.text = "Busted";
        
        EndPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        //SoundController.SoundInstance.ButtonClick(); //-> done in AnimatedButton
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ToWorkshop()
    {
        //SoundController.SoundInstance.ButtonClick(); //-> done in AnimatedButton
        SceneManager.LoadSceneAsync("UpgradeScreen");
    }

    public void Instructions()
    {
        KeyPanel.SetActive(true);
        PausePanel.SetActive(false);
        isPaused = false;
    }

    public void Pause()
    {
        if (isPaused)
        {
            AudioListener.pause = true;
            Time.timeScale = 0;
            KeyPanel.SetActive(false);
            PausePanel.SetActive(true);
            Cursor.visible = true;

        }
        else
        {
            AudioListener.pause = false;
            BackToGame();
            KeyPanel.SetActive(false);
        }
    }

    //Retry level after failing it
    public void Reset()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
