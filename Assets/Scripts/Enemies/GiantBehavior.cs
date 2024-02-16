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
    [SerializeField] private float throwForce = 5f;
    private float _timeToThrow;
    private Vector3 _playerPosition;
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
        _throwablePool = new ObjectPool<GameObject>(
            () => Instantiate(throwable, _eye.transform.position, Quaternion.identity), 
            o => {
                o.GetComponent<GiantThrowableBehavior>().Init(_throwablePool);
                // set throw direction so throwable will hit the player
                float throwAngle = FindThrowAngle(_player.transform.position);
                Vector2 throwDirection = new Vector2(Mathf.Cos(throwAngle), Mathf.Sin(throwAngle));
                o.GetComponent<Rigidbody2D>().AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            });
    }
    
    public float FindThrowAngle(Vector2 playerPos)
    {
        // Calculate the time of flight
        float timeOfFlight = Mathf.Sqrt((2 * (playerPos.y - transform.position.y)) / _throwableGravityScale);

        // Calculate the horizontal distance to the player's future position
        float horizontalDistance = playerPos.x - transform.position.x;

        // Calculate the initial horizontal velocity
        float horizontalVelocity = horizontalDistance / timeOfFlight;

        // Calculate the initial vertical velocity
        float verticalVelocity = (0.5f * _throwableGravityScale * throwForce * timeOfFlight) / throwForce;

        // Calculate the angle in radians
        float angleRad = Mathf.Atan(verticalVelocity / horizontalVelocity);

        // Convert angle to degrees
        float angleDeg = Mathf.Rad2Deg * angleRad;

        return angleDeg;
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
