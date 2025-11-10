using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachineBase
{

    public enum States
    {
        Idle,
        Moving,
    }
    
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
        
        
        // ---- StateBase Machine Fields ----
        
        // StateBases list
        public Dictionary<Type, StateBase> _StateBasesMap;
        
        public StateBase _currentStateBase;
        public StateBase _nextStateBase;
        
        
        // ---- StateBase Machine StateBase Fields ----
        public bool _isMoving;
        public bool _direction;
        public bool _disableInput;
        
        
        public override void OnStart()
        {
            // Get Player Components
            _rb = GetComponent<Rigidbody2D>();
            _movementValidator = GetComponent<MovementValidator>();
            _animator = GetComponent<Animator>();
        
            _spriteRenderer = transform.Find("Render").GetComponent<SpriteRenderer>();
        
            _gridPosition = new Vector3Int(0, 0, 0);
        }

        public override void OnUpdate()
        {
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
                _onMoveInput.x = 0;
            }
            
            _isMoving = !_onMoveInput.Equals(Vector2.zero);
        }
    
}
    