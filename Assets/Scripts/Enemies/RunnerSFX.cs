using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Enums;
using Enemies;
using UnityEngine;

public class RunnerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource _audioWalk;
    [SerializeField] private AudioSource _audioNotice;
    [SerializeField] private AudioSource _audioGrunt;
    [SerializeField] private AudioSource _audioRun;
    [SerializeField] private AudioSource _audioHit;
    [SerializeField] private AudioSource _audioDead;
    
    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private RunnerBehavior _runnerBehavior;
    private bool _hit;
    private bool _dead;
    private bool _chase;
    private bool _walk;
    private bool _chasedBeforeHit;

    // Update is called once per frame
    void Update()
    {
        if (_dead) return;
        if (!_runnerBehavior.chasingPlayer) _chase = false;
        if (_enemyHealth.IsDead)
        {
            MuteGlobalSounds();
            _audioDead.Play();
            _dead = true;        }
        else if (!_hit && _enemyHealth.Hit)
        {
            _chasedBeforeHit = _chase;
            MuteGlobalSounds();
            _hit = true;
            StartCoroutine(PlayHit());
        } else if (!_hit && _chasedBeforeHit)
        {
            StartCoroutine(PlayChase(false));
            _chasedBeforeHit = false;
            _chase = true;
        } else if (!_chase && !_hit && _runnerBehavior.chasingPlayer)
        {
            MuteGlobalSounds();
            _chase = true;
            StartCoroutine(PlayChase());
            
        } else if (!_chase && !_hit && !_walk)
        {
            MuteGlobalSounds();
            _walk = true;
            _audioWalk.mute = false;
        }
    }

    private IEnumerator PlayChase(bool notice=true)
    {
        if (notice)
        {
            _audioNotice.Play();
            yield return new WaitForSeconds(_runnerBehavior.noticeTime);
        }
        if (!_hit && !_dead)
        {
            _audioRun.mute = false;
            _audioGrunt.mute = false;
        }
    }

    private void MuteGlobalSounds()
    {
        _audioWalk.mute = true;
        _audioRun.mute = true;
        _audioGrunt.mute = true;
        _chase = false;
        _walk = false;
        _hit = false;
    }

    private IEnumerator PlayHit()
    {
        _audioHit.Play();
        yield return new WaitForSeconds(.7f);
        _hit = false;
    }
    
}
