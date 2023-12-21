using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TwoWheelController))]
public class UserScooterDriving : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerInput playerInput;
    private TwoWheelController m_Scooter; // the scooter controller we want to use

    private float smoothHorizontal;
    public float increaseSmoothing = 6f;
    public float decreaseSmoothing = 5f;

    private void Awake()
    {
        // get the scooter controller
        m_Scooter = GetComponent<TwoWheelController>();

        playerInput = GetComponent<PlayerInput>();
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }


    private void FixedUpdate()
    {
        Vector2 driveSteer = playerControls.Player.DriveSteer.ReadValue<Vector2>();
        float speedAdjustment = playerControls.Player.SpeedAdjustment.ReadValue<float>();
        float handbrake = playerControls.Player.Brake.ReadValue<float>();

        float h = driveSteer.x;
        float v = driveSteer.y;

        //only need to simulate smooth axis for keyboard, should be automatic for gamepad stick
        if (playerInput.currentControlScheme == "KeyboardMouse")
        {
            if (h == 0)
                smoothHorizontal = Mathf.MoveTowards(smoothHorizontal, 0f, Time.fixedDeltaTime * decreaseSmoothing);
            else
                smoothHorizontal = Mathf.MoveTowards(smoothHorizontal, h, Time.fixedDeltaTime * decreaseSmoothing);
            smoothHorizontal = Mathf.Clamp(smoothHorizontal, -1f, 1f);
        }
        else
            smoothHorizontal = h;


        /*
        // pass the input to the scooter!
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxisRaw("Drive");
        float handbrake = Input.GetAxis("Brake");
        float speedAdjustment = Input.GetAxisRaw("SpeedAdjustment");
        */

        m_Scooter.Move(smoothHorizontal, v , speedAdjustment, handbrake);
    }

    private void OnDestroy()
    {
        playerControls.Player.Disable();
    }
}

