using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
        // A reference to the CharacterController component
        private CharacterController controller;
        // A reference to the Camera component
        public Camera cam;
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
        // Calculate the movement vector based on the input, moveSpeed, and the camera's orientation
        Vector3 move = (cam.transform.right * horizontal + cam.transform.forward * vertical) * moveSpeed * Time.deltaTime;
        // Apply gravity to the movement vector
        move.y += Physics.gravity.y * Time.deltaTime;
        // Move the controller using the movement vector
        controller.Move(move);

        // Get the mouse input from the mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Calculate the pitch (rotation around x-axis) of the camera based on the mouseY input
        pitch -= mouseY;
        // Clamp the pitch to the min and max values
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotate the camera around the x-axis by the pitch value
        cam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        // Rotate the game object around the y-axis by the mouseX value
        transform.Rotate(Vector3.up * mouseX);
    }
}