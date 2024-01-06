using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControlsInstance
{
    public static PlayerControls instance;
    public static PlayerControls.PlayerActions playerActions;

    public static PlayerControls.PlayerActions GetActions()
    {
        if (instance == null)
        {
            instance = new PlayerControls();
            playerActions = instance.Player;
            playerActions.Enable();
        }


        return playerActions;
    }

    public static void Disable()
    {
        if(instance != null) 
        {
            playerActions.Disable();
        }
    }

    public static void Enable()
    {
        if (instance != null)
        {
            playerActions.Enable();
        }
    }
}
