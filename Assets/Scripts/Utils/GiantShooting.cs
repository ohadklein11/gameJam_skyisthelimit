using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantShooting
{
    // void InstantiateBullet(GameObject bulletPrefab)
    //     {
    //         
    //         var velocity = CalculateVelocityFromAngle(shootingPoint.transform.position, target.transform.position, 60f);
    //
    //         GameObject projectile =
    //             Instantiate(bulletPrefab, shootingPoint.transform.position, Quaternion.identity);
    //         gun.transform.rotation= Quaternion.LookRotation(velocity);
    //         Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    //         Vector3 direction = (transform.localScale.x >= 0) ? Vector2.right : Vector2.left;
    //         // rb.AddForce(direction * _shootingForce, ForceMode2D.Impulse);
    //         rb.velocity = velocity;
    //         Debug.Log(rb.velocity);
    //     }
    public static float MagnitudeToReachXYInGravityAtAngle(float x, float y, float g, float ang)
    {
        float sin2Theta = Mathf.Sin(2 * ang * Mathf.Deg2Rad);
        float cosTheta = Mathf.Cos(ang * Mathf.Deg2Rad);
        float inner = (x * x * g) / (x * sin2Theta - 2 * y * cosTheta * cosTheta);
        if (inner < 0)
        {
            return float.NaN;
        }
        float res = Mathf.Sqrt(inner);
        return res;
    }
}