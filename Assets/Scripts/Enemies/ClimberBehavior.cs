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
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;

    private bool _appearing;
    private bool _appeared;
    private bool _isClimbing;
    private Transform _curVine;
    private Transform _targetVine;
    private bool _shouldFacePlayer = true;

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
        _spriteRenderer = _body.GetComponent<SpriteRenderer>();
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
                _targetVine = vine;
                if (_isClimbing)
                {
                    _shouldFacePlayer = true;
                    if (vine != _curVine)
                    {
                        // get down from vine
                        _isClimbing = false;
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
                    _shouldFacePlayer = Mathf.Abs(targetX - _body.transform.position.x) > 3f; // will go towards vine even if player is not climbingZ
                }
                
                if (Mathf.Abs(targetX - _body.transform.position.x) < .3f)
                {
                    _isClimbing = true;
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
            else if (_enemyMovement.ForcedTurnAround())
            {
                _forcedTurnAroundCooldownTimer = _forcedTurnAroundCooldown;
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
        transform.DOMoveY(transform.position.y + _appearHeight, _appearDuration).SetEase(Ease.OutSine);
        _stoneAboveHead.GetComponent<MeshRenderer>().material = StoneMaterialTransparent;
        _stoneAboveHead.transform.parent = null;
        _stoneAboveHead.transform.DOMoveY(_stoneAboveHead.transform.position.y + _appearHeight + _stonePushHeight, _appearDuration).SetEase(Ease.OutSine);
        _stoneMeshRenderer.materials[0].DOFade(0, _appearDuration);
        
        yield return new WaitForSeconds(_appearDuration);
        // _body.transform.localScale = new Vector3(1, 1, 1);
        _stoneAboveHead.SetActive(false);
        _enemyHealth.CanTakeDamage = true;
        _enemyMovement.StartMovement(_movementSpeed);
        _rigidbody2D.simulated = true;
        _appearing = false;
        _appeared = true;
        
    }
    
}
