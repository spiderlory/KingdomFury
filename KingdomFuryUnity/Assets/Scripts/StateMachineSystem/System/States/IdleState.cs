using System;
using UnityEngine;

public class IdleState : StateBase
{
    private Type _movingStateType;
    private PlayerStateMachine _stm;

    public IdleState(StateMachineBase stm) : base(stm)
    {
        _stm = stm as PlayerStateMachine;
        _movingStateType = typeof(MovingState);
    }
        
    public override void CheckTransitions()
    {
        if (_stm._isMoving)
        {
            _stm.ChangeState(_movingStateType);
        }
    }
}

