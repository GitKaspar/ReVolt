using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [HideInInspector]
    public bool HasEntered;
    void Start()
    {
        HasEntered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player in da " + gameObject.name + "box!");
            HasEntered = true;
        }

    }
}
