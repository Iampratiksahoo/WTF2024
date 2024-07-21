using UnityEngine;
using UnityEngine.AI;

public class PedestrianHideState : BaseState<Pedestrian>
{
    Vector2 fearResetTimer = new(5, 10);
    float randomResetTimer = 0f;
    float currentResetTickTimer = 0f;
    WayPoint fleeWayPoint = null;

    public PedestrianHideState(FSM<Pedestrian> inCtx) : base(inCtx)
    {
    }

    public override void OnEnter() {
        randomResetTimer = Random.Range(fearResetTimer.x, fearResetTimer.y);
        Debug.Log("Going to a hiding spot: " + _ctx.GetFSMOwner().transform.name);
        fleeWayPoint = WayPointManager.Instance.GetRandomHidePoint();
        NavMesh.SamplePosition(
            fleeWayPoint.transform.position, 
            out NavMeshHit hitInfo, 
            900f, 
            NavMesh.AllAreas
        );
        _ctx.GetFSMOwner()._animator.SetBool("Walking", false);        
        _ctx.GetFSMOwner()._animator.SetBool("Running", true);
        _ctx.GetFSMOwner()._agent.speed = _ctx.GetFSMOwner()._pedestrianData.runSpeed;
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;
    }

    public override void OnExit() {
        // _ctx.GetFSMOwner()._agent.stoppingDistance = _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
        _ctx.GetFSMOwner()._animator.SetBool("Running", false);
        _ctx.GetFSMOwner()._animator.SetBool("Walking", false);
        _ctx.GetFSMOwner()._agent.ResetPath();
        currentResetTickTimer = 0f;
        randomResetTimer = 0f;
    }

    public override void OnUpdate() {
        if (Vector3.Distance(_ctx.GetFSMOwner().transform.position, fleeWayPoint.transform.position) <= _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius) {
            _ctx.GetFSMOwner()._animator.SetBool("Running", false);
            if (currentResetTickTimer >= randomResetTimer) {
                _ctx.SwitchState(_ctx.GetFSMOwner().IdleState);
                currentResetTickTimer = 0f;
            }
            currentResetTickTimer += Time.deltaTime;
        }
    }
}
