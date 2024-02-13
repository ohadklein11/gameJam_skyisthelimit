using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeansShooting : MonoBehaviour
{
    // [SerializeField] private float gunShootingDistance = 20f;
    private float _shootingForce;
    private bool _shootingForceRising;

    [SerializeField] private float minShootingForce = 10f;
    [SerializeField] private float maxShootingForce = 20f;
    [SerializeField] private float shootingSpeedChange = 0.5f;


    [SerializeField] private GameObject aimPoint;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private int trajectoryPointsCount = 20;
    [SerializeField] private LayerMask _platformLayerMask;
    private GameObject[] trajectoryPoints;


    [SerializeField] private GameObject beanPrefab;


    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;

    private SpriteRenderer _playerSpriteRenderer;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        trajectoryPoints = new GameObject[trajectoryPointsCount];
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i] = Instantiate(trajectoryPointPrefab, transform.position, Quaternion.identity);
            trajectoryPoints[i].SetActive(false);
        }
    }


    private void Update()
    {
        ShootBeans();
    }Vector2 TrajectoryPointsPosition(float t)
         {
             Vector3 direction = (transform.localScale.x >= 0) ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);
     
             Vector2 currentPointPosition = (Vector2)shootingPoint.transform.position +
                                            ((Vector2)direction * (_shootingForce * t) + _rigidBody.velocity) +
                                            (Physics2D.gravity * (t * t * 0.5f));
             return currentPointPosition;
         }

    

    private void ShowTrajectoryPoints()
    {
        float timeStep = 0.1f;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i].transform.position = TrajectoryPointsPosition(i * timeStep);
            if (i >= 1)
            {
                var hit = Physics2D.Linecast(trajectoryPoints[i].transform.position,
                    trajectoryPoints[i - 1].transform.position, _platformLayerMask);
                if (hit)
                    return;
            }

            trajectoryPoints[i].SetActive(true);
        }
    }
    
    private void DeleteTrajectoryPoints()
    {
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i].SetActive(false);
        }
    }

    void ShootBeans()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // transform.Rotate(Vector3.up * speed * Time.deltaTime);
            _shootingForce= minShootingForce;
            _shootingForceRising = true;
            
        }

        if (Input.GetButton("Fire1"))
        {
            ShowTrajectoryPoints();
            if (_shootingForceRising)
            {
                _shootingForce += Time.deltaTime*shootingSpeedChange;
                if (_shootingForce >= maxShootingForce)
                {
                    _shootingForceRising = false;
                }
            }
            else
            {
                _shootingForce -= Time.deltaTime*shootingSpeedChange;
                if (_shootingForce <= minShootingForce)
                {
                    _shootingForceRising = true;
                }
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            GameObject projectile = Instantiate(beanPrefab, shootingPoint.transform.position, gun.transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector3 direction = (transform.localScale.x >= 0) ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);
            rb.velocity = direction * _shootingForce + (Vector3)_rigidBody.velocity;
            DeleteTrajectoryPoints();
        }
    }
}