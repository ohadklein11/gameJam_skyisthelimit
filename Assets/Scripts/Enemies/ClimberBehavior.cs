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
        var vine = _player.transform.parent;
        bool playerClimbing = vine != null  // vine
                              && vine.parent != null  // vine holder 
                              && (_isClimbing? _player.transform.parent.parent.parent == transform.parent.parent.parent : 
                                  _player.transform.parent.parent.parent == transform.parent)
                              && _playerMovement.climbing;
        if (_appeared)
        {
            if (playerClimbing)
            {
                if ( Mathf.Abs(vine.transform.position.x - _body.transform.position.x) < .1f)
                { // climb on the vine
                    Debug.Log("Climbing");
                    _isClimbing = true;
                }
                else
                {
                    _isClimbing = false;
                }
            }
            _enemyMovement.FacePlayer();
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
