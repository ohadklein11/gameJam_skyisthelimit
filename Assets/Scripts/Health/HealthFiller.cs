using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HealthFiller : MonoBehaviour
{
    [SerializeField] private int healthValue;

    void Start()
    {
        // transform.DOScale(new Vector3(.35f, .35f, 1f), 1.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 2.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        // set end color to 328E35
        Color endColor = new Color(0.1960785f, 0.5568628f, 0.2078432f, 1f);
        GetComponent<SpriteRenderer>().DOBlendableColor(endColor, 1f).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.HealthRecovery, healthValue);
            AudioManager.PlayHealthUp();
            Destroy(gameObject);
        }
    }
}
