using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enemies;
using UnityEngine;

public class RunnerBehavior : MonoBehaviour
{
    [SerializeField] private float viewDistance;
    [SerializeField] private float chaseSpeed;
    
    private EnemyMovement _enemyMovement;
    private EnemyHealth _enemyHealth;
    private bool _chasingPlayer = false;
    private Transform _playerTransform;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (_chasingPlayer) return;
        var direction = _enemyMovement.GetDirection();
        var enemyBounds = GetComponent<SpriteRenderer>().bounds;
        var position = transform.position;
        Vector2 enemyCenter = new Vector2(position.x, position.y);
        Vector2 enemyBottom = new Vector2(position.x, position.y - enemyBounds.size.y / 2);
        RaycastHit2D playerHitTop = Physics2D.Raycast(enemyCenter, Vector2.right * direction, viewDistance, LayerMask.GetMask("Player"));
        RaycastHit2D playerHitBottom = Physics2D.Raycast(enemyBottom, Vector2.right * direction, viewDistance, LayerMask.GetMask("Player"));
        Debug.DrawRay(enemyCenter, Vector2.right * direction * viewDistance, Color.green);
        Debug.DrawRay(enemyBottom, Vector2.right * direction * viewDistance, Color.green);
        if (playerHitTop.collider != null)
        {
            _playerTransform = playerHitTop.collider.transform;
            StartCoroutine(ChasePlayer());
        } else if (playerHitBottom.collider != null)
        {
            _playerTransform = playerHitBottom.collider.transform;
            StartCoroutine(ChasePlayer());
        }
    }

    private IEnumerator ChasePlayer()
    {
        _chasingPlayer = true;
        _enemyMovement.StopMovement(freeze: true);
        _spriteRenderer.DOColor(new Color(215f/255f, 49f/255f, 38f/255f), 1f);
        yield return new WaitForSeconds(1f);
        _enemyMovement.StartMovement(chaseSpeed);
        // tween to change color to red
        
        bool seeingPlayer;
        do
        {
            if (_enemyHealth.IsDead) yield break;
            if (_enemyMovement.GetDirection() > 0)
            {
                seeingPlayer = _playerTransform.position.x - transform.position.x > 0;
            }
            else
            {
                seeingPlayer = transform.position.x - _playerTransform.position.x > 0;
            }
            yield return null;
        } while (seeingPlayer);
        _chasingPlayer = false;
        _spriteRenderer.DOColor(_originalColor, 1f);
        _enemyMovement.StartMovement();
    }
}
