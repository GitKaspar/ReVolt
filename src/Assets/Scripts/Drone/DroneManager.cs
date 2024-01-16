using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : MonoBehaviour
{
    public GameObject Player;
    private AudioSource playerAudioSource;

    // Get stat from stat manager. Placeholder
    public float StealthFloat = 1;

    void Awake()
    {
        playerAudioSource = Player.GetComponent<AudioSource>();
        playerAudioSource.volume = playerAudioSource.volume * StealthFloat;

        // This seems to work.
        AI[] droneAIs = FindObjectsOfType<AI>();
        foreach (AI droneAI in droneAIs)
        {
            droneAI.StealthModifier = StealthFloat;
        }
    }
}
