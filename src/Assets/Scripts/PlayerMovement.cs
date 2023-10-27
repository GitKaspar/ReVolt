using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed = 25f;
    private float currentSpeed;
    private float targetSpeed;
    public float Acceleration = 5f;
    public float Deceleration = 7f;
    public float BrakingForce = 12f;

    private float turnVelocity;
    private float turnSmoothTime = 0.1f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetSpeed = 0f;
        currentSpeed = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Read current movement parameters
        currentSpeed = rb.velocity.magnitude;
        Vector3 currentMovement = rb.velocity;

        //Get Inputs
        float turn = Input.GetAxisRaw("Horizontal");
        float drive = Input.GetAxisRaw("Drive");
        float speedAdjustment = Input.GetAxisRaw("SpeedAdjustment");
        float brake = Input.GetAxisRaw("Brake");

        //Apply brake force if desired
        if (brake > 0f)
        {
            rb.AddForce(currentMovement.normalized * -BrakingForce, ForceMode.Acceleration);
        }

        //Adjust speed if W held
        if (drive > 0f)
        {
            Vector3 moveDir = Vector3.Normalize(drive * transform.forward);

            //Apply speed change to target  speed
            targetSpeed = Mathf.Clamp(targetSpeed + speedAdjustment, 0f, MaxSpeed);

            //Accelerate or decelerate depending on speeds
            if (SpeedsClose(currentSpeed, targetSpeed, 1f)) //stabilize constant speed
            {
                rb.velocity = rb.velocity.normalized * targetSpeed;
            }
            else if (targetSpeed > currentSpeed)
            {
                rb.AddForce(moveDir * Acceleration, ForceMode.Acceleration);

            }
            else if (targetSpeed < currentSpeed)
            {
                rb.AddForce(moveDir * -Deceleration, ForceMode.Acceleration);
            }
        }
        else //always keep targetSpeed on same level as currentSpeed if player doesn't want to actively drive
        {
            targetSpeed = currentSpeed;
        }

        //add turning force
        if (Mathf.Abs(turn) > 0.1f && currentSpeed > 0f)
        {
            Vector3 turnDir = Vector3.Normalize(turn * transform.right);
            rb.AddForce(turnDir * 10, ForceMode.Impulse);
        }

        float angleY = transform.eulerAngles.y;
        //Rotate in direction
        if (currentSpeed > 0.5f)
        {
            float targetAngleY = Mathf.Atan2(currentMovement.x, currentMovement.z) * Mathf.Rad2Deg;
            angleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnVelocity, turnSmoothTime);
            float targetAngleZ = -30 * Vector3.Dot(transform.right, currentMovement) * ((MaxSpeed * 1.75f - currentSpeed) / (MaxSpeed * 1.75f));
            transform.rotation = Quaternion.Euler(0f, angleY, targetAngleZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        }



        //Clamp velocity to not exceed MaxSpeed
        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
        }

        //Debug.Log("speed: " + currentSpeed);
    }


    private bool SpeedsClose(float currentSpeed, float targetSpeed, float delta)
    {
        float diff = Mathf.Abs(currentSpeed - targetSpeed);

        if (diff < delta)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
