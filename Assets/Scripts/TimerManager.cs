using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {
    public static TimerManager Instance;
    public List<Timer> activeTimers = new List<Timer>();

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Update() {
        for (var i = 0; i < activeTimers.Count; i++) {
            activeTimers[i].TickTimer();
        }
    }

    public void RegisterTimer(Timer timer) {
        activeTimers.Add(timer);
    }

    public void UnRegisterTimer(Timer timer) {
        if (activeTimers.Contains(timer)) {
            activeTimers.Remove(timer);
        }
    }
}
