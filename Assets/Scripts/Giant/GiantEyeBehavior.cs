using System;
using System.Collections;
using System.Collections.Generic;
using Giant;
using UnityEngine;

public class GiantEyeBehavior : MonoBehaviour
{
    [SerializeField] private GiantFightManager giantFightManager;
    [SerializeField] private Animator animator;
    [SerializeField] private int maxHealth;
    public bool CanGetHit => animator.GetCurrentAnimatorStateInfo(0).IsName("sit") 
                             || animator.GetCurrentAnimatorStateInfo(0).IsName("roar");
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullets") && CanGetHit)
        {
            _currentHealth--;
            if (_currentHealth <= 0)
            {
                giantFightManager.EndGiantFight();
            }
            else
            {
                giantFightManager.HitGiant();
            }
        }
    }
}
