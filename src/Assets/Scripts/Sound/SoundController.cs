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

    public AudioClip MenuTheme;
    public AudioClipGroup ButtonClicks;

    public AudioClipGroup Charge;

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
        if(level == 1) AmbientMusic.Play();
    }



}
