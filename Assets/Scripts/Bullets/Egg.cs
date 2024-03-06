using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Egg : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        VFXManager.PlayEggPiecesVFX(transform.position);
    }
}
