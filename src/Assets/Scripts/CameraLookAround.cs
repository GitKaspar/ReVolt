using UnityEngine;

public class CameraLookAround : MonoBehaviour
{
    public float rotationSpeed = 10f;

    private float rotationOffset = 0f;

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            rotationOffset += Input.GetAxis("Mouse X") * rotationSpeed;
            rotationOffset = Mathf.Clamp(rotationOffset, -90f, 90f);
        }

        if (!Input.GetMouseButton(1)) 
        {
            rotationOffset = Mathf.MoveTowardsAngle(rotationOffset, 0, rotationSpeed / 3);
        }

        Quaternion currentRotation = Quaternion.Euler(0, rotationOffset, 0);
        transform.localRotation = currentRotation;

    }
}
