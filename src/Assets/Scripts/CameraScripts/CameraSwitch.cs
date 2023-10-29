using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject ThirdCam;
    public GameObject FirstCam;
    public bool firstActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CameraSwitch"))
        {
            firstActive = !firstActive;

            StartCoroutine(CameraChange());
        }
    }

    IEnumerator CameraChange()
    {
        yield return new WaitForEndOfFrame();
        if (firstActive) 
        { 
            FirstCam.SetActive(true);
            ThirdCam.SetActive(false);
        }
        else
        {
            FirstCam.SetActive(false);
            ThirdCam.SetActive(true);
        }
    }
}
