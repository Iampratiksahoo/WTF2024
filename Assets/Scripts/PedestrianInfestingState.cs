using UnityEngine;

public class PedestrianInfestingState : BaseState<Pedestrian>
{
    public PedestrianInfestingState(FSM<Pedestrian> inCtx) : base(inCtx) {
    }

    public override void OnEnter() {
        // TODO (satweek): Play the getting infested animation
    }

    public override void OnExit() {
    }

    public override void OnUpdate() {
    }
}
