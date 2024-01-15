using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public TMP_InputField message;
    private string defaultMessage = "I was too lazy to choose a revolution cause";

    private void OnEnable()
    {
        if (!String.IsNullOrWhiteSpace(ProgressManager.Instance.revolutionMessage) & !(ProgressManager.Instance.revolutionMessage == defaultMessage))
        {
            message.text = ProgressManager.Instance.revolutionMessage;
        }
    }

    public void SetMessageToButtonText(Button button)
    {
        SoundController.SoundInstance.ButtonClick();
        message.text = button.GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public void SaveMessage()
    {
        ProgressManager.Instance.revolutionMessage = String.IsNullOrWhiteSpace(message.text) ? defaultMessage : message.text;
    }
}
