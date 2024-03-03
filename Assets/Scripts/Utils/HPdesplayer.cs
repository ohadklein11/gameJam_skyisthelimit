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
    private TweenerCore<float,float,FloatOptions> tween;

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
            tween?.Kill();
            tween = hpSlider.DOFillAmount(newFill, 1f).SetEase(Ease.OutCubic);
            _curHP = curHP;
        }
    }
}
