using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatName
{
    Speed, 
    Battery
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
        foreach (Stat stat in StatList)
        {
            stat.ResetLevel();
        }
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
}