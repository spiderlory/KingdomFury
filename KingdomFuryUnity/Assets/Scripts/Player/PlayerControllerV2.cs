using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour
{
    
    // Player States Vars
    private enum PlayerState
    {
        Idle,
        Moving,
    }
    
    private PlayerState _currentState;
    private PlayerState _prevState;
    
    
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
        _currentState = PlayerState.Idle;
        _prevState = PlayerState.Idle;
                
        // Get Player Components
        _rb = GetComponent<Rigidbody2D>();
        _movementValidator = GetComponent<MovementValidator>();
        _animator = GetComponent<Animator>();
        
        _gridPosition = new Vector3Int(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _prevState = _currentState;
        
        
        // State Manager Invocation
        switch (_currentState)
        {
            case PlayerState.Idle:
                IdleStateManager();
                break;
            case PlayerState.Moving:
                MovingStateManager();
                break;
        }
    }

    private void IdleStateManager()
    {
        print("Idle");
        if (_onMoveInput != Vector2.zero)
        {
            SetMovingState();
        }
    }
    
    private void MovingStateManager()
    {
        print("Moving");
        
        if (_prevState == PlayerState.Moving)
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
                    _animator.SetBool("IsMoving", false);
                    
                    SetIdleState();
                }
            }
        }
    }

    private void SetMovingState()
    {
        _currentState = PlayerState.Moving;
        _animator.SetBool("IsMoving", true);
            
        _startingPosition = transform.position;
        _targetPosition = GetTargetPosition();
    }
    
    private void SetIdleState()
    {
        _currentState = PlayerState.Idle;
    }
    
    
    
    // New MovementBase
    private Vector3 _isometricX = new Vector3(1, 0.5f, 0) / 2;
    private Vector3 _isometricY = new Vector3(1, -0.5f, 0) / 2;
    private Vector3 _isometricZ = new Vector3(0, 0.25f, 1);
    
    
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
    
    // Input Manager Methods
    private void OnMove(InputValue value)
    {
        _onMoveInput = value.Get<Vector2>();
    
        if (_onMoveInput.x != 0 && _onMoveInput.y != 0)
        {
            _onMoveInput.y = 0;
        }

        if (_onMoveInput.Equals(Vector2.zero))
        {
            _currentState = PlayerState.Idle;
        }
    }
    
    ///
    /// State Machine
    ///     State -> OnStateEnter, OnStateExit, UpdateState, Transition Classe?
    ///     StateMachine -> Sceglie lo stato da chiamare, mantiene lo stato attuale
    ///
    ///     Devo mantenere delle variabili che vengono costantemente aggiornate, ogni stato deve sempre sapere tutto
    ///     Se lo stato è una classe non può accedere facilmente ai campi del personaggio, 
    /// 
}
