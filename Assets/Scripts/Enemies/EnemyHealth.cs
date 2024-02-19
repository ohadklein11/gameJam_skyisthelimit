using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 3;
        private int _currentHealth;

        private EnemyMovement _enemyMovement;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _enemyMovement = GetComponent<EnemyMovement>();
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            _enemyMovement.StopMovement();
            _enemyMovement.Push();
            
            yield return new WaitForSeconds(2f);
            // fade out animation
            
            
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
