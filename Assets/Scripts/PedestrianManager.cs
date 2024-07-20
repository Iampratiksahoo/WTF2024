using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PedestrianManager : MonoBehaviour
{
    public List<Pedestrian> _pedestrians = new List<Pedestrian>();

    void Start() 
    {
        var objects = FindObjectsByType(typeof(Pedestrian), FindObjectsSortMode.None);
        objects.ToList().ForEach(x => _pedestrians.Add(x.GetComponent<Pedestrian>()));
    }

    void Update()
    {
        for (var i = 0; i < _pedestrians.Count; i++) 
        {
            _pedestrians[i].MyTick();
        }
    }
}
