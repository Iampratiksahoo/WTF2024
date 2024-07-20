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
        );                                                                                                                  // Sample position to navmesh
        _ctx.GetFSMOwner()._animator.SetBool("Running", true);                                                              // Start the running animation
        _ctx.GetFSMOwner()._agent.speed = _ctx.GetFSMOwner()._pedestrianData.runSpeed;                                      // Set the agent speed
        _ctx.GetFSMOwner()._agent.stoppingDistance = _ctx.GetFSMOwner()._pedestrianData.acceptanceRadius;                   // Set the acceptance radius to stop the agent at a radius away from the point
        _ctx.GetFSMOwner()._agent.destination = hitInfo.position;                                                           // Move to that projected position
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }
}
