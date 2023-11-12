using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeScreen : MonoBehaviour
{
    public void OnSpeedUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Speed);
        Debug.Log("Vroooom vroooom! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Speed));
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnBatteryUpgradeClick()
    {
        UpgradeStats.Instance.UpgradeStat(StatName.Battery);
        Debug.Log("Powerbank docked! New value: " + UpgradeStats.Instance.GetCurrentValue(StatName.Battery));
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
