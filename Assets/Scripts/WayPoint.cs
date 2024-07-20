using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EWayPointType 
{
    Patrol,
    Hide
}

public class WayPoint : MonoBehaviour
{
    public EWayPointType _wayPointType;

    void OnDrawGizmos() 
    {
        if (_wayPointType == EWayPointType.Patrol)
        {
            Gizmos.color = Color.green;
        }
        else if (_wayPointType == EWayPointType.Hide)
        {
            Gizmos.color = Color.blue;
        }

        var start = transform.position;
        var end = start + Vector3.up * 5f;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawIcon(end, "WayPoint");
    }
}
