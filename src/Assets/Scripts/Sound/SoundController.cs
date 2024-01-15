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
    public AudioClipGroup ChargeDone;

    public AudioClipGroup MenuMusic;

    public AudioClipGroup Flashlight;

    public AudioClipGroup Jump;

    public AudioClipGroup Collisions;

    public AudioClipGroup Upgrade;
    public AudioClipGroup WorkshopMusic;

    public AudioClip DroneShutDown;

    private AudioSource buttonAudioSource;

    private void Awake()
    {

        if (SoundInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SoundInstance = this;

        buttonAudioSource = GetComponent<AudioSource>(); //need own audiosource for buttons so that buttons sounds still work when game is paused
        buttonAudioSource.ignoreListenerPause = true;
    }

    public void ButtonClick()
    {
        ButtonClicks.Play(buttonAudioSource);
    }
}
