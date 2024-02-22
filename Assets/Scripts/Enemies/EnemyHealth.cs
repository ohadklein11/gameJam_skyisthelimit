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

        public bool IsDead => _currentHealth <= 0;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _currentHealth = _maxHealth;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            StartCoroutine(ColorChange());
            if (_currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator ColorChange()
        {
            var originalColor = _spriteRenderer.color;
            _spriteRenderer.DOColor(Color.green, .1f);
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.DOColor(originalColor, .1f);
        }

        private IEnumerator Die()
        {
            _enemyMovement.StopMovement();
            _enemyMovement.Push();
            
            yield return new WaitForSeconds(1f);
            _spriteRenderer.DOFade(0, 1f);
            yield return new WaitForSeconds(1f);
            
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                TakeDamage(1);
            }
        }
    }
}
