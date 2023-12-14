using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    public float ChargingRate;
    private bool playerInRange;
    private Battery battery;
    private bool isCharging;
    private AudioSource source;

    private bool batteryLow = false;

    private void Awake()
    {
        Events.OnBatteryLow += OnBatteryLow;
        isCharging = false;
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

            if (Input.GetButtonDown("Charge") && !isCharging)
            {
                isCharging = true;
                source = SoundController.SoundInstance.Charge.Play();
            }

            if (isCharging)
            {
                Charge();
            }

        }

        if (Input.GetButtonUp("Charge"))
        {
            isCharging = false;
            if (source != null) source.Stop();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            battery = other.GetComponent<Battery>();
            GameController.GameControllerInstance.PromtText.text = "press 'c' to charge";
            GameController.GameControllerInstance.PromtPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            battery = null;
            GameController.GameControllerInstance.PromtPanel.SetActive(false);
            isCharging = false;
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
