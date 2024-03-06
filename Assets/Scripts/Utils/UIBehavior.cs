using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // make transform float up and down
        transform.DOLocalMoveY(transform.localPosition.y + 0.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
