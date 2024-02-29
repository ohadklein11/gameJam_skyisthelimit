using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

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
    [SerializeField] private float beanShootingCooldown = 0.3f;

    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject gun;
    // [SerializeField] private GameObject trajectoryPointPrefab;
    // [SerializeField] private int trajectoryPointsCount = 20;
    [SerializeField] private LayerMask _platformLayerMask;
    // private GameObject[] trajectoryPoints;
    private GameObject _beanPrefab;
    private GameObject _eggPrefab;
    private Rigidbody2D _playerRb;
    public GunType _gunType;
    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;
    private float _shootingCooldownWait;
    private SpriteRenderer _playerSpriteRenderer;
    public bool canShoot = true;
    public bool isLoading => _shootingForce >= minShootingForce;
    public bool IsOnCooldown => _shootingCooldownWait > 0;


    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _gunType= GunType.BeansGun;
        _beanPrefab = Resources.Load<GameObject>("Prefabs/BulletsTypes/Bean");
        _eggPrefab = Resources.Load<GameObject>("Prefabs/BulletsTypes/Egg");
    }

    void Start()
    {
        _playerRb = _playerSpriteRenderer.gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameData.isGameStopped) return;

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
        {
            _shootingForce = 0;
            return;
        }

        _shootingCooldownWait -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            if (_shootingCooldownWait <= 0)
            {
                _shootingForce = minShootingForce;
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (_shootingCooldownWait <= 0)
            {
                if (_shootingForce < maxShootingForce)
                    _shootingForce += Time.deltaTime * shootingSpeedChange;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (_shootingCooldownWait<=0)
            {
                GetComponent<PlayerAnimation>().PlayShootingAnimation(0);
                InstantiateBullet(_beanPrefab);
                _shootingCooldownWait = beanShootingCooldown;
                _shootingForce = 0;
            }
            
        }
    }
    
    void InstantiateBullet(GameObject bulletPrefab)
    {
        GameObject projectile =
            Instantiate(bulletPrefab, shootingPoint.transform.position, bulletPrefab.transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        Vector3 direction = (transform.localScale.x >= 0) ? Vector2.right : Vector2.left;
        rb.velocity=(direction * _shootingForce)+new Vector3(_playerRb.velocity.x,0,0);
    }

    void ShootEggs()
    {
        _shootingCooldownWait -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
        {
            _shootingForce = eggShootingSpeed;
        }
        if (Input.GetButton("Fire1"))
        {
            if (_shootingCooldownWait <= 0)
            {
                GetComponent<PlayerAnimation>().PlayShootingAnimation(1);
                InstantiateBullet(_eggPrefab);
                _shootingCooldownWait = eggShootingCooldown;
            }
        }
    }
    
    public void SetGunType(GunType gunType)
    {
        _gunType = gunType;
    }
    
    public float GetShootingForcePercentage()
    {
        if (_gunType != GunType.BeansGun)
            return 0f;
        return _shootingForce / maxShootingForce;
    }
    
    public float GetCooldownPercentage()
    {
        if (_gunType != GunType.BeansGun)
            return 0f;
        return Mathf.Max(0, _shootingCooldownWait) / beanShootingCooldown;
    }
}