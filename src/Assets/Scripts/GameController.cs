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

    public GameObject EndPanel;
    public GameObject PausePanel;
    public GameObject KeyPanel;
    public GameObject WorkshopButton;
    public GameObject RetryButton;
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
        actions.Pause.performed += Pause_performed;

        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;

        if (SoundController.SoundInstance == null)
        {
            GameObject.Instantiate(SoundControllerPrefab, transform);
        }

        isRunning = true;
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
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    //kala
    public void EndGame()
    {
        ResultText.text = "Busted";
        EndPanel.SetActive(true);
        Time.timeScale = 0;
    }


    private void OnDestroy()
    {
        Events.OnEndGame -= OnEndGame;
        Events.OnDropDone -= OnDropDone;
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
        Time.timeScale = 0;
        ControlsInstance.Disable(); //Disable player input actions

        if (isWin)
        {
            ResultText.text = "Win";
            WorkshopButton.SetActive(true);
            RetryButton.SetActive(false);
        }
        else
            ResultText.text = "Busted";
        
        EndPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SoundController.SoundInstance.ButtonClick();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1;
    }

    public void ToWorkshop()
    {
        SoundController.SoundInstance.ButtonClick();
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
            Time.timeScale = 0;
            KeyPanel.SetActive(false);
            PausePanel.SetActive(true);
            
        }
        else
        {
            BackToGame();
            KeyPanel.SetActive(false);
        }
    }


    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            SoundController.SoundInstance.MenuMusic.Play();
        }
    }

    public void Reset()
    {
        SceneManager.LoadSceneAsync(1);
    }


}
