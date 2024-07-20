using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianWanderState : BaseState<Pedestrian> 
{
    WayPoint _randomPatrolPoint = null;
    Dictionary<Func<bool>, BaseState<Pedestrian>> _transitioningFunctions;

    public PedestrianWanderState(FSM<Pedestrian> inCtx) : base(inCtx)
    {
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
        _ctx.GetFSMOwner()._animator.SetBool("Walking", true);                                                              // Start the walking animation
        _ctx.GetFSMOwner()._agent.speed = _ctx.GetFSMOwner()._pedestrianData.walkSpeed;                                     // Set the agent speed
        _ctx.GetFSMOwner()._agent.stoppingDistance = _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;                   // Set the acceptance radius to stop the agent at a radius away from the point
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;                                                           // Move to that projected position
    }

    public override void OnExit()
    {
        Debug.Log("Exiting wander state");
        _ctx.GetFSMOwner()._animator.SetBool("Walking", false);
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
        _ctx.GetFSMOwner()._agent.stoppingDistance <= _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
        // Vector3.Distance(_randomPatrolPoint.transform.position, _ctx.GetFSMOwner().transform.position) < _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
    }
}
