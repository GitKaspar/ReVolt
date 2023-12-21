using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingStation : MonoBehaviour
{
    private PlayerControls.PlayerActions actions;

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

        actions = ControlsInstance.GetActions();
        actions.Charge.started += Charge_started;
        actions.Charge.canceled += Charge_canceled;
    }



    private void OnDestroy()
    {
        Events.OnBatteryLow -= OnBatteryLow;
    }

    public void OnBatteryLow(bool isLow) => batteryLow = isLow;

    private void Charge_started(InputAction.CallbackContext ctx)
    {
        if (playerInRange && !isCharging)
        {
            isCharging = true;
            source = SoundController.SoundInstance.Charge.Play();
        }
    }

    private void Charge_canceled(InputAction.CallbackContext octx)
    {
        isCharging = false;
        if (source != null) source.Stop();
    }



    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {
            Charge();
        }

        /*
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
        */

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
            if (source != null) source.Stop();
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
