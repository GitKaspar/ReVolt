using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public Material MatDropped;
    public Material MatOpen;
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
        Debug.Log("Hello");
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
        Debug.Log("Drop done");
        done = true;
        Events.DropDone(this);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (var rend in renderers)
        {
            rend.material = MatDropped;
        }
    }
}
