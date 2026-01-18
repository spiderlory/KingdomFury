using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachineBase
{
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

///
/// GAME OBJECT
///     STATE_MACHINE -> INTERFACE TO CHANGE STATE (other components change the state) (Manages only the change state logic? yes if it support a way to block the state. Some state may need to end in order to go to the next state)
///         
///
/// Two kinds of states. Normal and transitionals. If a state is transitional the state machine needs to stay in that state until it ends.
/// Each state has:
///     logic
///     transition
///
/// Keeping logic and transitions together give more flexibility on what the state wants to do.
/// Transitions needs a condition, this can be implemented inside the state or outside. If outside, the state has less controll on what it can do. To mitigate that it's possible to use a freeze mechanism.
/// The main problem is for states that needs to end. If the state changes while they are still going there can be problems
///
/// PROBLEMS:
///     states that needs to end a task being interrupted mid execution -> solution: freeze mechanic or is busy function. If the state is busy the state machine doesn't change.
///     logic can be put in a single place (all inside the state) or divided based on role (each component check the current state and take an action) (more expensive)
///
///     states needs to be able to access player components easily -> shared memory: all the items gets saved in a single class (for example the state machine), and the state can access it. This class needs to be readonly and contains only user components
///                                                                   each state gets the components directly from the player (expensive at startup, data replication)
///
///                                                                   so a shared memory approach is the best
///
/// 
/// 