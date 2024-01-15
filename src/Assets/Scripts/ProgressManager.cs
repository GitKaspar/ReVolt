using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;

    public string TutorialLevel;
    public string[] GameLevels;
    public string WorkshopLevel;
    
    public int currentLevel = -1;
    public bool workshopVisited { get; set; }

    //[HideInInspector]
    public string revolutionMessage;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        Events.OnLevelBeat += OnProgress;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        Events.OnLevelBeat -= OnProgress;
    }

    public string GetNextLevelName()
    {
        if (currentLevel == -1)
        { 
            return TutorialLevel;
        }
        else if (currentLevel > GameLevels.Length - 1)
        {
            Debug.Log("All levels done, game completed.");
            return null;
        }
        else if (!workshopVisited)
        {
            return WorkshopLevel;
        }
        else
        {
            return GameLevels[currentLevel];
        }
    }

    public void OnProgress()
    {
        currentLevel++;
    }
    

    public void ResetProgress()
    {
        currentLevel = -1;
        workshopVisited = false;
        UpgradeStats.Instance.ResetAll();
        revolutionMessage = "";
    }
}
