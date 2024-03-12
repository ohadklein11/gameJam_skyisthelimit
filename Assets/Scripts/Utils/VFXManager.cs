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
        [SerializeField] private ParticleSystem shinyVFX;
        [SerializeField] private ParticleSystem stonePiecesVFX;
        [SerializeField] private ParticleSystem eggPiecesVFX;
        [SerializeField] private ParticleSystem mushPiecesVFX;
    
        private ObjectPool<ParticleSystem> _dustVFXs;
        private ObjectPool<ParticleSystem> _beanDustVFXs;
        private ObjectPool<ParticleSystem> _stoneVFXs;
        private ObjectPool<ParticleSystem> _eggVFXs;
        private ObjectPool<ParticleSystem> _mushVFXs;


        private static VFXManager Instance { get; set; }
        
        private void Awake() => Instance = this;
        
        void Start()
        {
            _dustVFXs = InitObjectPool(dustVFX);
            _beanDustVFXs = InitObjectPool(beanDustVFX);
            _stoneVFXs = InitObjectPool(stonePiecesVFX);
            _eggVFXs = InitObjectPool(eggPiecesVFX);
            _mushVFXs = InitObjectPool(mushPiecesVFX);
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
        
        private void ShinyVFX(Vector3 position)
        {
            shinyVFX.transform.position = position;
            shinyVFX.Play();
        }
        
        private void StonePiecesVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_stoneVFXs, position));
        }
        
        private void EggPiecesVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_eggVFXs, position));
        }
        
        private void MushVFX(Vector3 position)
        {
            StartCoroutine(VFXCoroutine(_mushVFXs, position));
        }
        
        public static void PlayDustVFX(Vector3 position) => Instance.DustVFX(position);
        public static void PlayBeanDustVFX(Vector3 position) => Instance.BeanDustVFX(position);
        public static void PlayShinyVFX(Vector3 position) => Instance.ShinyVFX(position);
        public static void PlayStonePiecesVFX(Vector3 position) => Instance.StonePiecesVFX(position);
        public static void PlayEggPiecesVFX(Vector3 position) => Instance.EggPiecesVFX(position);
        public static void PlayMushVFX(Vector3 position) => Instance.MushVFX(position);
    }
}
