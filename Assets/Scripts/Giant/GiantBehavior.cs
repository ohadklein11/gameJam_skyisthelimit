using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Pool;

namespace Giant
{
    public class GiantBehavior : MonoBehaviour
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
        private float _timeToThrow;
        private Vector3 _throwPosition;
        private float _throwableGravityScale;
        private ObjectPool<GameObject> _throwablePool;
        [SerializeField] private GiantFightManager giantFightManager;
    
        void Awake()
        {
            _eye = transform.GetChild(0).gameObject;
            _player = GameObject.FindWithTag("Player");
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _fighting = false;
            _crying = false;
            _standing = false;
            _timeToThrow = maxThrowTime;
            _throwPosition = transform.GetChild(1).gameObject.transform.position;
            _throwableGravityScale = throwable.GetComponent<Rigidbody2D>().gravityScale;
            _throwablePool = new ObjectPool<GameObject>(
                () => Instantiate(throwable, _throwPosition, Quaternion.identity), 
                o => {
                    o.SetActive(true);
                    o.transform.position = _throwPosition;
                    o.GetComponent<GiantThrowableBehavior>().Init(_throwablePool, _player);
                    // set throw direction & force so throwable will hit the player
                    float throwAngle = UnityEngine.Random.Range(minThrowAngle, maxThrowAngle);
                    o.GetComponent<Rigidbody2D>().velocity = FindThrowVelocity(
                        _throwPosition, _player.transform.position, throwAngle);
                }, o => o.SetActive(false));
        }
    
        public void StartGiantFight()
        {
            _fighting = true;
        }

        private Vector2 FindThrowVelocity(Vector3 origin, Vector3 target, float angle)
        {
            Vector2 velocity = Vector2.zero;
            float magnitude;
            float x = (target.x - origin.x);
            float y = (target.y - origin.y);
            float g = Physics2D.gravity.magnitude * _throwableGravityScale;
            float newMag = GiantShooting.MagnitudeToReachXYInGravityAtAngle(x, y, g, angle);
            if (float.IsNaN(newMag))
            {
                magnitude = 9f;
            }
            else
            {
                magnitude = newMag;
            
            }
            velocity.x = Mathf.Cos(angle * Mathf.Deg2Rad) * magnitude;
            velocity.y = Mathf.Sin(angle * Mathf.Deg2Rad) * magnitude;
            return velocity;
        }

        private void FightBehavior()
        {
            if (_timeToThrow <= 0)
            {
                ThrowThrowable();
                _timeToThrow = UnityEngine.Random.Range(minThrowTime, maxThrowTime);
            }
            _timeToThrow -= Time.deltaTime;
            
            // check if eye is hit by peas
            var radius = .6f;
            RaycastHit2D hit = Physics2D.Raycast(_eye.transform.position - new Vector3(0, radius, 0),
                Vector2.up, 2*radius, LayerMask.GetMask("Bullets"));
            if (_fighting && hit.collider != null)
            {
                StartCoroutine(ChangePhaseToCrying());
            }
        }

        private void ThrowThrowable()
        {
            _throwablePool.Get();
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
        
            yield return new WaitForSeconds(5f);
        }
    }
}
