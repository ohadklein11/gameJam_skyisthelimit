using System.Collections;
using Enemies;
using Stones;
using UnityEngine;
using UnityEngine.Pool;

namespace Utils
{
    public class ThrowAtPlayerBehavior : MonoBehaviour
    {
        private GameObject _throwable;
        private GameObject _player;
        private float _minThrowTime;
        private float _maxThrowTime;
        private ObjectPool<GameObject> _throwablePool;
        private float _throwableGravityScale;
        
        private float _timeToThrow;
        private float _throwDuration;
        private float _throwTime;
        private bool _paused;
        private float _throwDelay;
        public bool Throwing => _throwTime > 0;
        
        public void Init(GameObject throwable, float minThrowTime, float maxThrowTime, float minThrowAngle, float maxThrowAngle, Transform throwPosition, IThrower thrower, float throwDuration=0, float throwDelay=0)
        {
            _throwable = throwable;
            _player = GameObject.FindWithTag("Player");
            _minThrowTime = minThrowTime;
            _maxThrowTime = maxThrowTime;
            _timeToThrow = _maxThrowTime;
            _throwDuration = throwDuration;
            _throwDelay = throwDelay;
            _throwableGravityScale = _throwable.GetComponent<Rigidbody2D>().gravityScale;
            _throwablePool = new ObjectPool<GameObject>(
                () => Instantiate(throwable, throwPosition.position, Quaternion.identity), 
                o => {
                    o.SetActive(true);
                    var position = throwPosition.position;
                    o.transform.position = position;
                    o.TryGetComponent(out IThrowable throwableBehavior);
                    throwableBehavior.Init(_throwablePool, _player);
                    // set throw direction & force so throwable will hit the player
                    float throwAngle = Random.Range(minThrowAngle, maxThrowAngle);
                    if (thrower.GetDirection() > 0)
                    {
                        throwAngle = 180 - throwAngle;
                    }
                    o.GetComponent<Rigidbody2D>().velocity = FindThrowVelocity(
                        position, _player.transform.position, throwAngle);
                    o.GetComponent<Rigidbody2D>().angularVelocity = -.5f;
                }, o => o.SetActive(false));;
        }
        
        private Vector2 FindThrowVelocity(Vector3 origin, Vector3 target, float angle)
        {
            Vector2 velocity = Vector2.zero;
            float magnitude;
            float x = (target.x - origin.x);
            float y = (target.y - origin.y);
            float g = Physics2D.gravity.magnitude * _throwableGravityScale;
            float newMag = ThrowHelper.MagnitudeToReachXYInGravityAtAngle(x, y, g, angle);
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
        
        private void Update()
        {
            if (_paused) return;
            
            if (_timeToThrow <= 0)
            {
                StartCoroutine(ThrowThrowable());
                _timeToThrow = Random.Range(_minThrowTime, _maxThrowTime);
                _throwTime = _throwDuration;
            }
            if (_throwTime > 0)
            {
                _throwTime -= Time.deltaTime;
            }
            if (_timeToThrow > 0)
                _timeToThrow -= Time.deltaTime;
        }
        
        private IEnumerator ThrowThrowable()
        {
            yield return new WaitForSeconds(_throwDelay);
            _throwablePool.Get();
        }
        
        public void SetMaxThrowTime(float maxThrowTime)
        {
            _maxThrowTime = maxThrowTime;
        }
        
        public void PauseThrowing()
        {
            _paused = true;
        }
        
        public void ResumeThrowing()
        {
            _paused = false;
        }
        public bool IsPaused()
        {
            return _paused;
        }

        public void ReleaseAll()
        { 
            _throwablePool.Clear();
        }

        public void PauseThrow(float pauseTime)
        {
            StartCoroutine(PauseThrowCoroutine(pauseTime));
        }

        private IEnumerator PauseThrowCoroutine(float pauseTime)
        {
            PauseThrowing();
            yield return new WaitForSeconds(pauseTime);
            ResumeThrowing();
        }
    }
    
}