using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Gun : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private BeansShooting beansShootingScript; 
    [SerializeField] private GameObject gunNotPickedUpBarrier;


    void Start()
    {
        // make transform float up and down
        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gunNotPickedUpBarrier.SetActive(false);
            playerAnimator.SetTrigger(BeansShooting.BeanChange);
            // playerAnimator.Play("IdleBean");
            beansShootingScript.canShoot = true;
            Destroy(gameObject);
        }
    }
}
