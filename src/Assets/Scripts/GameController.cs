using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject EndPanel;
    public TextMeshProUGUI ResultText;

    private void Awake()
    {
        EndPanel.SetActive(false);

        Events.OnEndGame += OnEndGame;
    }

    private void OnDestroy()
    {
        Events.OnEndGame -= OnEndGame;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > 2f)
            Events.EndGame(true);
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
