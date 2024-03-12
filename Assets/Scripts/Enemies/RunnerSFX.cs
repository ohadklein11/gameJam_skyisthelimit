using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private Animator animator;  // will hitch a ride
    [SerializeField] private SpriteRenderer exclamation;
    [SerializeField] private SpriteRenderer exclamationOutline;
    private bool _hit;
    private bool _dead;
    private bool _chase;
    private bool _walk;
    private bool _chasedBeforeHit;
    private static readonly int Running = Animator.StringToHash("running");
    private static readonly int Noticing = Animator.StringToHash("noticing");

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
            MuteGlobalSounds();
            _chase = true;
            _chasedBeforeHit = false;
            StartCoroutine(PlayChase(false));
        } else if (!_chase && !_hit && _runnerBehavior.chasingPlayer)
        {
            MuteGlobalSounds();
            _chase = true;
            StartCoroutine(PlayChase());
            
        } else if (!_chase && !_hit && !_walk)
        {
            MuteGlobalSounds();
            if (animator.GetBool(Running))
                animator.SetBool(Running, false);
            _walk = true;
            _audioWalk.mute = false;
        }
    }

    private IEnumerator PlayChase(bool notice=true)
    {
        if (notice)
        {
            var yOffset = .3f;
            var transform1 = exclamation.transform;
            var localPosition = transform1.localPosition;
            var position = localPosition;
            var yPos = localPosition.y;
            position -= new Vector3(0, yOffset, 0);
            transform1.localPosition = position;
            var transitionTime = .3f;
            exclamation.transform.DOLocalMoveY(yPos, transitionTime);
            exclamation.DOFade(1, transitionTime);
            exclamationOutline.DOFade(.75f, transitionTime);
            animator.SetBool(Noticing, true);
            _audioNotice.Play();
            yield return new WaitForSeconds(_runnerBehavior.noticeTime);
            exclamation.DOFade(0, .05F);
            exclamationOutline.DOFade(0, .05F);
            animator.SetBool(Noticing, false);
        }
        if (_chase)
        {
            animator.SetBool(Running, true);
            if (!_hit && !_dead)
            {
                _audioRun.mute = false;
                _audioGrunt.mute = false;
            }
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
        yield return new WaitForSeconds(.5f);
        _hit = false;
    }
    
}
