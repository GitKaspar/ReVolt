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

    private void Awake()
    {
            SoundInstance = this;
    }

  

}
