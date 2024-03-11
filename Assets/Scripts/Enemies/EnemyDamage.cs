using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            try
            {
                if (!GetComponent<EnemyHealth>().IsDead)
                    EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, damage);
            }
            catch
            {
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, damage);
            }
        }
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            try
            {
                if (!GetComponent<EnemyHealth>().IsDead)
                    EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, damage);
            }
            catch
            {
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, damage);
            }
        }
    }
}
