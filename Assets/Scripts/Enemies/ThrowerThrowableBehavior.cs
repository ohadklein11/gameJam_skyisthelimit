using System;
using System.Collections;
using Player;
using Stones;
using UnityEngine;
using UnityEngine.Pool;
using Vines;

namespace Enemies
{
    public class ThrowerThrowableBehavior : MonoBehaviour, IThrowable
    {
        protected bool _released;
        
        protected ObjectPool<GameObject> _throwablePool;
        private GameObject _player;
        private PlayerMovement _playerMovement;
        private const float Epsilon = 0.01f;
        [SerializeField] private AudioSource audioThrow;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        
        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Throwables"), LayerMask.NameToLayer("Enemy"));
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ObjectPool<GameObject> throwablePool, GameObject player)
        {
            _throwablePool = throwablePool;
            _player = player;
            _playerMovement = player.GetComponent<PlayerMovement>();
            _released = false;
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return;
            }

            if (!_released)
            {
                StartCoroutine(Release());
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Climable"))
            {
                return;
            }
            if (!_released)
            {
                StartCoroutine(Release());
            }
        }

        private IEnumerator Release()
        {
            audioThrow.Play();
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(.5f);
            _throwablePool.Release(gameObject);
            _released = true;
            _spriteRenderer.enabled = true;
            _collider2D.enabled = true;
        }
    }
}
