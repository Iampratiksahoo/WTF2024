using System;
using UnityEngine;

public class Timer
{
    public float duration;
    public float tickTimer;
    public Action callback;
    public bool canTick;

    public Timer(float inDuration, Action inAfterCallback) {
        Debug.LogWarning("Timer started");
        duration = inDuration;
        callback = inAfterCallback;
        tickTimer = 0f;
        canTick = true;
        TimerManager.Instance.RegisterTimer(this);
    }

    public void TickTimer() {
        if (!canTick)
            return;

        if (tickTimer >= duration) {
            Debug.LogWarning("Timer ended");
            TimerManager.Instance.UnRegisterTimer(this);
            canTick = false;
            callback?.Invoke();
        }
        tickTimer += Time.deltaTime;
    }
}
