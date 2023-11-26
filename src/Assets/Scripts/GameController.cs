using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController GameControllerInstance;

    public GameObject EndPanel;
    public GameObject PausePanel;
    public TextMeshProUGUI ResultText;
    public GameObject SoundControllerPrefab;

    public GameObject PromtPanel;
    public TextMeshProUGUI PromtText;

    public Drop[] Drops;
    private int numDropsDone = 0;

    private bool menuActive;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuActive)
            {
                Time.timeScale = 0;
                PausePanel.SetActive(true);
                menuActive = true;
            }
            else
            {
                menuActive = false;
                BackToGame();
            }
        }
    }

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

    private void Awake()
    {
        GameControllerInstance = this;
        EndPanel.SetActive(false);
        PausePanel.SetActive(false);
        PromtPanel.SetActive(false);

        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;
        Attack.catchPlayer += EndGame;

        if (SoundController.SoundInstance == null)
        {
            GameObject.Instantiate(SoundControllerPrefab, transform);
        }
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
        if (isWin)
            ResultText.text = "Win";
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


}
