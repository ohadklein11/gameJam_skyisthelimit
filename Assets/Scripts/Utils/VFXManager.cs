using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Utils
{
    public class VFXManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem dustVFX;
        [SerializeField] private ParticleSystem beanDustVFX;
        [SerializeField] private ParticleSystem leavesVFX;
        [SerializeField] private ParticleSystem shinyVFX;
        [SerializeField] private ParticleSystem stonePiecesVFX;
    
        private ObjectPool<ParticleSystem> _dustVFXs;
        private ObjectPool<ParticleSystem> _beanDustVFXs;
        private ObjectPool<ParticleSystem> _stoneVFXs;

        
        private static VFXManager Instance { get; set; }
        
        private void Awake() => Instance = this;
        
        void Start()
        {
            _dustVFXs = InitObjectPool(dustVFX);
            _beanDustVFXs = InitObjectPool(beanDustVFX);
            _stoneVFXs = InitObjectPool(stonePiecesVFX);
        }

        private ObjectPool<ParticleSystem> InitObjectPool(ParticleSystem vfxPS)
        {
            return new ObjectPool<ParticleSystem>(() => Instantiate(vfxPS, transform, true),
                (vfx) =>
                {
                    vfx.gameObject.SetActive(true);
                    vfx.Play();
                }, (vfx) =>
                {
                    vfx.Stop();
                    vfx.gameObject.SetActive(false);
                });
        }
        
        private IEnumerator VFXCoroutine(ObjectPool<ParticleSystem> pool, Vector3 position)
        {
            var vfx = pool.Get();
            vfx.gameObject.transform.position = position;
            yield return new WaitForSeconds(vfx.main.duration);
            pool.Release(vfx);
        }

        private void DustVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_dustVFXs, position));

        }
        
        private void BeanDustVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_beanDustVFXs, position));
        }

        private void LeavesVFX(Vector3 position)
        {
            leavesVFX.transform.position = position;
            leavesVFX.Play();
        }
        
        private void ShinyVFX(Vector3 position)
        {
            shinyVFX.transform.position = position;
            shinyVFX.Play();
        }
        
        private void StonePiecesVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_stoneVFXs, position));

        }
        
        public static void PlayDustVFX(Vector3 position) => Instance.DustVFX(position);
        public static void PlayBeanDustVFX(Vector3 position) => Instance.BeanDustVFX(position);
        public static void PlayLeavesVFX(Vector3 position) => Instance.LeavesVFX(position);
        public static void PlayShinyVFX(Vector3 position) => Instance.ShinyVFX(position);
        public static void PlayStonePiecesVFX(Vector3 position) => Instance.StonePiecesVFX(position);
    }
}
