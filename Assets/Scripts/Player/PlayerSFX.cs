using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioWalk;
        [SerializeField] private AudioSource audioClimb;
        [SerializeField] private AudioSource audioJump;
        [SerializeField] private AudioSource audioLand;
        [SerializeField] private AudioSource audioHit;
        [SerializeField] private AudioSource audioDead;
    
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerHealthManager playerHealth;
        private bool _hit;
        private bool _dead;
        private bool _jump;
        private bool _land;
        private bool _climb;
        private bool _walk;

        private void Start()
        {
            MuteGlobalSounds();
        }

        // Update is called once per frame
        void Update()
        {
            if (_dead) return;
            if (playerHealth.IsDead)
            {
                audioDead.Play();
                _dead = true;        }
            else if (!_hit && playerHealth.Hit)
            {
                _hit = true;
                StartCoroutine(PlayHit());
            }
            if (!_jump && playerMovement.Jumping)
            {
                _jump = true;
                _land = false;
                audioJump.Play();
            }
            if (_jump && !playerMovement.Jumping && !playerMovement.Falling)
            {
                _jump = false;
                if (!_land)
                {
                    _land = true;
                    audioLand.Play();
                }
            }
            if (!_climb && playerMovement.ActivelyClimbing)
            {
                _climb = true;
                audioClimb.mute = false;
            } else if (_climb && !playerMovement.ActivelyClimbing)
            {
                _climb = false;
                audioClimb.mute = true;
            }
            if (!_walk && playerMovement.Walking)
            {
                _walk = true;
                audioWalk.mute = false;
            } else if (_walk && !playerMovement.Walking)
            {
                _walk = false;
                audioWalk.mute = true;
            }
            
        }

        private void MuteGlobalSounds()
        {
            audioWalk.mute = true;
            audioClimb.mute = true;
            _climb = false;
            _walk = false;
            _hit = false;
        }

        private IEnumerator PlayHit()
        {
            audioHit.Play();
            yield return new WaitForSeconds(2.1f);
            _hit = false;
        }
    
    }
}
