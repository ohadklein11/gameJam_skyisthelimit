using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Bullets
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private BeansShooting beansShootingScript;
        [SerializeField] private AudioSource audioCollect;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;


        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            // make transform float up and down
            transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        // Start is called before the first frame update
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                playerAnimator.SetTrigger(BeansShooting.BeanChange);
                beansShootingScript.canShoot = true;
                StartCoroutine(Release());
            }
        }
        private IEnumerator Release()
        {
            audioCollect.Play();
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
