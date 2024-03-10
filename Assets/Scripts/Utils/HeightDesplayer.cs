using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class HeightDesplayer : MonoBehaviour
{
    [SerializeField] private Image heightSlider;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossLevel;

    private GameData _gameData;
    private PlayerMovement _playerMovement;
    private float _minHeight;
    private float _maxHeight;
    private float _curHeight;
    private float _minFill = 0;
    private float _maxFill = 0.9f;
    private float _curFill;
    private TweenerCore<float,float,FloatOptions> tween;

    // Start is called before the first frame update
    void Start()
    {
        _gameData = GameData.Instance;
        _gameData.RestartPlayerHealth();
        _minHeight = player.transform.position.y;
        _maxHeight = bossLevel.transform.position.y;
        _curHeight = _minHeight;
        _curFill = 0;
        heightSlider.fillAmount = _curFill;
        _playerMovement = player.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        var curHeight = player.transform.position.y;
        if ((int)_curHeight != (int)curHeight && !_playerMovement.Falling && !_playerMovement.Jumping)
        {
            var newFill = Mathf.Lerp(_minFill, _maxFill, curHeight / _maxHeight);
            tween?.Kill();
            tween = heightSlider.DOFillAmount(newFill, 1f).SetEase(Ease.OutCubic);
            _curHeight = curHeight;
        }
    }
}
