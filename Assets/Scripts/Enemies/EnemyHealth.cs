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
        
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private static readonly int AnimHit = Animator.StringToHash("Hit");
        private static readonly int AnimDead = Animator.StringToHash("Dead");

        public bool CanTakeDamage { get; set; } = true;

        public bool IsDead => _currentHealth <= 0;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _currentHealth = _maxHealth;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
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
            _enemyMovement.StopMovement();
            _animator.SetTrigger(AnimHit);
            yield return new WaitForSeconds(.5f);
            _enemyMovement.ResumeMovement();
        }

        private IEnumerator Die()
        {
            _enemyMovement.StopMovement();
            _animator.SetBool(AnimDead, true);
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
