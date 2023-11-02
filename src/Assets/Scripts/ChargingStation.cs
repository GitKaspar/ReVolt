using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    public float ChargingRate;
    private bool playerInRange;
    private Battery battery;

    private bool batteryLow = false;

    private void Awake()
    {
        Events.OnBatteryLow += OnBatteryLow;
    }

    private void OnDestroy()
    {
        Events.OnBatteryLow -= OnBatteryLow;
    }

    public void OnBatteryLow(bool isLow) => batteryLow = isLow;


    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetButton("Charge"))
            {
                Charge();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            battery = other.GetComponent<Battery>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            battery = null;
        }
    }

    private void Charge()
    {
        if (battery == null) return;

        battery.CurrentCapacity += ChargingRate * Time.deltaTime;
        battery.CurrentCapacity = Mathf.Min(battery.MaxCapacity, battery.CurrentCapacity);

        if (batteryLow) Events.BatteryLow(false);
    }
}
