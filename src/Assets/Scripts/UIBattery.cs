using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattery : MonoBehaviour
{
    public Battery Battery;
    public Image BatteryFill;
    public TextMeshProUGUI BatteryValue;

    public Color FullColor;
    public Color MediumColor;
    public Color EmptyColor;

    // Update is called once per frame
    void Update()
    {
        float fillValue = Battery.CurrentCapacity / Battery.MaxCapacity;
        BatteryFill.fillAmount = fillValue;
        BatteryValue.text = Mathf.FloorToInt(fillValue * 100).ToString();
        if (BatteryValue.text == "100") BatteryValue.text = "99"; //To prevent triple digit display

        BatteryFill.color = 
            fillValue < 0.2 ? EmptyColor :
            fillValue < 0.8 ? MediumColor :
            FullColor;

    }
}
