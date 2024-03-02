using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Utils;

public class TeleportPlayerWhenFalling : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private int fallingDamage = 5;
    [SerializeField] private float teleportDistance = 5;

    // [SerializeField] private float killTime = 0.5f;
    private GameData _gameData;
    private PlayerMovement _playerMovement;
    private Vector3 _lastGroundPoint;
    private SpriteRenderer _playerSpriteRenderer;

    // private float _timeleftToKillWhileFalling;

    private void Start()
    {
        _gameData = GameData.Instance;
        _playerMovement = player.GetComponent<PlayerMovement>();
        _playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_gameData.isGiantFightOver) return;
        if (_playerMovement.grounded)
        {
            var halfSize = _playerSpriteRenderer.bounds.extents;
            var raycastOriginLeft =
                transform.position + new Vector3(-halfSize.x-.5f, -halfSize.y - .1f, 0);
            var raycastOriginRight =
                transform.position + new Vector3(halfSize.x+.5f, -halfSize.y - .1f, 0);
            var hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector3.down, .3f, LayerMask.GetMask("Ground"));
            var hitRight = Physics2D.Raycast(raycastOriginRight, Vector3.down, .3f, LayerMask.GetMask("Ground"));

            if (hitLeft && hitRight)
            {
                _lastGroundPoint = player.transform.position;
            }
        }

        // if player falls below last ground point, kill player
        if (_lastGroundPoint.y - player.transform.position.y > teleportDistance)
        {
            Debug.Log("Player fell too far");
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit,fallingDamage);
            player.transform.position = _lastGroundPoint;
        }
        
        
        // check if no ground below player with raycast
        // RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        // if (!hit)
        // {
        //     if (_timeleftToKillWhileFalling <= 0)
        //     {
        //         EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit,_gameData.maxPlayerLifePoints);
        //     }
        //     _timeleftToKillWhileFalling -= Time.fixedDeltaTime;
        // }
        // else
        // {
        //     _timeleftToKillWhileFalling = killTime;
        // }
    }
}
