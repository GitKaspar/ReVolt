using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePresenter : MonoBehaviour
{
    public Stat StatInfo;
    private bool maxLevelReached;

    public TextMeshProUGUI descriptionText;
    public string Description;

    private Button button;

    public Transform LevelOverviewPanel;
    public GameObject LevelPanelPrefab;

    public Color irrelevantLevel;
    public Color currentLevel;
    public Color upgradeLevel;

    public TextMeshProUGUI maxReachedText;

    private void Awake()
    {
        maxLevelReached = StatInfo.currentLevel >= StatInfo.Levels.Count - 1;

        button = GetComponentInChildren<Button>();
        button.interactable = !maxLevelReached;
        button.GetComponentInChildren<TextMeshProUGUI>().text = Enum.GetName(typeof(StatName), StatInfo.Name);

        descriptionText.text = Description;

        for (int i = 0; i < StatInfo.Levels.Count; i++)
        {
            GameObject panel = Instantiate<GameObject>(LevelPanelPrefab, LevelOverviewPanel);
            panel.GetComponentInChildren<TextMeshProUGUI>().text = StatInfo.Levels[i].ToString();
            panel.GetComponent<Image>().color = (StatInfo.currentLevel == i) ? currentLevel 
                        : (StatInfo.currentLevel == i - 1) ? upgradeLevel
                        : irrelevantLevel;
        }

        maxReachedText.enabled = maxLevelReached;
    }
}
