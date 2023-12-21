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

    private bool isPaused;

    public GameObject EndPanel;
    public GameObject PausePanel;
    public GameObject KeyPanel;
    public GameObject WorkshopButton;
    public TextMeshProUGUI ResultText;
    public GameObject SoundControllerPrefab;

    public GameObject PromtPanel;
    public TextMeshProUGUI PromtText;

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

        actions = ControlsInstance.GetActions();
        actions.Pause.performed += Pause_performed;

        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;
        Attack.catchPlayer += EndGame;

        if (SoundController.SoundInstance == null)
        {
            GameObject.Instantiate(SoundControllerPrefab, transform);
        }
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        isPaused = !isPaused;
        Pause();
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause(isPaused);
        }
    }
    */

    public void BackToGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

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
        Attack.catchPlayer -= EndGame;
    }

    public void OnDropDone(Drop drop)
    {
        numDropsDone++;
        Debug.Log(drop.transform.position);
        foreach (Drop d in Drops)
        {
            Debug.Log(d.done);
        }

        if (numDropsDone == Drops.Length)
        {
            Events.EndGame(true);
        }
    }



    public void OnEndGame(bool isWin)
    {
        Time.timeScale = 0;
        if (isWin)
        {
            ResultText.text = "Win";
            WorkshopButton.SetActive(true);
        }
        else
            ResultText.text = "Busted";



        ControlsInstance.Disable(); //Disable player input actions
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
        }
    }


    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            SoundController.SoundInstance.MenuMusic.Play();
        }
    }


}
