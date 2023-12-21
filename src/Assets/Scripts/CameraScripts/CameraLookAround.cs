using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraLookAround : MonoBehaviour
{
    private PlayerControls.PlayerActions actions;

    public float rotationSpeed = 3f;
    public float rotateBackSpeed = 3f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    private float maxHorizontalView = 150f;
    private float maxVerticalView = 90f;

    private void Awake()
    {
        actions = ControlsInstance.GetActions();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1)) //Input with mouse
        {
            horizontalRotation += Input.GetAxis("Mouse X") * rotationSpeed;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -maxHorizontalView, maxHorizontalView);

            verticalRotation += Input.GetAxis("Mouse Y") * rotationSpeed * 0.7f;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalView, maxVerticalView);
        }
        else if (actions.LookAround.ReadValue<Vector2>() != Vector2.zero) //Input with gamepad
        {
            Vector2 looking = actions.LookAround.ReadValue<Vector2>();

            horizontalRotation += looking.x * rotationSpeed;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -maxHorizontalView, maxHorizontalView);

            verticalRotation += looking.y * rotationSpeed * 0.7f;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalView, maxVerticalView);
        }
        else //no input -> rotate back
        { 
            horizontalRotation = Mathf.MoveTowardsAngle(horizontalRotation, 0, rotateBackSpeed);
            verticalRotation = Mathf.MoveTowardsAngle(verticalRotation, 0, rotateBackSpeed);
        }

        Quaternion currentRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        transform.localRotation = currentRotation;  

    }
}
