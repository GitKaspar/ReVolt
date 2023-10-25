using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // A reference to the CharacterController component
    private CharacterController controller;
    // A reference to the Camera component
   private Camera cam;
    // The speed of the player movement in meters per second
    public float moveSpeed = 5f;
    // The sensitivity of the mouse look in degrees per pixel
    public float mouseSensitivity = 2f;
    // The minimum and maximum angles for the camera rotation
    public float minPitch = -80f;
    public float maxPitch = 80f;
    // The current pitch (rotation around x-axis) of the camera
    private float pitch = 0f;

    void Start()
    {
        // Get the CharacterController component attached to this game object
        controller = GetComponent<CharacterController>();
        // Get the Camera component attached to this game object or its children
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Get the horizontal and vertical input from the keyboard
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Calculate the movement vector based on the input and moveSpeed
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move *= moveSpeed * Time.deltaTime;
        // Apply gravity to the movement vector
        move.y += Physics.gravity.y * Time.deltaTime;
        // Move the controller using the movement vector
        controller.Move(move);

        // Get the mouse input from Input.GetAxis()
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // Calculate the pitch angle based on mouseY and mouseSensitivity
        pitch -= mouseY * mouseSensitivity;
        // Clamp pitch angle between minPitch and maxPitch
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        // Apply pitch angle to cam.transform.localEulerAngles.x
        cam.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        // Calculate yaw angle based on mouseX and mouseSensitivity
        float yaw = mouseX * mouseSensitivity;
        // Apply yaw angle to transform.eulerAngles.y
        transform.eulerAngles += new Vector3(0f, yaw, 0f);
    }
}