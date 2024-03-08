using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ThrowerSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioWalk;
        [SerializeField] private AudioSource audioThrow;
        [SerializeField] private AudioSource audioHit;
        [SerializeField] private AudioSource audioDead;
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyHealth enemyHealth;
    
        private bool _hit;
        private bool _dead;
        private bool _throw;
        private bool _walk;
        private Animator _animator;

        private bool Throwing => _animator.GetCurrentAnimatorStateInfo(0).IsName("throw") ||
                                 _animator.GetCurrentAnimatorStateInfo(0).IsName("throw1");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (_dead) return;
            if (!enemyMovement.Moving) _walk = false;
            if (!Throwing) _throw = false;
            if (enemyHealth.IsDead)
            {
                MuteGlobalSounds();
                audioDead.Play();
                _dead = true;        }
            else if (!_hit && enemyHealth.Hit)
            {
                MuteGlobalSounds();
                _hit = true;
                StartCoroutine(PlayHit());
            } else if (!_throw && !_hit && Throwing)
            {
                MuteGlobalSounds();
                _throw = true;
                StartCoroutine(PlayThrow());
            
            } else if (!_throw && !_hit && !_walk && enemyMovement.Moving)
            {
                MuteGlobalSounds();
                _walk = true;
                audioWalk.mute = false;
            }
        }


        private IEnumerator PlayThrow()
        {
            audioThrow.Play();
            yield return new WaitForSeconds(2f);
            _throw = false;
        }

        private void MuteGlobalSounds()
        {
            audioWalk.mute = true;
            _walk = false;
            _hit = false;
            _throw = false;
        }

        private IEnumerator PlayHit()
        {
            audioHit.Play();
            yield return new WaitForSeconds(.7f);
            _hit = false;
        }
    
    }
}
