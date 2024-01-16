using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatName
{
    Speed, 
    Battery,
    Stealth
}


public class UpgradeStats : MonoBehaviour
{
    public static UpgradeStats Instance;

    [Serialize]
    public List<Stat> StatList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

#if UNITY_EDITOR
        ResetAll();
#endif
    }

    public float GetCurrentValue(StatName name)
    {
        foreach (Stat stat in StatList) 
        { 
            if (stat.Name == name)
            {
                return stat.getCurrentValue();
            }
        }

        Debug.Log("Stat not found");
        return 0;
    }

    public float GetLevelValue(StatName name, int level)
    {
        foreach (Stat stat in StatList)
        {
            if (stat.Name == name)
            {
                return stat.getLevelValue(level);
            }
        }

        Debug.Log("Stat not found");
        return 0;
    }

    public void UpgradeStat(StatName name)
    {
        foreach (Stat stat in StatList)
        {
            if (stat.Name == name)
            {
                stat.UpgradeLevel();
            }
        }
    }

    public void SetCurrentLevel(StatName name, int level)
    {
        foreach (Stat stat in StatList)
        {
            if (stat.Name == name)
            {
                stat.SetLevel(level);
            }
        }
    }

    public int GetCurrentLevel(StatName name)
    {
        foreach (Stat stat in StatList)
        {
            if (stat.Name == name)
            {
                return stat.currentLevel;            
            }
        }

        Debug.Log("Stat not found");
        return -1;
    }

    public void ResetAll()
    {
        foreach (Stat stat in StatList)
        {
            stat.ResetLevel();
        }
    }
}
