using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public class TargerPracticeBehavior : MonoBehaviour
    {
        [SerializeField] private BeansShooting beansShooting;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullets"))
            {
                if (other.GetComponent<Bean>().StartVelocity >= beansShooting.maxShootingForce - .02f)
                    StartCoroutine(Release());
                Destroy(other.gameObject);
            }
        }

        private IEnumerator Release()
        {
            // later animate
            Destroy(transform.parent.gameObject);
            yield return null;
        }
    }
}
