using System;
using System.Collections;
using Cinemachine;
using Player;
using Stones;
using UnityEngine;
using UnityEngine.Pool;
using Utils;
using Vines;

namespace Giant
{
    public class GiantThrowableBehavior : MonoBehaviour, IThrowable
    {
        protected bool _released;
        
        protected ObjectPool<GameObject> _throwablePool;
        private GameObject _player;
        private PlayerMovement _playerMovement;
        private const float Epsilon = 0.01f;
        [SerializeField] private int stoneDamage;
        private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float shakeDuration = 1f;
        [SerializeField] private float shakeMagnitude = 2f;

        [SerializeField] private AudioSource audioSmash;
        private MeshRenderer _meshRenderer;
        private Collider2D _collider2D;
        private CameraShake _cameraShake;

        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Throwables"), LayerMask.NameToLayer("Enemy"));
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider2D = GetComponent<Collider2D>();
            _cameraShake = _virtualCamera.GetComponent<CameraShake>();
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
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, stoneDamage);
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
                if (Math.Abs(_player.transform.position.x - other.GetComponent<IClimbable>().GetXPosition()) < Epsilon)
                {
                    _playerMovement.StopClimbing();
                }
                if (other.transform.parent != null)
                {
                    if (!other.transform.parent.GetComponent<GrowVineScript>().destroyed)
                    {
                        StartCoroutine(other.transform.parent.GetComponent<GrowVineScript>().DestroyVine());
                    }
                }
                else
                {
                    if (!other.GetComponent<GrowVineScript>().destroyed)
                    {
                        StartCoroutine(other.GetComponent<GrowVineScript>().DestroyVine());
                    }
                }
            }
            if (!_released)
            {
                StartCoroutine(Release());
            }
        }
        private IEnumerator Release()
        {
            VFXManager.PlayStonePiecesVFX(transform.position);
            _cameraShake.Shake(shakeMagnitude, shakeDuration);
            audioSmash.Play();
            _meshRenderer.enabled = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(1f);
            _throwablePool.Release(gameObject);
            _released = true;
            _meshRenderer.enabled = true;
            _collider2D.enabled = true;
        }
    }
}
