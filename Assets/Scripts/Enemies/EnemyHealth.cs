using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 3;
        private int _currentHealth;

        private EnemyMovement _enemyMovement;
        private Collider2D _collider2D;
        
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private static readonly int AnimHit = Animator.StringToHash("Hit");
        private static readonly int AnimDead = Animator.StringToHash("Dead");
        private Rigidbody2D _rigidBody;

        public bool CanTakeDamage { get; set; } = true;
        public bool Hit { get; private set; }

        public bool IsDead => _currentHealth <= 0;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _currentHealth = _maxHealth;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _collider2D = GetComponent<Collider2D>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(TakeDamageCoroutine());
            }
        }

        private IEnumerator TakeDamageCoroutine()
        {
            Hit = true;
            _animator.SetBool(AnimHit, true);
            _enemyMovement.StopMovement();
            yield return new WaitForSeconds(.5f);
            _animator.SetBool(AnimHit, false);
            if (_collider2D.enabled)
                _enemyMovement.ResumeMovement();
            Hit = false;
        }


        private IEnumerator Die()
        {
            _animator.SetBool(AnimDead, true);
            _enemyMovement.enabled = false;
            _rigidBody.simulated = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(1f);
            _spriteRenderer.DOFade(0, 1f);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (CanTakeDamage && other.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                TakeDamage(1);
            }
        }
    }
}
