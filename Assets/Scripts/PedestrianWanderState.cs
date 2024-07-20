using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianWanderState : BaseState<Pedestrian> 
{
    WayPoint _randomPatrolPoint = null;
    float _wanderRadius;
    float _speed;
    Dictionary<Func<bool>, BaseState<Pedestrian>> _transitioningFunctions;

    public PedestrianWanderState(FSM<Pedestrian> inCtx) : base(inCtx)
    {
        // Set the movement speed
        _wanderRadius = _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
        _speed = _ctx.GetFSMOwner()._pedestrianData.walkSpeed;
        _transitioningFunctions = new() {
            { HasReachedRandomPoint, _ctx.GetFSMOwner().IdleState }
        };
    }

    public override void OnEnter()
    {
        _randomPatrolPoint = WayPointManager.Instance.GetRandomPatrolPoint();                                               // Get a random point
        NavMesh.SamplePosition(
            _randomPatrolPoint.transform.position, 
            out NavMeshHit hitInfo, 
            900f, 
            NavMesh.AllAreas
        );                                                                                                                  // Sample position to navmesh
        _ctx.GetFSMOwner()._agent.speed = _speed;                                                                           // Set the agent speed
        _ctx.GetFSMOwner()._agent.stoppingDistance = _wanderRadius;                                                         // Set the acceptance radius to stop the agent at a radius away from the point
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;                                                           // Move to that projected position
    }

    public override void OnExit()
    {
        Debug.Log("Exiting wander state");
        _randomPatrolPoint = null;
    }

    public override void OnUpdate()
    {
        foreach (var kv in _transitioningFunctions) {
            if (kv.Key() == true) {
                _ctx.SwitchState(kv.Value);
            }
        }
    }

    bool HasReachedRandomPoint() {
        return _randomPatrolPoint != null &&
        Vector3.Distance(_randomPatrolPoint.transform.position, _ctx.GetFSMOwner().transform.position) < _wanderRadius;
    }
}
