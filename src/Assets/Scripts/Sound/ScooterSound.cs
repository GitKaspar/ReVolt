using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterSound : MonoBehaviour
{
    private AudioSource scooterAudioSource;
    private Rigidbody scooterRigidbody;

    private float currentSpeed;
    private float maxSpeed = 20;
    private float currentVolumeLevel = 0;
    private float currentPitch;
    private float maxPitch = 2;
    

    void Start()
    {
        scooterAudioSource = GetComponent<AudioSource>();
        scooterAudioSource.volume = currentVolumeLevel;
        scooterRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Possible fix later: Nicolas seems to have created an event for requesting the current speed. Could use that instead of  the RigidBody's velocity component?

        currentSpeed = scooterRigidbody.velocity.magnitude;
        currentVolumeLevel = currentSpeed / maxSpeed;
        currentPitch = currentVolumeLevel * maxPitch;  
        scooterAudioSource.volume = currentVolumeLevel;
        scooterAudioSource.pitch = currentPitch;
     }
}
