using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        SaveData data = SavingSystem.Load();
        if (data != null) 
        { 
            AssignSavedValues(data);
            Events.DataLoaded();
        }
    }

    private void OnApplicationQuit()
    {
        SavingSystem.Save();
    }


    private void AssignSavedValues(SaveData data)
    {
        ProgressManager.Instance.currentLevel = data.currentLevel;
        ProgressManager.Instance.workshopVisited = data.workshopVisited;

        UpgradeStats.Instance.SetCurrentLevel(StatName.Speed, data.speedLevel);
        UpgradeStats.Instance.SetCurrentLevel(StatName.Battery, data.batteryLevel);
        UpgradeStats.Instance.SetCurrentLevel(StatName.Stealth, data.stealthLevel);

        ProgressManager.Instance.revolutionMessage = data.revolutionMessage;
    }

}
