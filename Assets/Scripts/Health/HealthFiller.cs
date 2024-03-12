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
        transform.DOScale(new Vector3(.35f, .35f, 1f), 1.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        // transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.HealthRecovery, healthValue);
            Destroy(gameObject);
        }
    }
}
