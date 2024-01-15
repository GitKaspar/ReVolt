using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = ProgressManager.Instance.revolutionMessage;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);//+ camera.transform.rotation * Vector3.forward);
        transform.Rotate(0, 180, 0);
    }
}
