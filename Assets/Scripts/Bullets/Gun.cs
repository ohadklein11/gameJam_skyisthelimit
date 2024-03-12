using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Bullets
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private BeansShooting beansShootingScript;
        [SerializeField] private AudioSource audioCollect;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider2D;
        private TweenerCore<Vector3,Vector3,VectorOptions> sizeTween;
        private Tweener colorTween;

        [SerializeField] private GameObject shadow;


        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider2D = GetComponent<Collider2D>();
            // make transform float up and down
            // transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            sizeTween=transform.DOScale(new Vector3(.35f, .35f, 1f), 1.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            
            // create Color 58B158
            Color endColor = new Color(0.345098f, 0.6941177f, 0.345098f, 1f);
            colorTween=GetComponent<SpriteRenderer>().DOBlendableColor(endColor, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

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
            sizeTween.Kill();
            colorTween.Kill();
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
            shadow.SetActive(false);
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}
