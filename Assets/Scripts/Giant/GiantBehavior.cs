using System;
using System.Collections;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Giant
{
    public class GiantBehavior : MonoBehaviour, IThrower
    {
        private GameObject _eye;
        private GameObject _player;
        private SpriteRenderer _spriteRenderer;
        private PlayerMovement _playerMovement;

        private bool _fighting;  // phase 1
        private bool _crying;  // phase 2
        private bool _standing;  // phase 3
        
        [SerializeField] private GameObject throwable;
        [SerializeField] private float minThrowTime = 2f;
        [SerializeField] private float maxThrowTime = 5f;
        [SerializeField] private float minThrowAngle = 120f;
        [SerializeField] private float maxThrowAngle = 160f;
        [SerializeField] private GiantFightManager giantFightManager;
        
        private ThrowAtPlayerBehavior _throwAtPlayerBehavior;
        
    
        void Awake()
        {
            _eye = transform.GetChild(0).gameObject;
            _player = GameObject.FindWithTag("Player");
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _fighting = false;
            _crying = false;
            _standing = false;
            var throwPosition = transform.GetChild(1).gameObject.transform;
            _throwAtPlayerBehavior = GetComponent<ThrowAtPlayerBehavior>();
            _throwAtPlayerBehavior.Init(throwable, minThrowTime, maxThrowTime, minThrowAngle, maxThrowAngle, throwPosition, this);
            _throwAtPlayerBehavior.enabled = false;
        }
    
        public void StartGiantFight()
        {
            _fighting = true;
            _throwAtPlayerBehavior.enabled = true;
        }
        
        private void FightBehavior()
        {
            // check if eye is hit by peas
            var radius = .6f;
            RaycastHit2D hit = Physics2D.Raycast(_eye.transform.position - new Vector3(0, radius, 0),
                Vector2.up, 2*radius, LayerMask.GetMask("Bullets"));
            if (_fighting && hit.collider != null)
            {
                StartCoroutine(ChangePhaseToCrying());
            }
        }
    
        // ### crying phase ###
        private void CryBehavior()
        {
            return;
        }
    
        // ### standing phase ###
        private void StandBehavior()
        {
            throw new NotImplementedException();
        }
    
        private void Update()
        {
            if (_fighting)
            {
                FightBehavior();
            } else if (_crying)
            {
                CryBehavior();
            } else if (_standing)
            {
                StandBehavior();
            }
        }
    
        private IEnumerator ChangePhaseToCrying()
        {
            _fighting = false;
            _crying = true;

            giantFightManager.EndGiantFight();
            _throwAtPlayerBehavior.enabled = false;

            yield return new WaitForSeconds(5f);
        }

        public int GetDirection()
        {
            return -1;  // always throw to the left
        }
    }
}
