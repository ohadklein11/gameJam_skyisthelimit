using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float groundCheckRadius;
        [SerializeField]
        private float jumpForce;
        [SerializeField]
        private float slopeCheckDistance;
        [SerializeField]
        private float maxSlopeAngle;
        [SerializeField]
        private Transform groundCheck;
        [SerializeField]
        private LayerMask whatIsGround;
        [SerializeField]
        private PhysicsMaterial2D noFriction;
        [SerializeField]
        private PhysicsMaterial2D fullFriction;

        private float _xInput;
        private float _slopeDownAngle;
        private float _slopeSideAngle;
        private float _lastSlopeAngle;

        private int _facingDirection = 1;

        private bool _isGrounded;
        private bool _isOnSlope;
        private bool _isJumping;
        private bool _canWalkOnSlope;
        private bool _canJump;

        private Vector2 _newVelocity;
        private Vector2 _newForce;

        private Vector2 _slopeNormalPerp;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private float _velocityMultiplier = 1f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        private void Update()
        {
            CheckInput();     
        }

        private void FixedUpdate()
        {
            CheckGround();
            SlopeCheck();
            ApplyMovement();
        }

        private void CheckInput()
        {
            _xInput = Input.GetAxisRaw("Horizontal");

            if (Math.Abs(_xInput - 1) < .01f && _facingDirection == -1)
            {
                Flip();
            }
            else if (Math.Abs(_xInput - (-1)) < .01f && _facingDirection == 1)
            {
                Flip();
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

        }
        private void CheckGround()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            if(_rb.velocity.y <= 0.0f)
            {
                _isJumping = false;
            }

            if(_isGrounded && !_isJumping && _slopeDownAngle <= maxSlopeAngle)
            {
                _canJump = true;
            }

        }

        private void SlopeCheck()
        {
            Bounds spriteBounds = _spriteRenderer.bounds;
            Vector2 checkPos = transform.position - new Vector3(0, (spriteBounds.max.y - spriteBounds.min.y) / 2, 0);

            SlopeCheckHorizontal(checkPos);
            SlopeCheckVertical(checkPos);
        }

        private void SlopeCheckHorizontal(Vector2 checkPos)
        {
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);
        
            if (slopeHitFront)
            {
                _isOnSlope = true;

                _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

            }
            else if (slopeHitBack)
            {
                _isOnSlope = true;

                _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            }
            else
            {
                _slopeSideAngle = 0.0f;
                _isOnSlope = false;
            }

        }

        private void SlopeCheckVertical(Vector2 checkPos)
        {      
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);
            Debug.DrawRay(checkPos, Vector2.down, Color.red);

            if (hit)
            {

                _slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

                _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if(_slopeDownAngle != _lastSlopeAngle)
                {
                    _isOnSlope = true;
                }                       

                _lastSlopeAngle = _slopeDownAngle;
           
                Debug.DrawRay(hit.point, _slopeNormalPerp, Color.blue);
                Debug.DrawRay(hit.point, hit.normal, Color.green);

            }

            if (_slopeDownAngle > maxSlopeAngle || _slopeSideAngle > maxSlopeAngle)
            {
                _canWalkOnSlope = false;
            }
            else
            {
                _canWalkOnSlope = true;
            }

            if (_isOnSlope && _canWalkOnSlope && _xInput == 0.0f)
            {
                _rb.sharedMaterial = fullFriction;
            }
            else
            {
                _rb.sharedMaterial = noFriction;
            }
        }

        private void Jump()
        {
            if (_canJump)
            {
                _canJump = false;
                _isJumping = true;
                _newForce.Set(0.0f, jumpForce - _newVelocity.y*.8f);
                _rb.AddForce(_newForce, ForceMode2D.Impulse);
            }
        }   

        private void ApplyMovement()
        {
            if (_xInput != 0)
            {
                _velocityMultiplier = _xInput;
            }
            else
            {
                var reduce = _isGrounded ? .1f : .05f;
                _velocityMultiplier -= reduce * Mathf.Sign(_velocityMultiplier);
                if (Mathf.Abs(_velocityMultiplier) <= reduce)
                {
                    _velocityMultiplier = 0.0f;
                }
            }
            if (_isGrounded && !_isOnSlope && !_isJumping) //if not on slope
            {
                _newVelocity.Set(movementSpeed * _velocityMultiplier, 0.0f);
                _rb.velocity = _newVelocity;
            }
            else if (_isGrounded && _isOnSlope && _canWalkOnSlope && !_isJumping) //If on slope
            {
                _newVelocity.Set(movementSpeed * _slopeNormalPerp.x * -_velocityMultiplier, movementSpeed * _slopeNormalPerp.y * -_velocityMultiplier);
                _rb.velocity = _newVelocity;
            }
            else if (!_isGrounded) //If in air
            {
                _newVelocity.Set(movementSpeed * _velocityMultiplier, _rb.velocity.y);
                _rb.velocity = _newVelocity;
            }
        
            // gravity
            var gravity = _isJumping? 1.0f : 2.0f;
            gravity = Input.GetButton("Jump") ? .5f * gravity : gravity;
            _rb.gravityScale = gravity;

        }

        private void Flip()
        {
            _facingDirection *= -1;
            Vector3 flipScale = transform.localScale;
            flipScale.x *= -1;
            transform.localScale = flipScale;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

    }
}

