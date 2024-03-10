using System.Collections;
using UnityEngine;
using Utils;

namespace Bullets
{
    public class Egg : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSmash;
        private MeshRenderer _meshRenderer;
        private Collider2D _collider2D;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider2D = GetComponent<Collider2D>();
        }
        
        void OnCollisionEnter2D(Collision2D other)
        {
        
            StartCoroutine(Release());
        }
    
    
        private IEnumerator Release()
        {
            VFXManager.PlayEggPiecesVFX(transform.position);
            audioSmash.Play();
            _meshRenderer.enabled = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
