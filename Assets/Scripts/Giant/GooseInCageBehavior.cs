using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Giant
{
    public class GooseInCageBehavior : MonoBehaviour
    {
        [SerializeField] private GiantFightManager giantFightManager;
        [SerializeField] public AudioSource audioGoose;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
    
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                giantFightManager.TookGoose();
                StartCoroutine(Release());
            }
        }
    
        private IEnumerator Release()
        {
            audioGoose.Play();
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
