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
        _randomPatrolPoint = WayPointManager.Instance.GetRandomPatrolPoint();
        
        NavMesh.SamplePosition(
            _randomPatrolPoint.transform.position, 
            out NavMeshHit hitInfo, 
            900f, 
            NavMesh.AllAreas
        );
        _ctx.GetFSMOwner()._animator.SetBool("Walking", true);
        _ctx.GetFSMOwner()._agent.speed = _ctx.GetFSMOwner()._pedestrianData.walkSpeed;
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;
    }

    public override void OnExit()
    {
        Debug.Log("Exiting wander state");
        _ctx.GetFSMOwner()._animator.SetBool("Walking", false);
        _ctx.GetFSMOwner()._agent.ResetPath();
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
        Vector3.Distance(_randomPatrolPoint.transform.position, _ctx.GetFSMOwner().transform.position) < _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
    }
}
