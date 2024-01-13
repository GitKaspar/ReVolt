using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battery : MonoBehaviour
{
    public float MaxCapacity;
    public float DepletionRate;

    [HideInInspector]
    public float CurrentCapacity;

    // Start is called before the first frame update
    void Start()
    {
        MaxCapacity = UpgradeStats.Instance.GetCurrentValue(StatName.Battery);
        CurrentCapacity = MaxCapacity;
        if(SceneManager.GetActiveScene().name == "TutorialScene")
        {
            DepletionRate = 0.8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = Events.RequestCurrentSpeed();
        CurrentCapacity -= DepletionRate * currentSpeed * Time.deltaTime;
        CurrentCapacity = Mathf.Clamp(CurrentCapacity, 0, MaxCapacity);

        if (CurrentCapacity < float.Epsilon)
        {
            BatteryEmpty();
        }
    }

    public void BatteryEmpty()
    {
        Events.BatteryLow(true);
    }
}
