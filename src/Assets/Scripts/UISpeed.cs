using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISpeed : MonoBehaviour
{
    public TextMeshProUGUI SpeedText;

    // Update is called once per frame
    void Update()
    {
        float speed = Events.RequestCurrentSpeed();
        SpeedText.text = Mathf.RoundToInt(speed).ToString();
    }
}
