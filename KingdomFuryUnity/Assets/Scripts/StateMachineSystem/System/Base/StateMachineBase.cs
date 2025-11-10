using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour
{
    public TypeReference[] statesRefs;
    public Type test;
    
    // States list
    private Dictionary<Type, StateBase> _statesMap;
    
    private StateBase _currentState;
    private StateBase _nextState;
    
    private void Start()
    {
        OnStart();
        StateMachineStart();
    }

    private void Update()
    {
        OnUpdate();
        StateMachineUpdate();
    }
    
    public virtual void OnStart() {}
    public virtual void OnUpdate() {}
    
    
    private void StateMachineStart()
    {
        _statesMap = new Dictionary<Type, StateBase>();
        
        foreach (TypeReference typeRef in statesRefs)
        {
            Type type =  typeRef.Type;
            
            if (type != typeof(StateBase) && type.IsSubclassOf(typeof(StateBase)))
            {
                _statesMap.Add(type, (StateBase) Activator.CreateInstance(type, new object[] { this }));
            }
        }
        
        _currentState = _statesMap[statesRefs[0].Type];
        _nextState = _currentState;
        
        _currentState.OnEnter();
    }
    
    private void StateMachineUpdate()
    {
        // If nextState is different change state
        if (!_nextState.Equals(_currentState) && !_currentState.IsBusy())
        {
            print("CHANGING TO: " + _currentState.GetType().Name);
            
            _currentState.OnExit();
            _currentState = _nextState;
            _nextState.OnEnter();
        }
        else
        {
            print("UPDATE: " + _currentState.GetType().Name);
            _currentState.UpdateState();
            _currentState.CheckTransitions();
        }
    }

    public void ChangeState(Type newState)
    {
        print("Changing state: " + newState.GetType().Name);
        _nextState = _statesMap[newState];
    }
}
