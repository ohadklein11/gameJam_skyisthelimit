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
    
        private ObjectPool<ParticleSystem> _beanDustVFXs;
        
        private static VFXManager Instance { get; set; }
        
        private void Awake() => Instance = this;
        
        void Start()
        {
            _beanDustVFXs = new ObjectPool<ParticleSystem>(() => Instantiate(beanDustVFX, transform, true),
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

        private void DustVFX(Vector3 position)
        {
            dustVFX.transform.position = position;
            dustVFX.Play();
        }
        
        private void BeanDustVFX(Vector3 position)
        {
            StartCoroutine(BeanDustVFXCoroutine(position));
        }

        private IEnumerator BeanDustVFXCoroutine(Vector3 position)
        {
            var vfx = _beanDustVFXs.Get();
            vfx.gameObject.transform.position = position;
            yield return new WaitForSeconds(vfx.main.duration + 2f);
            _beanDustVFXs.Release(vfx);
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
            stonePiecesVFX.transform.position = position;
            stonePiecesVFX.Play();
        }
        
        public static void PlayDustVFX(Vector3 position) => Instance.DustVFX(position);
        public static void PlayBeanDustVFX(Vector3 position) => Instance.BeanDustVFX(position);
        public static void PlayLeavesVFX(Vector3 position) => Instance.LeavesVFX(position);
        public static void PlayShinyVFX(Vector3 position) => Instance.ShinyVFX(position);
        public static void PlayStonePiecesVFX(Vector3 position) => Instance.StonePiecesVFX(position);
    }
}
