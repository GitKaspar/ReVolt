using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject EndPanel;
    public TextMeshProUGUI ResultText;

    public Drop[] Drops;
    private int numDropsDone = 0;

    // Kaspar added the following 3 methods for the lose event. Attack.catchPlayer is invoked, when the drone begins attacking.

    private void OnEnable()
    {
        Attack.catchPlayer += EndGame;
    }

    private void OnDisable()
    {
        Attack.catchPlayer -= EndGame;
    }

    public void EndGame()
    {
        ResultText.text = "Busted";
        EndPanel.SetActive(true);
    }

    private void Awake()
    {
        EndPanel.SetActive(false);

        Events.OnEndGame += OnEndGame;
        Events.OnDropDone += OnDropDone;
    }

    private void OnDestroy()
    {
        Events.OnEndGame -= OnEndGame;
        Events.OnDropDone -= OnDropDone;
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

 

}
