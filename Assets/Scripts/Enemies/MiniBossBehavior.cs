using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class MiniBossBehavior: MonoBehaviour
    {
        [SerializeField] private AudioSource gruntAudio;
        [SerializeField] private AudioSource hitAudio;
        [SerializeField] private AudioSource deadAudio;
        private EnemyHealth _enemyHealth;
        private bool _hit;
        private bool _dead;
        
        private void Start()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
        }

        private void Update()
        {
            if (_dead) return;
            if (_enemyHealth.Hit || _enemyHealth.IsDead)
            {
                gruntAudio.mute = true;
            }

            if (!_hit && _enemyHealth.Hit)
            {
                _hit = true;
                StartCoroutine(PlayHit());
            }

            if (_enemyHealth.IsDead)
            {
                _dead = true;
                deadAudio.Play();
            }
        }

        private IEnumerator PlayHit()
        {
            hitAudio.Play();
            yield return new WaitForSeconds(.5f);
            _hit = false;
        }
    }
}