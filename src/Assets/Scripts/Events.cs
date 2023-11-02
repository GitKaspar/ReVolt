using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    //Global Game State related
    public static event Action<bool> OnEndGame;
    public static void EndGame(bool isWin) => OnEndGame?.Invoke(isWin);


    //Requesting and Setting Attributes
    public static event Func<float> OnRequestCurrentSpeed;
    public static float RequestCurrentSpeed() => OnRequestCurrentSpeed?.Invoke() ?? 0f;

    public static event Func<float> OnRequestTopSpeed;
    public static float RequestTopSpeed() => OnRequestTopSpeed?.Invoke() ?? 0f;

    public static event Action<float> OnSetTopSpeed;
    public static void SetTopSpeed(float value) => OnSetTopSpeed?.Invoke(value);


    //Mechanics, gameplay related
    public static event Action<Drop> OnDropDone;
    public static void DropDone(Drop drop) => OnDropDone?.Invoke(drop);

    public static event Action<bool> OnBatteryLow;
    public static void BatteryLow(bool isLow) => OnBatteryLow?.Invoke(isLow);
}
