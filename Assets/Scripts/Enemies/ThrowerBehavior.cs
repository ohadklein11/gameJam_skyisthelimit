using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using UnityEngine;
using Utils;

public class ThrowerBehavior : MonoBehaviour, IThrower
{
    [SerializeField] private float viewDistance;
    
    private EnemyMovement _enemyMovement;
    private EnemyHealth _enemyHealth;
    private bool _throwingAtPlayer = false;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private GameObject throwable;
    [SerializeField] private float minThrowTime = 3f;
    [SerializeField] private float maxThrowTime = 3f;
    [SerializeField] private float minThrowAngle = 120f;
    [SerializeField] private float maxThrowAngle = 120f;
    [SerializeField] private Transform throwPosition;
    private ThrowAtPlayerBehavior _throwAtPlayerBehavior;

    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _throwAtPlayerBehavior = GetComponent<ThrowAtPlayerBehavior>();
        _throwAtPlayerBehavior.Init(throwable, minThrowTime, maxThrowTime, minThrowAngle, maxThrowAngle, throwPosition, this);
        _throwAtPlayerBehavior.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_throwingAtPlayer) return;
        var playerTransform = LookAtPlayer();
        if (playerTransform != null)
        {
            _playerTransform = playerTransform;
            StartCoroutine(ThrowAtPlayer());
        }
    }

    private Transform LookAtPlayer()
    {
        var direction = _enemyMovement.GetDirection();
        var enemyBounds = _spriteRenderer.bounds;
        var position = transform.position;
        Vector2 enemyTop = new Vector2(position.x, position.y + enemyBounds.size.y / 2);
        Vector2 enemyCenter = new Vector2(position.x, position.y);
        Vector2 enemyBottom = new Vector2(position.x, position.y - enemyBounds.size.y / 2);
        RaycastHit2D playerHitTop = Physics2D.Raycast(enemyTop, Vector2.right * direction, viewDistance, LayerMask.GetMask("Player"));
        RaycastHit2D playerHitCenter = Physics2D.Raycast(enemyCenter, Vector2.right * direction, viewDistance, LayerMask.GetMask("Player"));
        RaycastHit2D playerHitBottom = Physics2D.Raycast(enemyBottom, Vector2.right * direction, viewDistance, LayerMask.GetMask("Player"));
        if (playerHitTop.collider != null)
        {
            return playerHitTop.transform;
        }
        if (playerHitCenter.collider != null)
        {
            return playerHitCenter.transform;
        }
        if (playerHitBottom.collider != null)
        {
            return playerHitBottom.transform;
        }
        return null;
    }

    private IEnumerator ThrowAtPlayer()
    {
        if (_enemyHealth.IsDead) yield break;
        _throwingAtPlayer = true;
        _throwAtPlayerBehavior.enabled = true;
        _enemyMovement.StopMovement(freeze: true);
        bool seeingPlayer;
        do
        {
            if (_enemyHealth.IsDead)
            {
                _throwingAtPlayer = false;
                _throwAtPlayerBehavior.enabled = false;
                yield break;
            } else
            {
                var playerTransform = LookAtPlayer();
                seeingPlayer = playerTransform != null;
            }
            yield return null;
        } while (seeingPlayer);
        _throwingAtPlayer = false;
        _throwAtPlayerBehavior.enabled = false;
        _enemyMovement.StartMovement();
    }

    public int GetDirection()
    {
        return _enemyMovement.GetDirection();
    }
}
