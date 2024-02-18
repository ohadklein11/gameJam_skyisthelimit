using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Giant;
using UnityEngine;

public class FallingStoneBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeMagnitude = 2f;
    void Awake()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("FallingStone");
            _virtualCamera.GetComponent<CameraShake>().Shake(shakeMagnitude, shakeDuration);
        }
        else
        {
            Debug.Log("GiantThrowable");
        }
    }
}
