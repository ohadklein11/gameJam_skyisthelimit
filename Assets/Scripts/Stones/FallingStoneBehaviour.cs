using System.Collections;
using System.Collections.Generic;
using Giant;
using UnityEngine;

public class FallingStoneBehaviour : GiantThrowableBehavior
{
    private CameraShake _cameraShake;

    void Awake()
    {
        _cameraShake = Camera.main.GetComponent<CameraShake>();
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(_cameraShake.Shake(0.1f, 0.1f));}
        else
            base.OnCollisionEnter2D(other);
    }
}
