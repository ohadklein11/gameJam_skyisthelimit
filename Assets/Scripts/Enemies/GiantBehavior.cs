using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;

public class GiantBehavior : MonoBehaviour
{
    private GameObject _eye;
    private GameObject _player;
    
    private bool _sitting;  // phase 1
    private bool _crying;  // phase 2
    private bool _standing;  // phase 3
    
    // ### sitting phase ###
    [SerializeField] private GameObject throwable;
    [SerializeField] private float minThrowTime = 2f;
    [SerializeField] private float maxThrowTime = 5f;
    [SerializeField] private float minThrowAngle = 120f;
    [SerializeField] private float maxThrowAngle = 160f;
    private float _timeToThrow;
    private Vector3 _throwPosition;
    private float _throwableGravityScale;
    private ObjectPool<GameObject> _throwablePool;
    
    void Awake()
    {
        _eye = transform.GetChild(0).gameObject;
        _player = GameObject.FindWithTag("Player");
        _sitting = true;
        _crying = false;
        _standing = false;
        _timeToThrow = maxThrowTime;
        _throwPosition = _eye.transform.position;
        _throwableGravityScale = throwable.GetComponent<Rigidbody2D>().gravityScale;
        _throwablePool = new ObjectPool<GameObject>(
            () => Instantiate(throwable, _throwPosition, Quaternion.identity), 
            o => {
                o.SetActive(true);
                o.transform.position = _throwPosition;
                o.GetComponent<GiantThrowableBehavior>().Init(_throwablePool);
                // set throw direction & force so throwable will hit the player
                float throwAngle = UnityEngine.Random.Range(minThrowAngle, maxThrowAngle);
                o.GetComponent<Rigidbody2D>().velocity = FindThrowVelocity(
                    _throwPosition, _player.transform.position, throwAngle);
                // float throwMagnitude = FindThrowForce(_throwPosition, _playerPosition, throwAngle);
                // o.GetComponent<Rigidbody2D>().velocity = new Vector2(
                //     Mathf.Cos(throwAngle) * throwMagnitude, 
                //     Mathf.Sin(throwAngle) * throwMagnitude);
            }, o => o.SetActive(false));
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

    private void SitBehavior()
    {
        if (_timeToThrow <= 0)
        {
            ThrowThrowable();
            _timeToThrow = UnityEngine.Random.Range(minThrowTime, maxThrowTime);
        }
        _timeToThrow -= Time.deltaTime;
    }

    private void ThrowThrowable()
    {
        _throwablePool.Get();
    }
    
    // ### crying phase ###
    private void CryBehavior()
    {
        throw new NotImplementedException();
    }
    
    // ### standing phase ###
    private void StandBehavior()
    {
        throw new NotImplementedException();
    }
    
    
    
    private void Update()
    {
        if (_sitting)
        {
            SitBehavior();
        } else if (_crying)
        {
            CryBehavior();
        } else if (_standing)
        {
            StandBehavior();
        }
    }
}
