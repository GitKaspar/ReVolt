using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrontLight : MonoBehaviour
{
    public Light Light;
    private bool _enabled = false;

    private void Start()
    {
        _enabled = false;
        Light.enabled = _enabled;
    }

    public void OnFlashlight()
    {
        _enabled = !_enabled;
        Light.enabled = _enabled;

        SoundController.SoundInstance.Flashlight.Play();
    }
}
