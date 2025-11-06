using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Public Vars
    //   Player Stats
    [SerializeField] private float speed;
    
    
    // Player States
    enum PlayerState
    {
        standing,
        walking
    }
    
    private PlayerState _currentState;
    private bool canMove = true;

    
    // Components
    private Rigidbody2D _rb;
    private MovementValidator _movementValidator;
    
    
    // Player Inputs
    private Vector2 _onMoveInput;
    
    
    // Player grid position
    private Vector3Int _gridPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get Player Components
        _rb = GetComponent<Rigidbody2D>();
        _movementValidator = GetComponent<MovementValidator>();
        
        // Setting Initial Player state
        _currentState =  PlayerState.standing;
        
        _gridPosition = new Vector3Int(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState.Equals(PlayerState.walking) && canMove)
        {
            Move();
            canMove = false;
            StartCoroutine(ResetCanMove());
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnMove(InputValue value)
    {
        _onMoveInput = value.Get<Vector2>();

        if (_onMoveInput.Equals(Vector2.zero))
        {
            _currentState = PlayerState.standing;
        }
        else
        {
            _currentState = PlayerState.walking;
        }
    }

    // New MovementBase
    private Vector3 _isometricX = new Vector3(1, 0.5f, 0) / 2;
    private Vector3 _isometricY = new Vector3(1, -0.5f, 0) / 2;
    private Vector3 _isometricZ = new Vector3(0, 0.25f, 1);
    
    
    IEnumerator ResetCanMove()
    {
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }
    
    private void Move()
    {
        Vector2Int intInput = new Vector2Int(
            _onMoveInput.x != 0? (int) Mathf.Sign(_onMoveInput.x): 0,
            _onMoveInput.y != 0? (int) Mathf.Sign(_onMoveInput.y): 0
            );
        
        Vector3Int correctedDirection = _movementValidator.GetValidDirection(_gridPosition, intInput);
        
        _gridPosition += correctedDirection;

        if (!correctedDirection.Equals(Vector3Int.zero))
        {
            if (correctedDirection.x != 0) // Horizontal Movement
            {
                //_rb.MovePosition(_rb.position + _isometricX * Mathf.Sign(_onMoveInput.x));
                transform.position += _isometricX * Mathf.Sign(correctedDirection.x);
            } else if (correctedDirection.y != 0) // Vertical Movement
            {
                transform.position -= _isometricY * Mathf.Sign(correctedDirection.y);
            }

            if (correctedDirection.z != 0)
            {
                transform.position += _isometricZ * Mathf.Sign(correctedDirection.z);
            }
        }
    }
    
}
