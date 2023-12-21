using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float forwardForce;
    public float upForce;
    private Rigidbody rb;

    public float Cooldown = 1.5f;
    private float timeLast;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnJump()
    {
        if (Time.time - timeLast >= Cooldown)
        {
            rb.AddForce(transform.forward * forwardForce + Vector3.up * upForce, ForceMode.VelocityChange);
            timeLast = Time.time;
        }
    }
}
