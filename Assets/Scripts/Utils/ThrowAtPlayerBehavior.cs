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
        
        public void Init(GameObject throwable, float minThrowTime, float maxThrowTime, float minThrowAngle, float maxThrowAngle, Vector3 throwPosition)
        {
            _throwable = throwable;
            _player = GameObject.FindWithTag("Player");
            _minThrowTime = minThrowTime;
            _maxThrowTime = maxThrowTime;
            _timeToThrow = _maxThrowTime;
            _throwableGravityScale = _throwable.GetComponent<Rigidbody2D>().gravityScale;
            _throwablePool = new ObjectPool<GameObject>(
                () => Instantiate(throwable, throwPosition, Quaternion.identity), 
                o => {
                    o.SetActive(true);
                    o.transform.position = throwPosition;
                    o.TryGetComponent(out IThrowable throwableBehavior);
                    throwableBehavior.Init(_throwablePool, _player);
                    // set throw direction & force so throwable will hit the player
                    float throwAngle = Random.Range(minThrowAngle, maxThrowAngle);
                    o.GetComponent<Rigidbody2D>().velocity = FindThrowVelocity(
                        throwPosition, _player.transform.position, throwAngle);
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
            if (_timeToThrow <= 0)
            {
                ThrowThrowable();
                _timeToThrow = Random.Range(_minThrowTime, _maxThrowTime);
            }
            _timeToThrow -= Time.deltaTime;
        }
        
        private void ThrowThrowable()
        {
            _throwablePool.Get();
        }
    }
    
}