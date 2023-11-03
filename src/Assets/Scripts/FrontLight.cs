using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontLight : MonoBehaviour
{
    public Light Light;
    private bool _enabled = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            _enabled = !_enabled;
            Light.enabled = _enabled;
        }
    }
}
