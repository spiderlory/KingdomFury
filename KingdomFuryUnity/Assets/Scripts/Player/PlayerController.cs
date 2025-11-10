using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Public Vars
    //   Player Stats
    
    
    // Player States
    enum PlayerState
    {
        standing,
        moving
    }
    
    private PlayerState _currentState;
    
    private bool _canMove = false;
    private bool _isMoving = false;

    
    // Components
    private Rigidbody2D _rb;
    private MovementValidator _movementValidator;
    private Animator _animator;
    
    
    // Player Inputs
    private Vector2 _onMoveInput;
    
    
    // Player grid position
    private Vector3Int _gridPosition;

    private Vector2 _moveInitialPosition;
    
    // Player Animation vars
    private float _jumpAnimDuration;
    private float _jumpAnimationNormalizedTime;
    
    // Player movement vars
    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _currentPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Player Components
        _rb = GetComponent<Rigidbody2D>();
        _movementValidator = GetComponent<MovementValidator>();
        _animator = GetComponent<Animator>();
        
        // Setting Initial Player state
        _currentState =  PlayerState.standing;
        
        _gridPosition = new Vector3Int(0, 0, 0);

        _canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
        {
            UpdatePosition();
        }
        else if (_canMove)
        {
            // wants to move
            if (!_onMoveInput.Equals(Vector2.zero))
            {
                _currentState = PlayerState.moving;
                _animator.SetBool("IsMoving", true);
                
                _startingPosition = transform.position;
                _targetPosition = GetTargetPosition();

                _canMove = false;
                _isMoving = true;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }


    private void UpdatePosition()
    {
        AnimatorStateInfo animatorTransitionInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (animatorTransitionInfo.IsName("Jump") && !_animator.IsInTransition(0))
        {
            if (animatorTransitionInfo.normalizedTime <= 1)
            {
                _currentPosition = Vector3.Lerp(_startingPosition, _targetPosition, animatorTransitionInfo.normalizedTime);
                transform.position = _currentPosition;
            }
            else
            {
                transform.position = _targetPosition;
                _isMoving = false;

                StartCoroutine(ResetCanMove());
                
                _animator.SetBool("IsMoving", false);
            }
        }
    }
    
    private void OnMove(InputValue value)
    {
        _onMoveInput = value.Get<Vector2>();
    
        if (_onMoveInput.x != 0 && _onMoveInput.y != 0)
        {
            _onMoveInput.y = 0;
        }

        if (_onMoveInput.Equals(Vector2.zero))
        {
            _currentState = PlayerState.standing;
        }
    }

    // New MovementBase
    private Vector3 _isometricX = new Vector3(1, 0.5f, 0) / 2;
    private Vector3 _isometricY = new Vector3(1, -0.5f, 0) / 2;
    private Vector3 _isometricZ = new Vector3(0, 0.25f, 1);
    
    
    IEnumerator ResetCanMove()
    {
        yield return new WaitForSeconds(0.2f);
        _canMove = true;
    }
    
    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = transform.position;
        
        Vector2Int intInput = new Vector2Int(
            _onMoveInput.x != 0? (int) Mathf.Sign(_onMoveInput.x): 0,
            _onMoveInput.y != 0? (int) Mathf.Sign(_onMoveInput.y): 0
            );
        
        Vector3Int correctedDirection = _movementValidator.GetValidDirection(_gridPosition, intInput);
        
        _gridPosition += correctedDirection;

        if (!correctedDirection.Equals(Vector3Int.zero))
        {
            if (_onMoveInput.x != 0) // Horizontal Movement
            {
                //_rb.MovePosition(_rb.position + _isometricX * Mathf.Sign(_onMoveInput.x));
                targetPosition += _isometricX * Mathf.Sign(correctedDirection.x);
            } else if (_onMoveInput.y != 0) // Vertical Movement
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
