using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public GameObject Player;
    private AudioSource playerAudioSource;
    public float StealthFloat = 1;

    void Awake()
    {
        // Get stat from stat manager.
        playerAudioSource = Player.GetComponent<AudioSource>();
        playerAudioSource.volume = playerAudioSource.volume * StealthFloat;

        AI[] droneAIs = FindObjectsOfType<AI>();
        

        foreach (AI droneAI in droneAIs)
        {
            droneAI.StealthModifier = StealthFloat;
        }
    }
}
