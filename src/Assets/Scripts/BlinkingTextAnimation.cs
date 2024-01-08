using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingTextAnimation : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public float offTime = 0.4f;
    public float onTime = 0.9f;

    private float timeStamp = 0f;
    private bool isOn = true;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOn & timeStamp >= offTime)
        {
            tmp.gameObject.SetActive(true);
            isOn = true;
            timeStamp = 0f;
        }

        if (isOn & timeStamp >= onTime) 
        {
            tmp.gameObject.SetActive(false);
            isOn = false;
            timeStamp = 0f;
        }

        timeStamp += Time.unscaledDeltaTime;
    }

    private void OnEnable()
    {
        timeStamp = 0f;
        tmp.gameObject.SetActive(false);
        isOn = false;
    }

    private void OnDisable()
    {
        tmp.gameObject.SetActive(true);
    }
}
