using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFiller : MonoBehaviour
{
    [SerializeField] private int healthValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.HealthRecovery, healthValue);
            Destroy(gameObject);
        }
    }
}
