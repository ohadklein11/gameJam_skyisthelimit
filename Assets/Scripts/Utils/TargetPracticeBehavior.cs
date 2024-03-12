using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils
{
    public class TargerPracticeBehavior : MonoBehaviour
    {
        [SerializeField] private BeansShooting beansShooting;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource smashSound;
        [SerializeField] private AudioSource finishSound;
        private static readonly int Hit = Animator.StringToHash("Hit");
        [SerializeField] private Collider2D collider2d;

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
            animator.SetTrigger(Hit);
            smashSound.Play();
            collider2d.enabled = false;
            yield return new WaitForSeconds(0.5f);
            finishSound.Play();
            Destroy(transform.parent.gameObject);
            yield return null;
        }
    }
}
