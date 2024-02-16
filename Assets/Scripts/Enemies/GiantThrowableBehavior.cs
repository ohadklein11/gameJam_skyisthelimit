using UnityEngine;
using UnityEngine.Pool;

namespace Enemies
{
    public class GiantThrowableBehavior : MonoBehaviour
    {
        private ObjectPool<GameObject> _throwablePool;

        private void Awake()
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("GiantThrowables"), LayerMask.NameToLayer("Enemy"));
        }

        public void Init(ObjectPool<GameObject> throwablePool)
        {
            _throwablePool = throwablePool;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                return;
            }
            _throwablePool.Release(this.gameObject);
        }
    }
}
