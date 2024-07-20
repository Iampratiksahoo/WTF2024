public class FSM<TStateCharacter> where TStateCharacter : IStateCharacter
{
    private BaseState<TStateCharacter> _currentState;
    private BaseState<TStateCharacter> _previousState;
    TStateCharacter _owner;

    public FSM(TStateCharacter inOwner) 
    {
        _owner = inOwner;
    }
    public void SwitchState(BaseState<TStateCharacter> inNewState)
    {
        if (_currentState != null) 
        {
            _previousState = _currentState;
            _previousState.OnExit();
        }

        _currentState = inNewState;
        _currentState.OnEnter();
    }

    public void Tick()
    {
        _currentState.OnUpdate();
    }

    public TStateCharacter GetFSMOwner() => _owner;

    public BaseState<TStateCharacter> GetPreviousState() => _previousState;
    public BaseState<TStateCharacter> GetCurrentState() => _currentState;
}