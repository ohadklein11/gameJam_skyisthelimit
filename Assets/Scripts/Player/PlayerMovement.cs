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
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsVine;
        [SerializeField] private float climbSpeed;
        [SerializeField] private int minNumFramesForClimbing;
        [SerializeField] private Transform loadingBar;

        private float _xInput;
        private float _yInput;
        private float _playerXradius;
        private float _fallingMaxHeight;

        private int _facingDirection = 1;

        private SlopeChecker _slopeChecker;
        private bool _isGrounded;
        private bool _isJumping;
        private bool _canJump;
        private bool _canClimb;
        private bool _isClimbing;
        private bool _isEnteringClimbing;
        private bool _wasFalling;
        private bool _wasGrounded = true;
        private bool _firstClimbWithGrowingVine;
        private int _numFramesSinceEnteringClimbing;
        private IClimbable _climbable;

        private bool IsFalling => (!_isGrounded && !_isClimbing && _rb.velocity.y < 0.0f);

        private bool IsTryingToClimb => (_isGrounded && _yInput > 0.0f) || (!_isGrounded && _yInput != 0.0f);

        private Vector2 _newVelocity;
        private Vector2 _newForce;
        
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
        [SerializeField] private float minFallHeightForDust = .8f;
        private float _fallingFirstHeight;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _slopeChecker = GetComponent<SlopeChecker>();
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
                var position = transform.position;
                _fallingFirstHeight = position.y;
                _fallingMaxHeight = position.y;
            }
            else if (IsFalling && transform.position.y > _fallingMaxHeight)
            {
                _fallingMaxHeight = transform.position.y;
            }

            if (!_wasGrounded && _isGrounded)
            {
                if (_fallingFirstHeight - transform.position.y >= minFallHeightForDust)
                {
                    var position = transform.position;
                    VFXManager.PlayDustVFX(new Vector3(position.x, position.y - _spriteRenderer.bounds.size.y + .1f, position.z));
                }
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

            if (Input.GetButtonDown("Jump") && _yInput > 0 && _isClimbing)
            {
                _yInput = 0;
                // _isClimbing = false;
            }
            else
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
                    var yTopPosition = _climbable.GetHeadYPosition() - _spriteRenderer.bounds.extents.y;
                    if (GetComponent<BeansShooting>().IsShootingBeans())
                        yTopPosition += 1.7f;
                    var yMaxPosition = _climbable.IsGrowing()?Mathf.Infinity:yTopPosition;
                    if (position.y > yTopPosition && _firstClimbWithGrowingVine)
                        yMaxPosition = position.y;
                    else
                        _firstClimbWithGrowingVine = false;

                    position = new Vector3(xPosition, Math.Min(position.y,yMaxPosition), position.z); 
                    transform.position = position;
                    // prevent climbing up when reaching end of vine
                    if (_yInput > 0)
                    {
                        float playerHalfHeight = _spriteRenderer.bounds.size.y / 2;
                        RaycastHit2D vineHitUp = Physics2D.Raycast(
                            new Vector2(transform.position.x, transform.position.y + playerHalfHeight),
                            Vector2.up, .01f, whatIsVine);
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

            if (!_isJumping && (_isClimbing || (_isGrounded && _slopeChecker.GetSlopeDownAngle() <= _slopeChecker.GetMaxSlopeAngle())))
            {
                _canJump = true;
            }
        }

        private void SlopeCheck()
        {
            Bounds spriteBounds = _spriteRenderer.bounds;
            Vector2 checkPos = transform.position - new Vector3(0, (spriteBounds.max.y - spriteBounds.min.y) / 2, 0);
            _slopeChecker.SlopeCheck(checkPos, _facingDirection, _xInput);
        }

        private void CheckClimb()
        {
            RaycastHit2D vineHitLeft = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y),
                Vector2.right * _facingDirection, _playerXradius, whatIsVine);
            RaycastHit2D vineHitRight = Physics2D.Raycast(
                new Vector2(transform.position.x, transform.position.y),
                Vector2.left * _facingDirection, _playerXradius, whatIsVine);
            IClimbable climbableLeft = null, climbableRight = null;
            if ((vineHitLeft.collider != null && vineHitLeft.collider.TryGetComponent(out climbableLeft)) ||
                (vineHitRight.collider != null && vineHitRight.collider.TryGetComponent(out climbableRight)))
            {
                var climbable = climbableLeft ?? climbableRight;
                _canClimb = true;
                if (_climbable != climbable)
                    _firstClimbWithGrowingVine = climbable.IsGrowing();

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
                if (_yInput < 0)
                {
                    _yInput = 0;
                    newJumpForce = -newJumpForce;
                }

                _newForce.Set(0.0f, newJumpForce);
                var velocity = _rb.velocity;
                if (velocity.y > 0)
                { 
                    _rb.velocity = new Vector2(velocity.x, _slopeChecker.IsOnSlope()? Mathf.Min(.3f, velocity.y) : 0);
                }
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
            if (_isGrounded && !_slopeChecker.IsOnSlope() && !_isJumping) //if not on slope
            {
                _newVelocity.Set(movementSpeed * _velocityMultiplier, 0.0f);
                _rb.velocity = _newVelocity;
            }
            else if (_isGrounded && _slopeChecker.IsOnSlope() && _slopeChecker.CanWalkOnSlope() && !_isJumping) //If on slope
            {
                _newVelocity.Set(movementSpeed * _slopeChecker.GetSlopeNormalPerp().x * -_velocityMultiplier,
                    movementSpeed * _slopeChecker.GetSlopeNormalPerp().y * -_velocityMultiplier);
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