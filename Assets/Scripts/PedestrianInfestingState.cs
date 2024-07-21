using UnityEngine;

public class PedestrianInfestingState : BaseState<Pedestrian>
{
    public PedestrianInfestingState(FSM<Pedestrian> inCtx) : base(inCtx) {
    }

    public override void OnEnter() {
        // TODO (satweek): Play the getting infested animation
        _ctx.GetFSMOwner()._agent.isStopped = true;
    }

    public override void OnExit() {
        _ctx.GetFSMOwner()._agent.ResetPath();
        _ctx.GetFSMOwner()._agent.isStopped = false;
    }

    public override void OnUpdate() {
    }
}
