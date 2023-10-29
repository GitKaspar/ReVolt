using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private bool droppable;
    [HideInInspector]
    public bool done;

    // Update is called once per frame
    void Update()
    {
        if (droppable && !done)
        {
            if (Input.GetButtonDown("Drop"))
            {
                DoDrop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            droppable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            droppable = false;
        }
    }

    private void DoDrop()
    {
        done = true;
        Events.DropDone(this);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var rend in renderers)
        {
            rend.material.color = Color.green;
        }
    }
}
