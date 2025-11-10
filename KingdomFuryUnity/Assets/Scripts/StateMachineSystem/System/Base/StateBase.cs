using System;
using UnityEngine;

public abstract class StateBase
{
    protected bool _busy;
    
    protected StateBase(StateMachineBase stm) { }

    public virtual void OnEnter() { }
    public virtual void UpdateState() { }
    public virtual void OnExit() { }
        
    public virtual void CheckTransitions() { }

    public bool IsBusy()
    {
        return _busy;
    }
}
