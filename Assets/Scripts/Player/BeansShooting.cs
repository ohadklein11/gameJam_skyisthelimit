using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum GunType
{
    BeansGun,
    EggsGun
}

public class BeansShooting : MonoBehaviour
{
    // [SerializeField] private float gunShootingDistance = 20f;
    private float _shootingForce;
    // private bool _shootingForceRising;
    [SerializeField] private KeyCode switchKey = KeyCode.Q;

    [SerializeField] private float minShootingForce = 10f;
    [SerializeField] private float maxShootingForce = 20f;
    [SerializeField] private float shootingSpeedChange = 0.5f;
    [SerializeField] private float eggShootingSpeed = 10f;
    [SerializeField] private float eggShootingCooldown = 0.4f;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject gun;
    // [SerializeField] private GameObject trajectoryPointPrefab;
    // [SerializeField] private int trajectoryPointsCount = 20;
    [SerializeField] private LayerMask _platformLayerMask;
    // private GameObject[] trajectoryPoints;
    private GameObject _beanPrefab;
    private GameObject _eggPrefab;

    private GunType _gunType;
    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;
    private float _eggShootingCooldownWait;
    private SpriteRenderer _playerSpriteRenderer;
    public bool canShoot = true;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _gunType= GunType.BeansGun;
        _beanPrefab = Resources.Load<GameObject>("Prefabs/BulletsTypes/Bean");
        _eggPrefab = Resources.Load<GameObject>("Prefabs/BulletsTypes/Egg");
    }


    private void Update()
    {
        if (_gunType == GunType.BeansGun)
            ShootBeans();
        else if(_gunType == GunType.EggsGun)
            ShootEggs();
        if (Input.GetKeyDown(switchKey))
        {
            _gunType= (_gunType == GunType.BeansGun) ? GunType.EggsGun : GunType.BeansGun;
        }
    }

    void ShootBeans()
    {
        if (!canShoot)
            return;
        if (Input.GetButtonDown("Fire1"))
        {
            _shootingForce= minShootingForce;
        }

        if (Input.GetButton("Fire1"))
        {
            if (_shootingForce < maxShootingForce)
                _shootingForce += Time.deltaTime*shootingSpeedChange;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            InstantiateBullet(_beanPrefab);
        }
    }
    
    void InstantiateBullet(GameObject bulletPrefab)
    {
        GameObject projectile =
            Instantiate(bulletPrefab, shootingPoint.transform.position, bulletPrefab.transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector3 direction = (transform.localScale.x >= 0) ? Vector2.right : Vector2.left;
        rb.velocity=direction * _shootingForce;
    }

    void ShootEggs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _shootingForce = eggShootingSpeed;
            _eggShootingCooldownWait = 0;
        }
        if (Input.GetButton("Fire1"))
        {
            if (_eggShootingCooldownWait <= 0)
            {
                InstantiateBullet(_eggPrefab);
                _eggShootingCooldownWait = eggShootingCooldown;
            }
            else
            {
                _eggShootingCooldownWait -= Time.deltaTime;
            }
        }
    }
    
    public void SetGunType(GunType gunType)
    {
        _gunType = gunType;
    }
    
    
}