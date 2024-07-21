using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PedestrianManager : MonoBehaviour
{
    public static PedestrianManager Instance;
    public List<Pedestrian> _pedestrians = new List<Pedestrian>();
    public List<Pedestrian> deads = new();
    public GameProgressBar gameProgress;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    void Start() 
    {
        var objects = FindObjectsByType(typeof(Pedestrian), FindObjectsSortMode.None);
        objects.ToList().ForEach(x => {
            var ped = x.GetComponent<Pedestrian>();
            _pedestrians.Add(ped);
        });
    }

    void Update()
    {
        for (var i = 0; i < _pedestrians.Count; i++) 
        {
            _pedestrians[i].MyTick();
        }

        gameProgress?.SetProgress(deads.Count, _pedestrians.Count + deads.Count);
    }

    public void Register(Pedestrian other) {
        if (!_pedestrians.Contains(other)) {
            _pedestrians.Add(other);
        }
    }

    public void UnRegister(Pedestrian other, bool isZombie = false) {
        if (isZombie) {
            ZombieManager.Instance.RemoveZombie(other);
        }

        if (_pedestrians.Contains(other)) {
            _pedestrians.Remove(other);
            deads.Add(other);
            // Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
