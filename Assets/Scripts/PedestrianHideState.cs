using UnityEngine;
using UnityEngine.AI;

public class PedestrianHideState : BaseState<Pedestrian>
{
    public PedestrianHideState(FSM<Pedestrian> inCtx) : base(inCtx)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Going to a hiding spot: " + _ctx.GetFSMOwner().transform.name);
        var wp = WayPointManager.Instance.GetClosestHidePoint(_ctx.GetFSMOwner().transform.position);
        NavMesh.SamplePosition(
            wp.transform.position, 
            out NavMeshHit hitInfo, 
            900f, 
            NavMesh.AllAreas
        );
        _ctx.GetFSMOwner()._animator.SetBool("Walking", false);        
        _ctx.GetFSMOwner()._animator.SetBool("Running", true);
        _ctx.GetFSMOwner()._agent.speed = _ctx.GetFSMOwner()._pedestrianData.runSpeed;
        _ctx.GetFSMOwner()._agent.stoppingDistance = 0;
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;
    }

    public override void OnExit()
    {
        _ctx.GetFSMOwner()._animator.SetBool("Running", false);
        _ctx.GetFSMOwner()._agent.stoppingDistance = _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;
        _ctx.GetFSMOwner()._agent.ResetPath();
    }

    public override void OnUpdate()
    {
    }
}
