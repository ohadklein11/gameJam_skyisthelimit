using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 3f;
    [SerializeField] private float maxJumpTime = 1f;
    
    private Rigidbody2D _rigidBody;
    
    private float _inputAxis;
    private Vector2 _velocity;
    [SerializeField] private float minVelocity;
    private float _velocityMultiplier = 1f;
    private SpriteRenderer _spriteRenderer;
    private int _groundLayerMask;

    private float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    private float Gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2);
    
    public bool CanMove { get; set; } = true;
    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool Turning => (_inputAxis > 0f && _velocity.x < 0f) || (_inputAxis < 0f && _velocity.x > 0f);
    public bool Running => Mathf.Abs(_velocity.x) > .25f || Mathf.Abs(_inputAxis) > .25f;
    
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _groundLayerMask = LayerMask.GetMask("Ground");
    }
    
    private void Update()
    {
        HorizontalMovement();
        CheckGrounded();
        if (Grounded)
        {
            GroundedMovement();
        }
        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = _rigidBody.position;
        position += _velocity * Time.fixedDeltaTime;
        _rigidBody.MovePosition(position);
    }

    private void HorizontalMovement()
    {
        _inputAxis = CanMove ? Input.GetAxis("Horizontal") : 0f;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * moveSpeed, moveSpeed * Time.deltaTime);
        _velocity.x *= _velocityMultiplier;
        _velocity.x = _inputAxis * Mathf.Max(minVelocity, Mathf.Abs(_velocity.x));
        
        // player's facing direction
        if (_inputAxis > 0f)
        {
            ChangeDirection(_spriteRenderer.flipX,false);
            _spriteRenderer.flipX = false;
        }
        else if (_inputAxis < 0f)
        {
            ChangeDirection(_spriteRenderer.flipX,true);
            _spriteRenderer.flipX = true;
        }
        if (Turning)
        {
            _velocity.x =0;  // i.e. disables turning for now
        }
    }

    private void ChangeDirection(bool currentFlipX, bool newFlipX)
    {
        if (currentFlipX!=newFlipX)
        {
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
        }
    }
    
    private void CheckGrounded()
    {
        Vector3 position = transform.position;
        Vector3 localScale = transform.localScale;
        float distance = 1.5f * localScale.x;
        Bounds spriteBounds = _spriteRenderer.bounds;
        Vector3 playerBottomCenter = position + new Vector3(0, spriteBounds.min.y, 0) * localScale.x;  // fix
        Vector3 playerBottomLeftCorner = playerBottomCenter + new Vector3(-.2f, .15f, 0);
        Vector3 playerBottomRightCorner = playerBottomCenter + new Vector3(.2f, .15f, 0);
        var leftHit = Physics2D.Raycast(playerBottomLeftCorner, Vector2.down, distance, _groundLayerMask);
        var rightHit = Physics2D.Raycast(playerBottomRightCorner, Vector2.down, distance, _groundLayerMask);
        Debug.DrawRay(playerBottomLeftCorner, Vector3.down * distance, Color.red);
        Debug.DrawRay(playerBottomRightCorner, Vector3.down * distance, Color.red);

        var touchesGround = leftHit || rightHit;
        if (!Grounded && touchesGround)
        {
            Debug.Log("Landed");
        }
        Grounded = touchesGround;
    }
    
    private void GroundedMovement()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0f);
        Jumping = _velocity.y > 0f;
        
        if (CanMove && Input.GetButtonDown("Jump"))
        {
            _velocity.y = JumpForce * _velocityMultiplier;
            Debug.Log("Velocity: " + _velocity.y);
            Jumping = true;
        }
    }
    
    private void ApplyGravity()
    {
        bool falling = _velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;
        _velocity.y += multiplier * Gravity * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, Gravity / 2f);
    }
}
