﻿using System;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Vines;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float minFallDamage;
        [SerializeField] private float movementSpeed;

        [SerializeField]
        private int
            numFramesUntilMaxSlowdown; // e.g. after 20 frames of moving, the player will slide to a stop upon releasing the movement key

        [SerializeField] private float groundCheckRadius;
        [SerializeField] private float jumpForce;
        [SerializeField] private float slopeCheckDistance;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsVine;
        [SerializeField] private PhysicsMaterial2D noFriction;
        [SerializeField] private PhysicsMaterial2D fullFriction;
        [SerializeField] private float climbSpeed;
        [SerializeField] private int minNumFramesForClimbing;
        [SerializeField] private Transform loadingBar;

        private float _xInput;
        private float _yInput;
        private float _slopeDownAngle;
        private float _slopeSideAngle;
        private float _lastSlopeAngle;
        private float _playerXradius;
        private float _fallingMaxHeight;

        private int _facingDirection = 1;

        private bool _isGrounded;
        private bool _isOnSlope;
        private bool _isJumping;
        private bool _canWalkOnSlope;
        private bool _canJump;
        private bool _canClimb;
        private bool _isClimbing;
        private bool _isEnteringClimbing;
        private bool _wasFalling;
        private bool _wasGrounded;
        private int _numFramesSinceEnteringClimbing;
        private IClimbable _climbable;

        private bool IsFalling => (!_isGrounded && !_isClimbing && _rb.velocity.y < 0.0f);

        private bool IsTryingToClimb => (_isGrounded && _yInput > 0.0f) || (!_isGrounded && _yInput != 0.0f);

        private Vector2 _newVelocity;
        private Vector2 _newForce;

        private Vector2 _slopeNormalPerp;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private float _velocityMultiplier = 1f;
        private float _timeMultiplier;
        [SerializeField] private float highJumpGravity;
        [SerializeField] private float jumpGravity;
        [SerializeField] private float fallGravity;
        
        public bool climbing => _isClimbing;
        public bool grounded => _isGrounded;

        public bool jumping => _isJumping;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerXradius = _spriteRenderer.bounds.extents.x;
            _timeMultiplier = 0;
        }

        private void Update()
        {
            if (GameData.isGameStopped) return;
            CheckInput();
        }

        private void FixedUpdate()
        {
            CheckGround();
            SlopeCheck();
            CheckClimb();
            ApplyMovement();
            CheckFalling();
        }

        private void CheckFalling()
        {
            if (!_wasFalling && IsFalling)
            {
                _fallingMaxHeight = transform.position.y;
            }
            else if (IsFalling && transform.position.y > _fallingMaxHeight)
            {
                _fallingMaxHeight = transform.position.y;
            }

            if (!_wasGrounded && _isGrounded)
            {
                if (_wasFalling)
                {
                    TakeFallDamage();
                }
            }

            _wasFalling = IsFalling;
            _wasGrounded = _isGrounded;
        }

        private void TakeFallDamage()
        {
            int fallDamage = (int)(_fallingMaxHeight - transform.position.y);
            if (fallDamage > minFallDamage)
            {
                Debug.Log("Player took alot of fall damage");
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, fallDamage);
            }
        }

        private void CheckInput()
        {
            _xInput = Input.GetAxisRaw("Horizontal");
            _yInput = Input.GetAxisRaw("Vertical");

            if (Math.Abs(_xInput - 1) < .01f && _facingDirection == -1)
            {
                Flip();
            }
            else if (Math.Abs(_xInput - (-1)) < .01f && _facingDirection == 1)
            {
                Flip();
            }

            HandleClimbing();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        private void HandleClimbing()
        {
            if (_canClimb && (IsTryingToClimb || (_isClimbing && !_isGrounded && !_isJumping)))
            {
                // climb
                if (!_isClimbing)
                {
                    _numFramesSinceEnteringClimbing = minNumFramesForClimbing;
                }

                _isClimbing = true;
                var position = transform.position;
                try
                {
                    var xPosition = _climbable.GetXPosition();
                    position = new Vector3(xPosition, position.y, position.z);
                    transform.position = position;
                    // prevent climbing up when reaching end of vine
                    if (_yInput > 0 && !Input.GetButton("Jump"))
                    {
                        float playerHalfHeight = _spriteRenderer.bounds.size.y / 2;
                        RaycastHit2D vineHitUp = Physics2D.Raycast(
                            new Vector2(transform.position.x, transform.position.y + playerHalfHeight + 0.1f),
                            Vector2.up, .1f, whatIsVine);
                        if (!vineHitUp)
                        {
                            _yInput = 0;
                        }
                    }

                    _rb.velocity = new Vector2(0, _yInput * climbSpeed);
                }
                catch (MissingReferenceException e)
                {
                    _isClimbing = false;
                }
            }
            else
            {
                _isClimbing = false;
            }

            if (_isClimbing && transform.parent != _climbable.GetParentTransform())
            {
                transform.SetParent(_climbable.GetParentTransform());
            }
            else if (!_isClimbing && transform.parent != null)
            {
                transform.SetParent(null);
            }

            _isEnteringClimbing = _numFramesSinceEnteringClimbing > 0;
            _numFramesSinceEnteringClimbing = Mathf.Max(0, _numFramesSinceEnteringClimbing - 1);
        }

        private void CheckGround()
        {
            _isGrounded = !_isEnteringClimbing &&
                          Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            if (_rb.velocity.y <= 0.0f)
            {
                _isJumping = false;
            }

            if (!_isJumping && (_isClimbing || (_isGrounded && _slopeDownAngle <= maxSlopeAngle)))
            {
                _canJump = true;
            }
        }

        private void SlopeCheck()
        {
            Bounds spriteBounds = _spriteRenderer.bounds;
            Vector2 checkPos = transform.position - new Vector3(0, (spriteBounds.max.y - spriteBounds.min.y) / 2, 0);
            // Debug.Log((spriteBounds.max.y - spriteBounds.min.y) / 2);

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

                if (_slopeDownAngle != _lastSlopeAngle)
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

        private void CheckClimb()
        {
            // RaycastHit2D vineHit = Physics2D.Raycast(
            //     new Vector2(transform.position.x - _playerXradius * _facingDirection, transform.position.y), 
            //     Vector2.right * _facingDirection, 2*_playerXradius, whatIsVine);
            RaycastHit2D vineHitLeft = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y),
                Vector2.right * _facingDirection, _playerXradius, whatIsVine);
            RaycastHit2D vineHitRight = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y),
                Vector2.left * _facingDirection, _playerXradius, whatIsVine);
            IClimbable climbable1 = null, climbable2 = null;
            if ((vineHitLeft.collider != null && vineHitLeft.collider.TryGetComponent(out climbable1)) ||
                (vineHitRight.collider != null && vineHitRight.collider.TryGetComponent(out climbable2)))
            {
                var climbable = climbable1 ?? climbable2;
                _canClimb = true;
                _climbable = climbable;
            }
            else
            {
                _canClimb = false;
                _climbable = null;
            }
        }

        private void Jump()
        {
            if (_canJump)
            {
                _canJump = false;
                _isJumping = true;
                var newJumpForce = _isGrounded ? jumpForce - _newVelocity.y * .8f : jumpForce;
                _newForce.Set(0.0f, newJumpForce);
                _rb.AddForce(_newForce, ForceMode2D.Impulse);
            }
        }

        private void ApplyMovement()
        {
            if (_xInput != 0)
            {
                _velocityMultiplier = _xInput;
                _timeMultiplier = Mathf.Min(1f, _timeMultiplier + 1f / numFramesUntilMaxSlowdown);
            }
            else
            {
                var reduce = _isGrounded ? .1f : .05f;
                _velocityMultiplier -= reduce * Mathf.Sign(_velocityMultiplier);
                if (Mathf.Abs(_velocityMultiplier) <= reduce)
                {
                    _velocityMultiplier = 0.0f;
                    _timeMultiplier = 0.0f;
                }
            }

            _velocityMultiplier *= _timeMultiplier;
            if (_isGrounded && !_isOnSlope && !_isJumping) //if not on slope
            {
                _newVelocity.Set(movementSpeed * _velocityMultiplier, 0.0f);
                _rb.velocity = _newVelocity;
            }
            else if (_isGrounded && _isOnSlope && _canWalkOnSlope && !_isJumping) //If on slope
            {
                _newVelocity.Set(movementSpeed * _slopeNormalPerp.x * -_velocityMultiplier,
                    movementSpeed * _slopeNormalPerp.y * -_velocityMultiplier);
                _rb.velocity = _newVelocity;
            }
            else if (!_isGrounded && !_isClimbing) //If in air
            {
                _newVelocity.Set(movementSpeed * _velocityMultiplier, _rb.velocity.y);
                _rb.velocity = _newVelocity;
            }

            // gravity handling
            var gravity = _isClimbing ? 0f :
                _isJumping ? Input.GetButton("Jump") ? highJumpGravity : jumpGravity : fallGravity;
            _rb.gravityScale = gravity;
        }

        private void Flip()
        {
            _facingDirection *= -1;
            var transform1 = transform;
            Vector3 flipScale = transform1.localScale;
            flipScale.x *= -1;
            transform1.localScale = flipScale;
            var localScale = loadingBar.localScale;
            localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            loadingBar.localScale = localScale;
        }

        public void StopClimbing()
        {
            _canClimb = false;
            _isClimbing = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}