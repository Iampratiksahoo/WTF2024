using UnityEngine;

public class PedestrianIdleState : BaseState<Pedestrian>
{
    float _currentWanderTimer;

    public PedestrianIdleState(FSM<Pedestrian> inCtx) : base(inCtx)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Entering idle state");
        new Timer(_ctx.GetFSMOwner()._pedestrianData.wanderTimer, () => {
            _ctx.SwitchState(_ctx.GetFSMOwner().WanderState);
        });
    }

    public override void OnExit()
    {
        _currentWanderTimer = 0f;
        Debug.Log("Exiting idle state");
    }

    public override void OnUpdate()
    {
        /** 
        if (_currentWanderTimer >= _ctx.GetFSMOwner()._pedestrianData.wanderTimer) {
            _ctx.SwitchState(_ctx.GetFSMOwner().WanderState);
        }
        _currentWanderTimer += Time.deltaTime;
        */
    }
}