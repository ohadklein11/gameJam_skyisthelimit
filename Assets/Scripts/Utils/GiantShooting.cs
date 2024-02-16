using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantShooting : MonoBehaviour
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
        Vector3 CalculateVelocityFromAngle(Vector3 origin, Vector3 target, float angle)
        {
            Vector3 distance = target - origin;
            distance.y = 0;
            float horizontalDistance = distance.magnitude;
            float angleRad = angle * Mathf.Deg2Rad;
            float velocity = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));
            float x = velocity * Mathf.Cos(angleRad);
            float y = velocity * Mathf.Sin(angleRad);
            return new Vector3(x, y, 0);
        }
}
