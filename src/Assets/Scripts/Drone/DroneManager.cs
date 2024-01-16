using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public GameObject Player;
    private ScooterSound scooterSound;
    private float stealthFloat;

    void Awake()
    {

        float stealthFloat = UpgradeStats.Instance.GetCurrentValue(StatName.Stealth);
        scooterSound = Player.GetComponent<ScooterSound>();
        scooterSound.StealthModifier = stealthFloat; 

        // This seems to work.
        AI[] droneAIs = FindObjectsOfType<AI>();
        foreach (AI droneAI in droneAIs)
        {
            droneAI.StealthModifier = stealthFloat;
        }
    }
}