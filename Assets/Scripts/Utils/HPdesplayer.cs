using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class HPdesplayer : MonoBehaviour
{
    [SerializeField] private Image hpSlider;
    private GameData _gameData;

    private int _minHP = 0;
    private int _maxHP;
    private int _curHP;
    private float _minFill = 0.32f;
    private float _maxFill = 0.946f;
    private float _curFill;
    private TweenerCore<float,float,FloatOptions> valTween;
    private TweenerCore<Vector3,Vector3,VectorOptions> sizeTween;
    [SerializeField] private Transform hpTransform;

    // Start is called before the first frame update
    void Start()
    {
        _gameData = GameData.Instance;
        _gameData.RestartPlayerHealth();
        _maxHP = _gameData.maxPlayerLifePoints;
        _curHP = _maxHP;
        _curFill = _maxFill;
        hpSlider.fillAmount = _curFill;
    }
    void Update()
    {
        var curHP = _gameData.GetPlayerHealth();
        if (_curHP != curHP)
        {
            var newFill = Mathf.Lerp(_minFill, _maxFill, (float)curHP / _maxHP);
            valTween?.Kill();
            sizeTween?.Kill();
            valTween = hpSlider.DOFillAmount(newFill, 1f).SetEase(Ease.OutCubic);
            sizeTween = hpTransform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            _curHP = curHP;
        }
    }
}
