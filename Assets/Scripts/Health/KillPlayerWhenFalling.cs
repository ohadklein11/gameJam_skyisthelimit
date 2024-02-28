using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class KillPlayerWhenFalling : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float killTime = 0.5f;
    private GameData _gameData;

    private float _timeleftToKillWhileFalling;

    private void Start()
    {
        _gameData = GameData.Instance;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // check if no ground below player with raycast
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (!hit)
        {
            if (_timeleftToKillWhileFalling <= 0)
            {
                EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit,_gameData.maxPlayerLifePoints);
            }
            _timeleftToKillWhileFalling -= Time.fixedDeltaTime;
        }
        else
        {
            _timeleftToKillWhileFalling = killTime;
        }
    }
}
