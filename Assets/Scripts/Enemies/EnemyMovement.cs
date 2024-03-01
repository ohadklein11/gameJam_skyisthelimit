using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using Utils;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _speed = 2f;

    private bool _canMove = true;
    private float _originalSpeed;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private SlopeChecker _slopeChecker;
    private int _direction = 1;
    private Vector3 _positionBefore1Frame;
    private Vector3 _positionBefore2Frames;
    
    private bool _isVertical = false;
    private float _originalGravityScale;
    
    private Transform _player;
    
    public bool Grounded { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravityScale = _rb.gravityScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _positionBefore1Frame = transform.position;
        _positionBefore2Frames = transform.position;
        _originalSpeed = _speed;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _slopeChecker = GetComponent<SlopeChecker>();
    }

    private bool CheckUnwalkableSlopeInFront()
    {
        var bounds = _spriteRenderer.bounds;
        var checkPos = transform.position + new Vector3(_direction * (bounds.max.x - bounds.min.x) / 2, - (bounds.max.y - bounds.min.y) / 4);
        return _slopeChecker.CheckUnwalkableSlopeInFront(checkPos, _direction);
    }

    private void Update()
    {
        if (!_canMove) return;
        if (NeedsToTurnAround())
        {
            TurnAround();
        }

        _positionBefore2Frames = _positionBefore1Frame;
        _positionBefore1Frame = transform.position;
        if (_isVertical)
        {
            MoveVertical();
        }
        else if (Grounded)
        {
            MoveHorizontal();
        }
        
        // also check if the enemy is on the ground
        var bounds = _spriteRenderer.bounds;
        var raycastOrigin = transform.position + new Vector3(0, -bounds.size.y / 2 - .1f, 0);
        var hit = Physics2D.Raycast(raycastOrigin, Vector3.down, .4f, LayerMask.GetMask("Ground"));
        Grounded = hit.collider != null;
    }

    private void MoveVertical()
    {
        var aboveHead = new Vector2(transform.position.x, transform.position.y + _spriteRenderer.bounds.size.y / 2 + .1f);
        RaycastHit2D hit = Physics2D.Raycast(aboveHead, Vector2.up, .1f, LayerMask.GetMask("Climable"));
        if (hit.collider != null)
        {
            _rb.velocity = new Vector2(0, _speed/2); 
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void MoveHorizontal()
    {
        _rb.velocity = new Vector2(_speed * _direction, _rb.velocity.y);
    }

    private bool NeedsToTurnAround()
    {
        if (_isVertical || !_canMove || !Grounded)
            return false;
        if (Mathf.Abs(transform.position.x - _positionBefore2Frames.x) < 0.01f)  // can't move
        {
            return true;
        }
        // check if not walking on slope
        if (CheckUnwalkableSlopeInFront())
        {
            return true;
        }
        
        // check if can move forward
        var bounds = _spriteRenderer.bounds;
        var raycastOrigin =
            transform.position + new Vector3(_direction * bounds.size.x / 2, -bounds.size.y / 2 - .1f, 0);
        var hit = Physics2D.Raycast(raycastOrigin, Vector3.down, .6f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(raycastOrigin, Vector3.down * .6f, Color.red);
        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }


    private void TurnAround()
    {
        _direction *= -1;
        var transform1 = transform;
        var localScale = transform1.localScale;
        localScale = new Vector3(_direction * Mathf.Abs(localScale.x), localScale.y, localScale.z);
        transform1.localScale = localScale;
    }

    public void StopMovement(bool freeze = false)
    {
        _canMove = false;
        _rb.velocity = Vector2.zero;
        if (freeze)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        } else
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
    
    public void StartMovement(float speed)
    {
        _speed = speed;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _canMove = true;
    }
    
    public void StartMovement()
    {
        StartMovement(_originalSpeed);
    }

    public void Push()
    {
        _rb.gravityScale = 1;
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
    }

    public int GetDirection()
    {
        return _direction;
    }
    
    public void FacePlayer()
    {
        int facingDirection;
        if (_player.position.x < transform.position.x)
        {
            facingDirection = -1;
        }
        else
        {
            facingDirection = 1;
        }
        if (facingDirection != _direction)
        {
            TurnAround();
        }
    }
    
    public void StartClimbing()
    {
        _isVertical = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        _rb.gravityScale = 0;
    }
    
    public void StopClimbing()
    {
        _isVertical = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = _originalGravityScale;
        _rb.velocity = Vector2.zero;
    }

    
}
