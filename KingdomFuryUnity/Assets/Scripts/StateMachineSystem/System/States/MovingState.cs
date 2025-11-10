using System;
using UnityEngine;

public class MovingState : StateBase
    {
        private Vector3 _startingPosition;
        private Vector3 _targetPosition;
        private Vector3 _currentPosition;

        private float _normalizedTime;
        
        
        private Vector3 _isometricX = new Vector3(1, 0.5f, 0) / 2;
        private Vector3 _isometricY = new Vector3(1, -0.5f, 0) / 2;
        private Vector3 _isometricZ = new Vector3(0, 0.25f, 1);
        
        private Animator _animator;
        private Transform _transform;

        private AnimatorStateInfo _animatorTransitionInfo;

        private Type _idleStateType;
        
        private PlayerStateMachine _stm;

        public MovingState(StateMachineBase stm) : base(stm)
        {
            _stm = stm as  PlayerStateMachine;
            
            _animator = _stm._animator;
            _transform = _stm.transform;
            
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
            _normalizedTime = _animatorTransitionInfo.normalizedTime;
            
        
            if (_animatorTransitionInfo.IsName("Move") && !_animator.IsInTransition(0))
            {
                if (_normalizedTime <= 1.5)
                {
                    _currentPosition = Vector3.Lerp(_startingPosition, _targetPosition, _normalizedTime);
                    _transform.position = _currentPosition;
                }
                else
                {
                    _transform.position = _targetPosition;
                
                    _animator.SetBool("IsMoving", false);
                    _stm.ChangeState(_idleStateType);
                }
            }
        }
        
        private Vector3 GetTargetPosition()
        {
            Vector3 targetPosition = _stm.transform.position;
        
            Vector2Int intInput = new Vector2Int(
                _stm._onMoveInput.x != 0? (int) Mathf.Sign(_stm._onMoveInput.x): 0,
                _stm._onMoveInput.y != 0? (int) Mathf.Sign(_stm._onMoveInput.y): 0
            );
        
            Vector3Int correctedDirection = _stm._movementValidator.GetValidDirection(_stm._gridPosition, intInput);
        
            _stm._gridPosition += correctedDirection;

            if (!correctedDirection.Equals(Vector3Int.zero))
            {
                if (_stm._onMoveInput.x != 0) // Horizontal Movement
                {
                    //_rb.MovePosition(_rb.position + _isometricX * Mathf.Sign(_onMoveInput.x));
                    targetPosition += _isometricX * Mathf.Sign(correctedDirection.x);
                } else if (_stm._onMoveInput.y != 0) // Vertical Movement
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