using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Giant;
using UnityEngine;
using UnityEngine.Pool;

public class FallingStoneBehaviour : MonoBehaviour
{
    protected ObjectPool<GameObject> _throwablePool;

    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeMagnitude = 2f;
    
    private bool _released;
    void Awake()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    
    public void Init(ObjectPool<GameObject> throwablePool)
    {
        _throwablePool = throwablePool;
        _released = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Stone hit the ground");
            _virtualCamera.GetComponent<CameraShake>().Shake(shakeMagnitude, shakeDuration);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Player")&&other.gameObject.GetComponent<Rigidbody2D>().velocity.x<0)
        {
            if (!_released)
            {
                _throwablePool.Release(gameObject);
                _released = true;
            }
            // TODO: break the stone
            
        }
    }
}
