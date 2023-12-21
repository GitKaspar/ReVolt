using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraLookAround : MonoBehaviour
{
    private PlayerControls playerControls;
    public float rotationSpeed = 3f;
    public float rotateBackSpeed = 3f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void OnDestroy()
    {
        playerControls.Player.Disable();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 looking = playerControls.Player.LookAround.ReadValue<Vector2>();

        if (looking != Vector2.zero)
        {
            horizontalRotation += looking.x * rotationSpeed;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -150f, 150f);

            verticalRotation += looking.y * rotationSpeed * 0.7f;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        }
        else
        {
            horizontalRotation = Mathf.MoveTowardsAngle(horizontalRotation, 0, rotateBackSpeed);
            verticalRotation = Mathf.MoveTowardsAngle(verticalRotation, 0, rotateBackSpeed);
        }

        Quaternion currentRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        transform.localRotation = currentRotation;

        /*
        if (Input.GetMouseButton(1))
        {
            horizontalRotation += Input.GetAxis("Mouse X") * rotationSpeed;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -150f, 150f);

            verticalRotation += Input.GetAxis("Mouse Y") * rotationSpeed * 0.7f;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        }

        if (!Input.GetMouseButton(1)) 
        {
            horizontalRotation = Mathf.MoveTowardsAngle(horizontalRotation, 0, rotateBackSpeed);
            verticalRotation = Mathf.MoveTowardsAngle(verticalRotation, 0, rotateBackSpeed);
        }

        Quaternion currentRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        transform.localRotation = currentRotation;
        */

    }
    

    public void Look(InputAction.CallbackContext context)
    {
        Debug.Log("Look, ma!");
        Vector2 looking = context.ReadValue<Vector2>();

        if (looking != Vector2.zero) 
        {
            horizontalRotation += looking.x * rotationSpeed;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -150f, 150f);

            verticalRotation += looking.y * rotationSpeed * 0.7f;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        }
        else
        {
            horizontalRotation = Mathf.MoveTowardsAngle(horizontalRotation, 0, rotateBackSpeed);
            verticalRotation = Mathf.MoveTowardsAngle(verticalRotation, 0, rotateBackSpeed);
        }

        Quaternion currentRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        transform.localRotation = currentRotation;
    }
}
