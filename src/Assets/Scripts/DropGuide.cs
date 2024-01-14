using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropGuide : MonoBehaviour
{
    public Transform player;
    private Drop[] drops;

    public TextMeshProUGUI playerCoordinatesText;
    public TextMeshProUGUI closestDropText;

    private void Awake()
    {
        drops = GetComponent<GameController>().Drops;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 closestDrop = ComputeClosestDrop();

        playerCoordinatesText.text = "(" + Mathf.Round(player.position.x) + ", " + Mathf.Round(player.position.z) + ")";
        closestDropText.text = "(" + Mathf.Round(closestDrop.x) + ", " + Mathf.Round(closestDrop.z) + ")";
    }

    private Vector3 ComputeClosestDrop()
    {
        float closestDistance = float.PositiveInfinity;
        int closestDrop = 0;

        for (int i = 0; i < drops.Length; i++) 
        {
            if (!drops[i].done)
            {
                float curDistance = Vector3.Distance(player.position, drops[i].transform.position);
                if (curDistance < closestDistance) 
                {
                    closestDrop = i;
                    closestDistance = curDistance;
                }
            }
        }

        Vector3 closestPos = drops[closestDrop].transform.position;
        return closestPos;
    }
}
