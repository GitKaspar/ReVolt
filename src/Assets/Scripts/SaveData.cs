[System.Serializable]
public class SaveData
{
    public int currentLevel;
    public bool workshopVisited;

    public int speedLevel;
    public int batteryLevel;

    public string revolutionMessage;

    public SaveData()
    { 
        currentLevel = ProgressManager.Instance.currentLevel;
        workshopVisited = ProgressManager.Instance.workshopVisited;
        speedLevel = UpgradeStats.Instance.GetCurrentLevel(StatName.Speed);
        batteryLevel = UpgradeStats.Instance.GetCurrentLevel(StatName.Battery);
        revolutionMessage = ProgressManager.Instance.revolutionMessage;
    }

    public SaveData(int currentLevel, bool workshopVisited ,int speedLevel, int batteryLevel, string revolutionMessage)
    {
        this.currentLevel = currentLevel;
        this.workshopVisited = workshopVisited;

        this.speedLevel = speedLevel;
        this.batteryLevel = batteryLevel;

        this.revolutionMessage = revolutionMessage;
    }
}
