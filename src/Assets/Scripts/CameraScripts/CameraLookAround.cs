using UnityEngine;

public class CameraLookAround : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public float rotateBackSpeed = 3f;

    private float horizontalRotation = 0f;
    private float verticalRotation = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
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

    }
}
