using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattery : MonoBehaviour
{
    public Battery Battery;
    public Image BatteryLevelDisplay;
    public TextMeshProUGUI BatteryValue;

    // Update is called once per frame
    void Update()
    {
        BatteryLevelDisplay.fillAmount = Battery.CurrentCapacity / Battery.MaxCapacity;
        BatteryValue.text = Mathf.CeilToInt(Battery.CurrentCapacity / Battery.MaxCapacity * 100).ToString();
    }
}
