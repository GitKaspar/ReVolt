using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDevice : MonoBehaviour
{
    private void Start()
    {
        Events.ControlSchemeChange(GetComponent<PlayerInput>().currentControlScheme);
    }

    public void OnControlsChanged(PlayerInput pi)
    {
        Events.ControlSchemeChange(pi.currentControlScheme);
    }
}
