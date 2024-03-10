using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ClimberSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioAppear;
        [SerializeField] private AudioSource audioWalk;
        [SerializeField] private AudioSource audioClimb;
        [SerializeField] private AudioSource audioFall;
        [SerializeField] private AudioSource audioHit;
        [SerializeField] private AudioSource audioDead;
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private ClimberBehavior climberBehavior;
    
        private bool _hit;
        private bool _dead;
        private bool _climb;
        private bool _fall;
        private bool _walk;
        private bool _appeared;

        void Update()
        {
            if (!_appeared && climberBehavior.appearing)
            {
                _appeared = true;
                audioAppear.Play();
            }
            if (!climberBehavior.appeared) return;
            if (_dead) return;
            if (enemyMovement.ReachedTop) audioClimb.mute = true;
            if (enemyMovement.Grounded) _fall = false;
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
            } else if (!_hit && !_climb && enemyMovement.isVertical)
            {
                MuteGlobalSounds();
                _climb = true;
                audioClimb.mute = false;
            } else if (!_hit && _climb && !_fall && !enemyMovement.isVertical) {
                MuteGlobalSounds();
                _fall = true;
                audioFall.Play();
            } else if (!_climb && !_fall && !_hit && !_walk && enemyMovement.Moving)
            {
                MuteGlobalSounds();
                _walk = true;
                audioWalk.mute = false;
            }
        }

        private void MuteGlobalSounds()
        {
            audioWalk.mute = true;
            audioClimb.mute = true;
            _walk = false;
            _climb = false;
            _fall = false;
            _hit = false;
        }

        private IEnumerator PlayHit()
        {
            audioHit.Play();
            yield return new WaitForSeconds(.7f);
            _hit = false;
        }
    
    }
}
