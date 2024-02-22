using UnityEngine;
using UnityEngine.Pool;

namespace Stones
{
    public interface IThrowable
    {
        void Init(ObjectPool<GameObject> throwablePool, GameObject player);
    }
}