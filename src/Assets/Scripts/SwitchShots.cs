using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchShots : MonoBehaviour
{
    public Camera scooter;
    public GameObject cam;
    public bool scooterActive;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CameraChange());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            scooterActive = !scooterActive;

            StartCoroutine(CameraChange());
        }
    }

    IEnumerator CameraChange()
    {
        yield return new WaitForEndOfFrame();
        if (scooterActive)
        {
            scooter.enabled = true;
            cam.SetActive(false);
        }
        else
        {
            scooter.enabled = false;
            cam.SetActive(true);
        }
    }
}
