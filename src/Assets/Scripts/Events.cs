using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event Action<bool> OnEndGame;
    public static void EndGame(bool isWin) => OnEndGame?.Invoke(isWin);
}
