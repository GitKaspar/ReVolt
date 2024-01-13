using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public float[] ImpactLevels;
    public AudioClipGroup Collisions;
    public float MaxVolumeLevel = 0.5f;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log(collision.gameObject.name + ": " + collision.impulse.sqrMagnitude);
        
        if (collision.impulse.sqrMagnitude > 100)
        {
            AudioSource source = AudioSourcePool.Instance.GetSource();
            float volume = DetermineVolume(collision.impulse.sqrMagnitude);
            Collisions.VolumeMax = volume * MaxVolumeLevel;
            Collisions.VolumeMin = volume * MaxVolumeLevel;
            Collisions.Play(source);
        }
    }

    private float DetermineVolume(float impulseSqrMagnitude)
    {
        for (int i = 0; i < ImpactLevels.Length; i++)
        {
            if (impulseSqrMagnitude < ImpactLevels[i])
            {
                return i / ImpactLevels.Length + 1f / ImpactLevels.Length;
            }
        }

        return 1f;
    }
}
