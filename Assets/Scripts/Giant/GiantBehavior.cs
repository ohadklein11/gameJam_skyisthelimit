using System;
using System.Collections;
using DG.Tweening;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Utils;

namespace Giant
{
    public class GiantBehavior : MonoBehaviour, IThrower
    {
        private bool _fighting = false;
        
        [SerializeField] private GameObject throwable;
        [SerializeField] private Transform throwPosition;
        [SerializeField] private float minThrowTime;
        [SerializeField] private float maxThrowTime;
        [SerializeField] private float minThrowAngle = 120f;
        [SerializeField] private float maxThrowAngle = 160f;
        [SerializeField] private float throwDelay = .5f;
        private ThrowAtPlayerBehavior _throwAtPlayerBehavior;
        [SerializeField] private int collisionDamage = 5;
        
        private Animator _animator;
        private static readonly int Hit = Animator.StringToHash("hit");

        private static readonly int Roar = Animator.StringToHash("roar");
        private bool _roaring;
       
        private static readonly int Blinking = Animator.StringToHash("blinking");
        [SerializeField] private GiantEyeBehavior giantEyeBehavior;
        [SerializeField] private float minTimeToBlink = 5f;
        [SerializeField] private float maxTimeToBlink = 10f;
        [SerializeField] private float minBlinkDuration = 1f;
        [SerializeField] private float maxBlinkDuration = 10f;
        private float _timeToBlink;
        private float _timeToBlinkEnd;
        private bool _blinking;
       
        private static readonly int Throw = Animator.StringToHash("throw");
        [SerializeField] private GameObject floor;  // used for vine count
        private int _vineCount = 1;
        private bool _throwing;
        
        private static readonly int Stand = Animator.StringToHash("stand");
        [SerializeField] private Transform standPosition;
        private PolygonCollider2D _polygonCollider;
        private BoxCollider2D _boxCollider;
        private static readonly int SmallHit = Animator.StringToHash("small hit");
        private bool _hit;
        private static readonly int Start = Animator.StringToHash("start");

        void Awake()
        {
            _animator = GetComponent<Animator>();
            var playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            _animator.GetBehaviour<GiantStrongRoarBehavior>().Init(playerMovement);
            _throwAtPlayerBehavior = GetComponent<ThrowAtPlayerBehavior>();
            _throwAtPlayerBehavior.Init(throwable, minThrowTime, maxThrowTime, minThrowAngle, maxThrowAngle, throwPosition, this, minThrowTime, throwDelay);
            _throwAtPlayerBehavior.enabled = false;
            _polygonCollider = GetComponent<PolygonCollider2D>();
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (_fighting)
            {
                HandleBlinking();
                HandleRoaring();
                HandleThrowing();
                HandleHit();
            }
        }

        private void HandleHit()
        {
            if (_hit && _animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
            {
                _hit = false;
            }
        }

        private void HandleRoaring()
        {
            if (_roaring && !_animator.GetCurrentAnimatorStateInfo(0).IsName("roar"))
            {
                _roaring = false;
            }
        }

        private void HandleThrowing()
        {
            int vineCount = Mathf.Max(floor.transform.childCount, 1);
            if (vineCount != _vineCount)
            {
                _vineCount = vineCount;
                _throwAtPlayerBehavior.SetMaxThrowTime(Mathf.Max(maxThrowTime - vineCount, minThrowTime));
            }
            if (!_throwing && _throwAtPlayerBehavior.Throwing)
            {
                _animator.SetTrigger(Throw);
                _throwing = true;
            } else if (_throwing && !_throwAtPlayerBehavior.Throwing)
            {
                _throwing = false;
            }
            if (_throwAtPlayerBehavior.IsPaused() && !_blinking && !_hit && _animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
            {
                _throwAtPlayerBehavior.ResumeThrowing();
            }
        }

        private void HandleBlinking()
        {
            if (!_roaring && !_throwAtPlayerBehavior.Throwing)
            {
                if (_timeToBlink > 0)
                {
                    _timeToBlink -= Time.deltaTime;
                    if (_timeToBlink <= 0)
                    {
                        _animator.SetBool(Blinking, true);
                        _timeToBlinkEnd = UnityEngine.Random.Range(minBlinkDuration, maxBlinkDuration);
                        _throwAtPlayerBehavior.PauseThrowing();
                        _blinking = true;
                    }
                }
                else if (_blinking)
                {
                    _timeToBlinkEnd -= Time.deltaTime;
                    if (_timeToBlinkEnd <= 0)
                    {
                        _animator.SetBool(Blinking, false);
                    }
                }
            }
            if (_blinking && !_animator.GetBool(Blinking) && _animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
            {
                _blinking = false;
                _timeToBlink = UnityEngine.Random.Range(minTimeToBlink, maxTimeToBlink);
            }
        }

        public IEnumerator StartGiantFight()
        {
            _animator.SetTrigger(Start);
            yield return new WaitForSeconds(2f);
            _throwAtPlayerBehavior.enabled = true;
            _fighting = true;
            _timeToBlink = UnityEngine.Random.Range(minTimeToBlink, maxTimeToBlink);
        }
        
        
        public void EndGiantFight()
        {
            _throwAtPlayerBehavior.enabled = false;
            _fighting = false;
            _animator.SetTrigger(Hit);
            _throwAtPlayerBehavior.ReleaseAll();
        }


        public void StartEscape()
        {
            _polygonCollider.enabled = false;
            _boxCollider.enabled = true;
            _animator.SetTrigger(Stand);
            transform.DOLocalMove(standPosition.localPosition, 1f).SetEase(Ease.InCubic);
        }

        public int GetDirection()
        {
            return -1;  // always throw to the left
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit, collisionDamage);
            }
            else if (other.gameObject.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("sit"))
                
                {
                    _animator.SetTrigger(Roar);
                    _roaring = true;
                    _throwAtPlayerBehavior.PauseThrowing();
                }
            }
        }

        public void HitGiant()
        {
            _animator.SetTrigger(SmallHit);
            _throwAtPlayerBehavior.PauseThrowing();
            _hit = true;
        }
    }
}
