using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStateName 
{
    Idle, Wander, Follow, WayPointFollow
}

public abstract class BaseState<TOwnerCharacter> where TOwnerCharacter : IStateCharacter
{
    protected FSM<TOwnerCharacter> _ctx;

    public BaseState(FSM<TOwnerCharacter> inCtx)
    {
        _ctx = inCtx;
    }

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();
}