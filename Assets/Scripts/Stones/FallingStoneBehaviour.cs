using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Giant;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

public class FallingStoneBehaviour : MonoBehaviour
{
    private ObjectPool<GameObject> _throwablePool;
    private Camera _mainCamera;

    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeMagnitude = 2f;
    [SerializeField] private float magnitudeStopDamage = 5f;
    [SerializeField] private int stoneDamage;


    private bool _released;
    private CameraShake _cameraShake;
    [SerializeField] private AudioSource audioSmash;
    private MeshRenderer _meshRenderer;
    private Collider2D _collider2D;
    private bool _startedDestroy;

    void Awake()
    {
        _mainCamera = Camera.main;
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        _cameraShake = _virtualCamera.GetComponent<CameraShake>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    public void Init(ObjectPool<GameObject> throwablePool)
    {
        _throwablePool = throwablePool;
        _released = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        VFXManager.PlayDustVFX(transform.position);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _startedDestroy = true;
            StartCoroutine(SelfDestroyTimer());
            _cameraShake.Shake(shakeMagnitude, shakeDuration);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
                 gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > magnitudeStopDamage)
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, stoneDamage);

        }

        if (!_released && !_startedDestroy)
        {
            _startedDestroy = true;
            StartCoroutine(Release());
        }
    }

    IEnumerator SelfDestroyTimer()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<MeshRenderer>().material.DOFade(0, 1f);

        yield return new WaitForSeconds(1f);

        VFXManager.PlayStonePiecesVFX(transform.position);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        bool destory = false;
        // if camera is 1.5 screens on the left of the Stone, destroy the Stone
        if (transform.position.x <
            _mainCamera.transform.position.x - _mainCamera.orthographicSize * _mainCamera.aspect * 1.5f)
        {
            destory = true;
        }

        // if camera is 1.5 screens on the right of the Stone, destroy the Stone
        if (transform.position.x >
            _mainCamera.transform.position.x + _mainCamera.orthographicSize * _mainCamera.aspect * 1.5f)
        {
            destory = true;
        }

        // if camera is 1.5 screens above the Stone, destroy the Stone
        if (transform.position.y > _mainCamera.transform.position.y + _mainCamera.orthographicSize * 1.5f)
        {
            destory = true;
        }

        // if camera is 1.5 screens underneath the Stone, destroy the Stone
        if (transform.position.y < _mainCamera.transform.position.y - _mainCamera.orthographicSize * 1.5f)
        {
            destory = true;
        }

        if (!_released && destory && !_startedDestroy)
        {
            _startedDestroy = true;
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
        if (_throwablePool != null && gameObject != null)
            _throwablePool.Release(gameObject);
        _released = true;
        _meshRenderer.enabled = true;
        _collider2D.enabled = true;
    }
}