using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using Player;
using UnityEngine;

public class ClimberBehavior : MonoBehaviour
{
    [SerializeField] private float _appearHeight;
    [SerializeField] private float _stonePushHeight;
    [SerializeField] private float _appearDuration;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _forcedTurnAroundCooldown = 3f;
    private float _forcedTurnAroundCooldownTimer = 0f;
    
    [SerializeField] private Material StoneMaterialTransparent;
    
    private GameObject _player;
    private PlayerMovement _playerMovement;
    
    private GameObject _stoneAboveHead;
    private MeshRenderer _stoneMeshRenderer;
    private GameObject _body;
    private EnemyMovement _enemyMovement;
    private EnemyHealth _enemyHealth;
    private Rigidbody2D _rigidbody2D;

    private bool _appearing;
    private bool _appeared;
    private bool _isClimbing;
    private Transform _curVine;
    private Transform _targetVine;
    private bool _shouldFacePlayer = true;
    private readonly int _groundedMax = 6;
    private int _groundedCount = 0;
    private bool GroundedWell => _groundedCount == _groundedMax;
    
    private Animator _animator;
    private static readonly int AnimAppeared = Animator.StringToHash("Appeared");
    private static readonly int AnimClimbing = Animator.StringToHash("Climbing");
    private static readonly int AnimTop = Animator.StringToHash("Top");
    private static readonly int AnimGrounded = Animator.StringToHash("Grounded");
    [SerializeField] private float _distanceToTargetVine;


    // Start is called before the first frame update
    void Start()
    {
        _appeared = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = _player.GetComponent<PlayerMovement>();

        _body = transform.GetChild(0).gameObject;
        _enemyHealth = _body.GetComponent<EnemyHealth>();
        _enemyHealth.CanTakeDamage = false;
        _enemyMovement = _body.GetComponent<EnemyMovement>();
        _enemyMovement.StopMovement(freeze: true);
        _animator = _body.GetComponent<Animator>();
        _rigidbody2D = _body.GetComponent<Rigidbody2D>();
        _rigidbody2D.simulated = false;
        
        _stoneAboveHead = transform.GetChild(1).gameObject;
        _stoneMeshRenderer = _stoneAboveHead.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHealth == null)
        {
            Destroy(gameObject);
            return;
        }
        
        var vine = _player.transform.parent;
        bool playerClimbing = vine != null  // vine
                              && vine.parent != null  // vine holder 
                              && vine.parent.parent == transform.parent
                              && _playerMovement.climbing;
        if (_appeared)
        {
            if (_shouldFacePlayer)
            {
                _enemyMovement.FacePlayer();
            }

            if (playerClimbing)
            {
                if (Mathf.Abs(vine.position.x - _body.transform.position.x) <= _distanceToTargetVine)
                    _targetVine = vine;
                else
                    _targetVine = null;
                if (_isClimbing)
                {
                    _shouldFacePlayer = true;
                    if (vine != _curVine)
                    {
                        // get down from vine
                        _isClimbing = false;
                        _animator.SetBool(AnimTop, false);
                        _animator.SetBool(AnimClimbing, false);
                        _enemyMovement.StopClimbing();
                    }
                }
            }

            if (!_isClimbing)
            {
                var targetX = _targetVine != null ? _targetVine.position.x : _player.transform.position.x;  
                if (_forcedTurnAroundCooldownTimer > 0)
                {
                    _shouldFacePlayer = false;
                }
                else
                {
                    _shouldFacePlayer = Mathf.Abs(targetX - _body.transform.position.x) > _distanceToTargetVine; // will go towards vine even if player is not climbing
                }
                
                if (_targetVine != null && Mathf.Abs(targetX - _body.transform.position.x) < .3f)
                {
                    _isClimbing = true;
                    _animator.SetBool(AnimClimbing, true);
                    _curVine = _targetVine;
                    var position = _body.transform.position;
                    position = new Vector3(targetX, position.y, position.z);
                    _body.transform.position = position;
                    _enemyMovement.StartClimbing();
                }
            }
            
            if (_forcedTurnAroundCooldownTimer > 0)
            {
                _forcedTurnAroundCooldownTimer -= Time.deltaTime;
            }
            else if (GroundedWell && _enemyMovement.ForcedTurnAround())
            {
                _forcedTurnAroundCooldownTimer = _forcedTurnAroundCooldown;
            }
            if (_enemyMovement.Grounded && _groundedCount < _groundedMax)
            {
                _animator.SetBool(AnimGrounded, true);
                _groundedCount++;
            }
            else if (!_enemyMovement.Grounded)
            {
                _animator.SetBool(AnimGrounded, false);
                _groundedCount = 0;
            }
            
            if ( !_animator.GetBool(AnimTop) && _enemyMovement.ReachedTop)
            {
                _animator.SetBool(AnimTop, true);
            }
        }

        if (!_appeared && !_appearing && playerClimbing)
        {
            StartCoroutine(Appear());
        }
    }

    private IEnumerator Appear()
    {
        _appearing = true;
        _enemyMovement.FacePlayer();
        var position = transform.position;
        transform.DOMoveY(position.y + _appearHeight, _appearDuration).SetEase(Ease.OutSine);
        _stoneAboveHead.GetComponent<MeshRenderer>().material = StoneMaterialTransparent;
        _stoneAboveHead.transform.parent = null;
        _stoneAboveHead.transform.DOMoveY(_stoneAboveHead.transform.position.y + _appearHeight + _stonePushHeight, _appearDuration).SetEase(Ease.OutSine);
        _stoneMeshRenderer.materials[0].DOFade(0, _appearDuration);
        
        yield return new WaitForSeconds(_appearDuration);
        // _body.transform.localScale = new Vector3(1, 1, 1);
        _stoneAboveHead.SetActive(false);
        _enemyHealth.CanTakeDamage = true;
        _animator.SetBool(AnimAppeared, true);
        _enemyMovement.StartMovement(_movementSpeed);
        _rigidbody2D.simulated = true;
        _appearing = false;
        _appeared = true;
        
    }
    
}
