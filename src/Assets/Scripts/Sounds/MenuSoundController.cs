using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundController : MonoBehaviour
{
    public AudioClip MenuTheme;
    public AudioClipGroup ButtonClicks;

    public void ButtonClick()
    {
        ButtonClicks.Play();
    }
}
