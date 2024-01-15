using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterSound : MonoBehaviour
{
    private AudioSource scooterAudioSource;
    private Rigidbody scooterRigidbody;

    public float MaxVolumeLevel = 0.5f;

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
        maxSpeed = UpgradeStats.Instance.GetCurrentValue(StatName.Speed); //dynamic adjustment to max speed?
    }

    // Update is called once per frame
    void Update()
    {
        // Possible fix later: Nicolas seems to have created an event for requesting the current speed. Could use that instead of  the RigidBody's velocity component?

        currentSpeed = scooterRigidbody.velocity.magnitude;
        currentVolumeLevel = CurveFactor((currentSpeed / maxSpeed)) * MaxVolumeLevel;
        currentPitch = currentVolumeLevel * maxPitch;
        scooterAudioSource.volume = currentVolumeLevel;
        scooterAudioSource.pitch = currentPitch;
     }

    private float CurveFactor(float factor)
    {
        if (factor > 1) return 1;
        return 1 - (1 - factor) * Mathf.Sqrt((1 - factor));
    }
}
