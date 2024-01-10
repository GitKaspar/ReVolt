using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleController : MonoBehaviour
{
    public Drop[] Drops;
    private int numDropsDone = 0;

    public void EndGame()
    {

    }

    private void Awake()
    {
        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;
        Attack.catchPlayer += EndGame;

        Time.timeScale = 1;
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
        SceneManager.LoadSceneAsync("UpgradeScreen");
    }

    public void BackToMain()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
