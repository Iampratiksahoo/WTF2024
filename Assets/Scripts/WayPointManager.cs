using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class WayPointManager : MonoBehaviour
{
    Dictionary<EWayPointType, List<WayPoint>> _wayPointMap = new Dictionary<EWayPointType, List<WayPoint>>();

    public static WayPointManager Instance { get; private set; }

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    void Start()
    {
        GetAllWayPoints();
    }

    public WayPoint GetRandomPatrolPoint() 
    {
        return _wayPointMap[EWayPointType.Patrol][Random.Range(0, _wayPointMap[EWayPointType.Patrol].Count)];
    }

    public WayPoint GetRandomHidePoint() 
    {
        return _wayPointMap[EWayPointType.Hide][Random.Range(0, _wayPointMap[EWayPointType.Hide].Count)];
    }

    public WayPoint GetClosestPatrolPoint(Vector3 from)
    {
        var wps = _wayPointMap[EWayPointType.Patrol];
        var closest = wps[0];
        var closestDist = Vector3.SqrMagnitude(wps[0].transform.position - from);
        wps.ForEach(x => {
            var newClosestDist = Vector3.SqrMagnitude(x.transform.position - from);
            if (newClosestDist < closestDist) 
            {
                closest = x;
                closestDist = newClosestDist;
            }
        });
        return closest;
    }

    public WayPoint GetClosestHidePoint(Vector3 from)
    {
        var wps = _wayPointMap[EWayPointType.Hide];
        var closest = wps[0];
        var closestDist = Vector3.SqrMagnitude(wps[0].transform.position - from);
        wps.ForEach(x => {
            var newClosestDist = Vector3.SqrMagnitude(x.transform.position - from);
            if (newClosestDist < closestDist) 
            {
                closest = x;
                closestDist = newClosestDist;
            }
        });
        return closest;
    }

    private void GetAllWayPoints()
    {
        var objects = FindObjectsByType(typeof(WayPoint), FindObjectsSortMode.None);
        objects.ToList().ForEach(x =>
        {
            var wp = x.GetComponent<WayPoint>();
            if (_wayPointMap.ContainsKey(wp._wayPointType))
            {
                _wayPointMap[wp._wayPointType].Add(wp);
            }
            else
            {
                _wayPointMap[wp._wayPointType] = new List<WayPoint>() { wp };
            }
        });
    }
}
