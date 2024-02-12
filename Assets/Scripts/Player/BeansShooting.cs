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

    [SerializeField] private GameObject beanPrefab;

    [SerializeField] private LayerMask platformLayerMask;
    
    private Camera _mainCamera;

    private SpriteRenderer _playerSpriteRenderer;


    void Awake()
    {
        _mainCamera = Camera.main;
        _playerSpriteRenderer= GetComponent<SpriteRenderer>();
    }

    

   
    private void Update()
    {
        ShootBeans();
    }

    void HandleGunRotation()
    {
        
    }
    
    void ShootBeans()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // transform.Rotate(Vector3.up * speed * Time.deltaTime);
            GameObject projectile = Instantiate(beanPrefab, shootingPoint.transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            // use the gun rotation to determine the direction of the projectile
            
            
            Debug.Log(gun.transform.eulerAngles);
            Vector3 direction = (transform.localScale.x>=0)? new Vector3(1,1,0): new Vector3(-1,1,0);
            rb.velocity = transform.forward * shootingForce;        
        }
    }
    // void LaunchProjectile()
    // {
    //     Vector3 newAimPoint = aimPoint.transform.position;
    //     float distance = _playerSpriteRenderer.flipX? -aimDistance: aimDistance;
    //     newAimPoint+= new Vector3(distance,0,0);
    //     aimPoint.transform.position =newAimPoint;
    //     Ray ray = _mainCamera.ScreenPointToRay(aimPoint.transform.position);
    //     RaycastHit hit;
    //
    //     if (Physics.Raycast(ray, out hit, 100f,platformLayerMask))
    //     {
    //         aimPoint.SetActive(true);
    //         float timeToTarget = 1f;
    //         Vector3 velocity = CalculateVelocity(hit.point, shootingPoint.transform.position, timeToTarget);
    //         Debug.Log(velocity);
    //         // check if player pressed ctrl
    //         
    //         if (Input.GetButtonDown("Fire1"))
    //         {
    //             GameObject projectile = Instantiate(beanPrefab, shootingPoint.transform.position, Quaternion.identity);
    //             Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    //             rb.velocity = velocity;
    //         }
    //     }
    //     
    // }
    
    
    
    
    
    
    
    
    // Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    // {
    //     Vector3 distance = target - origin;
    //     Vector3 distanceXZ = distance;
    //     distanceXZ.y = 0f;
    //     
    //     float Sy = distance.y;
    //     float Sxz = distanceXZ.magnitude;
    //     
    //     float Vxz = Sxz / time;
    //     float Vy = Sy / time + .5f * Mathf.Abs(Physics.gravity.y) * time;
    //     
    //     Vector3 result = distanceXZ.normalized;
    //     result *= Vxz;
    //     result.y = Vy;
    //     
    //     return result;
    // }
}
