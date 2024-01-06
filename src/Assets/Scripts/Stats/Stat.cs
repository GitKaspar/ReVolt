using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class Stat : ScriptableObject
{
    public StatName Name;
    public List<float> Levels = new List<float>();
    public int currentLevel { get; private set; } = 0;

    public float getCurrentValue() => Levels[currentLevel];
    public float getLevelValue(int level)
    {
        if (level >= Levels.Count || level < 0)
        {
            Debug.Log("Invlaid level value");
            return 0;
        }

        return Levels[level];

    }

    public void UpgradeLevel()
    {
        if (currentLevel == Levels.Count - 1)
        {
            Debug.Log("Upgrade not possible, highest level already reached");
            return;
        }

        currentLevel++;
    }

    public void ResetLevel() => currentLevel = 0;

    public void SetLevel(int level)
    {
        if (level < Levels.Count && level >= 0)
        {
            currentLevel = level;
        }
    }


    /*
    public Stat(StatName name, List<float> levels)
    {
        new Stat(name, levels, 0);
    }

    public Stat(StatName name, List<float> levels, int currentLevel)
    {
        Name = name;
        Levels = levels;
        this.currentLevel = currentLevel;
    }
    */
}
