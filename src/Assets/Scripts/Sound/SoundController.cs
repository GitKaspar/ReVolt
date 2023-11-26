using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour
{
    public static SoundController SoundInstance;

    public AudioClipGroup Drops;
    public AudioClipGroup Lasers;
    public AudioClip Scan1;
    public AudioClip Alarm1;
    public AudioClipGroup AmbientMusic;

    public AudioClipGroup ButtonClicks;

    public AudioClipGroup Charge;

    public AudioClipGroup MenuMusic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SoundInstance = this;
    }

    public void ButtonClick()
    {
        ButtonClicks.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                {
                    // MenuMusic.Play(); // Does not work...
                    Debug.Log("Menu Music playing");
                    break;
                }
            case 1:
                {
                    AmbientMusic.Play();
                    Debug.Log("Ambient music playing.");
                    break;
                }
            default: return;
        }
    }



}
