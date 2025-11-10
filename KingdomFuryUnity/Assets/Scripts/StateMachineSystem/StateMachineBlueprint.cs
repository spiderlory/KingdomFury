using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateMachineBlueprint : MonoBehaviour
{
    // ****************
    // ----------------
    // ---- Fields ----
    // ----------------
    // ****************
    
    
    // ---- Custom Fields ----
    // Components
    public Rigidbody2D _rb;
    public MovementValidator _movementValidator;
    public Animator _animator;
    public SpriteRenderer _spriteRenderer;
    
    
    
    // Player Inputs
    public Vector2 _onMoveInput;
    
    // Player grid position
    public Vector3Int _gridPosition;
    
    // Player Animation vars
    public float _jumpAnimDuration;
    public float _jumpAnimationNormalizedTime;
    
    
    // ---- State Machine Fields ----
    
    // States list
    public Dictionary<Type, State> _statesMap;
    
    public State _currentState;
    public State _nextState;
    
    
    // ---- State Machine State Fields ----
    public bool _isMoving;
    public bool _direction;
    public bool _disableInput;



    public TypeReference test;
    
    
    
    
    
    
    // *****************
    // -----------------
    // ---- Methods ----
    // -----------------
    // *****************
    
    
    // ---- MonoBehaviour Methods ----
    private void Start()
    {
        // Get Player Components
        _rb = GetComponent<Rigidbody2D>();
        _movementValidator = GetComponent<MovementValidator>();
        _animator = GetComponent<Animator>();
        
        _spriteRenderer = transform.Find("Render").GetComponent<SpriteRenderer>();
        
        _gridPosition = new Vector3Int(0, 0, 0);
        
        StateMachineInit();
    }

    private void Update()
    {
        StateMachineUpdate();
        StateFieldsUpdate();

        if (_onMoveInput.x == -1 || _onMoveInput.y == 1)
        {
            _direction = true;
        } else if (_onMoveInput.x == 1 || _onMoveInput.y == -1)
        {
            _direction = false;
        }
        _spriteRenderer.flipX = _direction;
    }
    
    
    // ---- Player Input Methods -----
    private void OnMove(InputValue value)
    {
        _onMoveInput = value.Get<Vector2>();
    
        // Fix Input Sot that only x or y has a value
        if (_onMoveInput.x != 0 && _onMoveInput.y != 0)
        {
            _onMoveInput.y = 0;
        }
    }
    
    
    // ---- STATE MACHINE METHODS ----
    protected void StateMachineInit()
    {
        _statesMap = new Dictionary<Type, State>();
        
        Type stateMachineType = typeof(StateMachineBlueprint);
        Type[] nestedTypes = stateMachineType.GetNestedTypes();

        Type firstType = null;
        
        foreach (Type type in nestedTypes)
        {
            if (type != typeof(State) && type.IsSubclassOf(typeof(State)))
            {
                if (firstType == null)
                {
                    firstType = type;
                }
                _statesMap.Add(type, (State) Activator.CreateInstance(type, new object[] { this }));
            }
        }
        
        // Setting the first state
        if (firstType == null)
        {
            Destroy(this);
        }
        
        _currentState = _statesMap[firstType];
        _nextState = _currentState;
        
        _currentState.OnEnter();
    }
    
    private void StateMachineUpdate()
    {
        // If nextState is different change state
        if (!_nextState.Equals(_currentState))
        {
            //print("CHANGING TO: " + _currentState.GetType().Name);
            
            _currentState.OnExit();
            _currentState = _nextState;
            _nextState.OnEnter();
        }
        else
        {
            //print("UPDATE: " + _currentState.GetType().Name);
            _currentState.UpdateState();
            _currentState.CheckTransitions();
        }
    }

    private void StateFieldsUpdate()
    {
        _isMoving = !_onMoveInput.Equals(Vector2.zero);

    }

    public void ChangeState(Type newState)
    {
        _nextState = _statesMap[newState];
    }
    
    
    
    
    
    
    
    
    
    
    // ***************************
    // ---------------------------
    // ---- STATES DEFINITION ----
    // ---------------------------
    // ***************************
    
    public abstract class State
    {
        protected StateMachineBlueprint _STM;
        
        protected State(StateMachineBlueprint stm)
        {
            _STM = stm;
        }

        public virtual void OnEnter() { }
        public virtual void UpdateState() { }
        public virtual void OnExit() { }
        
        public virtual void CheckTransitions() { }
    }
    
    public class IdleState : State
    {

        private Type _movingStateType;

        public IdleState(StateMachineBlueprint stm) : base(stm)
        {
            _movingStateType = typeof(MovingState);
        }
        
        public override void CheckTransitions()
        {
            if (_STM._isMoving)
            {
                _STM.ChangeState(_movingStateType);
            }
        }
    }
    
    public class MovingState : State
    {
        private Vector3 _startingPosition;
        private Vector3 _targetPosition;
        private Vector3 _currentPosition;
        
        
        private Vector3 _isometricX = new Vector3(1, 0.5f, 0) / 2;
        private Vector3 _isometricY = new Vector3(1, -0.5f, 0) / 2;
        private Vector3 _isometricZ = new Vector3(0, 0.25f, 1);
        
        private Animator _animator;
        private Transform _transform;

        private AnimatorStateInfo _animatorTransitionInfo;

        private Type _idleStateType;

        public MovingState(StateMachineBlueprint stm) : base(stm)
        {
            _animator = _STM._animator;
            _transform = _STM.transform;
            
            _idleStateType = typeof(IdleState);
        }

        public override void OnEnter()
        {
            _animator.SetBool("IsMoving", true);
                
            _startingPosition = _transform.position;
            _targetPosition = GetTargetPosition();
        }

        public override void UpdateState()
        {
            _animatorTransitionInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
            if (_animatorTransitionInfo.IsName("Move") && !_animator.IsInTransition(0))
            {
                if (_animatorTransitionInfo.normalizedTime <= 1.5)
                {
                    _currentPosition = Vector3.Lerp(_startingPosition, _targetPosition, _animatorTransitionInfo.normalizedTime);
                    _transform.position = _currentPosition;
                }
                else
                {
                    _transform.position = _targetPosition;
                
                    _animator.SetBool("IsMoving", false);
                    _STM.ChangeState(_idleStateType);
                }
            }
        }

        
        private Vector3 GetTargetPosition()
        {
            Vector3 targetPosition = _STM.transform.position;
        
            Vector2Int intInput = new Vector2Int(
                _STM._onMoveInput.x != 0? (int) Mathf.Sign(_STM._onMoveInput.x): 0,
                _STM._onMoveInput.y != 0? (int) Mathf.Sign(_STM._onMoveInput.y): 0
            );
        
            Vector3Int correctedDirection = _STM._movementValidator.GetValidDirection(_STM._gridPosition, intInput);
        
            _STM._gridPosition += correctedDirection;

            if (!correctedDirection.Equals(Vector3Int.zero))
            {
                if (_STM._onMoveInput.x != 0) // Horizontal Movement
                {
                    //_rb.MovePosition(_rb.position + _isometricX * Mathf.Sign(_onMoveInput.x));
                    targetPosition += _isometricX * Mathf.Sign(correctedDirection.x);
                } else if (_STM._onMoveInput.y != 0) // Vertical Movement
                {
                    targetPosition -= _isometricY * Mathf.Sign(correctedDirection.y);
                }

                if (correctedDirection.z != 0)
                {
                    targetPosition += _isometricZ * Mathf.Sign(correctedDirection.z);
                }
            }
        
            return targetPosition;
        }
    }
}