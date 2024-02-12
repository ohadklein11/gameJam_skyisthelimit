using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeansShooting : MonoBehaviour
{
    // [SerializeField] private float gunShootingDistance = 20f;
    [SerializeField] private float shootingForce = 20f;

    [SerializeField] private float aimDistance = 20f;
    [SerializeField] private GameObject aimPoint;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject trajectoryPointPrefab;
    [SerializeField] private int trajectoryPointsCount = 20;
    private GameObject[] trajectoryPoints;


    [SerializeField] private GameObject beanPrefab;

    [SerializeField] private LayerMask platformLayerMask;

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
    }

    Vector2 TrajectoryPointsPosition(float t)
    {
        Vector3 direction = (transform.localScale.x >= 0) ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);

        Vector2 currentPointPosition = (Vector2)shootingPoint.transform.position +
                                       ((Vector2)direction * (shootingForce * t)+_rigidBody.velocity) +
                                       (Physics2D.gravity * (t * t * 0.5f));
        return currentPointPosition;
    }

    private void ShowTrajectoryPoints()
    {
        float timeStep = 0.1f;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            trajectoryPoints[i].transform.position = TrajectoryPointsPosition(i * timeStep);
            trajectoryPoints[i].SetActive(true);
        }
    }

    void ShootBeans()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // transform.Rotate(Vector3.up * speed * Time.deltaTime);
            GameObject projectile = Instantiate(beanPrefab, shootingPoint.transform.position, gun.transform.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();


            Vector3 direction = (transform.localScale.x >= 0) ? new Vector3(1, 1, 0) : new Vector3(-1, 1, 0);
            rb.velocity = direction * shootingForce + (Vector3)_rigidBody.velocity;
            ShowTrajectoryPoints();
        }
    }
}