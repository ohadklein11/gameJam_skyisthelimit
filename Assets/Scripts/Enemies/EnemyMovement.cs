using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private float _speed = 2f;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    private bool _canMove = true;
    private float _originalSpeed;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private int _direction = 1;
    private Vector3 _positionBefore1Frame;
    private Vector3 _positionBefore2Frames;
    
    private Transform _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _positionBefore1Frame = transform.position;
        _positionBefore2Frames = transform.position;
        _originalSpeed = _speed;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!_canMove) return;
        if (Mathf.Abs(transform.position.x - _positionBefore2Frames.x) < 0.01f)
        {
            TurnAround();
        }

        _positionBefore2Frames = _positionBefore1Frame;
        _positionBefore1Frame = transform.position;
        // raycast to check if there is ground in front of the enemy
        var bounds = _spriteRenderer.bounds;
        Vector3 raycastOrigin =
            transform.position + new Vector3(_direction * bounds.size.x / 2, -bounds.size.y / 2 - .1f, 0);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector3.down, .6f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(raycastOrigin, Vector3.down * .6f, Color.red);
        if (hit.collider == null)
        {
            TurnAround();
        }

        _rb.velocity = new Vector2(_speed * _direction, _rb.velocity.y);
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
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
    }

    public int GetDirection()
    {
        return _direction;
    }
    
    public void FacePlayer()
    {
        int direction;
        if (_player.position.x < transform.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        if (direction != _direction)
        {
            TurnAround();
        }
    }
}
