using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class HPdesplayer : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI hpText;
    private GameData _gameData;
    private int _maxPlayerLifePoints;
    // Start is called before the first frame update
    void Start()
    {
        _gameData = GameData.Instance;
        _gameData.RestartPlayerHealth();
        _maxPlayerLifePoints = _gameData.maxPlayerLifePoints;
        hpSlider.maxValue = _maxPlayerLifePoints;
        hpText.text = _gameData.GetPlayerHealth().ToString() + "/" + _maxPlayerLifePoints.ToString();
    }
    void Update()
    {
        hpSlider.value = _gameData.GetPlayerHealth();
        hpText.text = _gameData.GetPlayerHealth().ToString() + "/" + _maxPlayerLifePoints.ToString();
    }
}
