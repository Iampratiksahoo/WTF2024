using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;

    public List<IThreat> _affectedZombies = new();

    void Awake() {
        if (Instance == null)
            Instance = this;
        
        DontDestroyOnLoad(this);
    }

    public void RemoveZombie(IThreat threat) {
        if (_affectedZombies.Contains(threat)) {
            _affectedZombies.Remove(threat);
        }
    }
}
