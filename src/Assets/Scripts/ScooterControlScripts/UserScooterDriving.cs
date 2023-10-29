using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TwoWheelController))]
public class UserScooterDriving : MonoBehaviour
{
    private TwoWheelController m_Scooter; // the scooter controller we want to use

    private void Awake()
    {
        // get the scooter controller
        m_Scooter = GetComponent<TwoWheelController>();
    }


    private void FixedUpdate()
    {
        // pass the input to the scooter!
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Drive");
        float handbrake = Input.GetAxis("Brake");
        float speedAdjustment = Input.GetAxisRaw("SpeedAdjustment");

        m_Scooter.Move(h, v , speedAdjustment, handbrake);
    }
}

