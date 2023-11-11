using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDisplay : MonoBehaviour
{
    public float RotationSpeed;
    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }
}
