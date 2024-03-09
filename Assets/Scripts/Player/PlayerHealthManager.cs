using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool _isDead;
    public bool IsDead => _isDead;
    private GameData _gameData;
    private bool _canPlayerTakeDamage = true;
    public bool Hit => !_canPlayerTakeDamage;
    
    // Start is called before the first frame update
    void Start()
    {
        _isDead = false;
        _gameData = GameData.Instance;
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerGotHit, PlayerGotHit);
        EventManagerScript.Instance.StartListening(EventManagerScript.HealthRecovery, HealthPointsFiller);

    }
    private void PlayerGotHit(object damage)
    {
        if (!_canPlayerTakeDamage) return;
        StartCoroutine(CooldownTimer());
        _gameData.SetPlayerHealth( Math.Max(_gameData.GetPlayerHealth()-(int)damage,0));
        Debug.Log("player took " + damage + " damage, " + _gameData.GetPlayerHealth() + " life points left");
            
        if (_gameData.GetPlayerHealth() <= 0&&!_isDead)
        {
            _isDead = true;
            Debug.Log("Player died");
            StartCoroutine(PlayDeadPlayer());
        }
    }
    private void HealthPointsFiller(object healthPoints)
    {
        _gameData.SetPlayerHealth(Math.Min(_gameData.maxPlayerLifePoints,_gameData.GetPlayerHealth()+(int)healthPoints));
        Debug.Log("player recovered " + healthPoints + " health points, " + _gameData.GetPlayerHealth() + " life points left");

    }
    
    void UpdatePlayerColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    IEnumerator CooldownTimer()
    {
        _canPlayerTakeDamage = false;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(1f);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Color.red,
            "to", Color.white,
            "time", 1,
            "onupdate", "UpdatePlayerColor",
            "easetype", iTween.EaseType.easeOutSine));
        yield return new WaitForSeconds(1f);
        _canPlayerTakeDamage = true;
    }
    
    IEnumerator PlayDeadPlayer()
    {
        _isDead = true;
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerDead,null);
        AudioManager.StopCurrentBGM();
        AudioManager.PlayLoseBackground();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LostScene");  // todo move to the same scene
    }
}
