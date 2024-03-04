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
    [SerializeField] private float saveNewTeleportPointY = 2;


    [SerializeField] private float fallingTimeUntillTeleport = 0.5f;
    private GameData _gameData;
    private PlayerMovement _playerMovement;
    private Vector3 _lastGroundPoint;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _hitSolidGround;

    private float _timeleftToTeleport;

    private void Start()
    {
        _gameData = GameData.Instance;
        _playerMovement = player.GetComponent<PlayerMovement>();
        _playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _hitSolidGround = _playerMovement.grounded && !_playerMovement.falling && !_playerMovement.climbing;
        if (_gameData.isGiantFightOver)
            teleportOnDescend();
        else
            teleportOnAscend();
    }

    private void teleportOnAscend()
    {
        if (_hitSolidGround && player.transform.position.y-_lastGroundPoint.y>=saveNewTeleportPointY)
        {
            _lastGroundPoint = player.transform.position;
        }
        // if player falls below last ground point, kill player
        if (_lastGroundPoint.y - player.transform.position.y > teleportDistance)
        {
            Teleport();
        }
    }

    void Update()
    {
        _timeleftToTeleport -= Time.deltaTime;

    }

    private void teleportOnDescend()
    {
        // check if no ground below player with raycast
        RaycastHit2D hitGroundBeneath = Physics2D.Raycast(player.transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
        if (_hitSolidGround && _lastGroundPoint.y - player.transform.position.y>=saveNewTeleportPointY)
        {
            _lastGroundPoint = player.transform.position;
        }
        else if (!hitGroundBeneath)
        {
            if (_timeleftToTeleport <= 0)
            {
                Teleport();
            }
        }
        else
        {
            _timeleftToTeleport = fallingTimeUntillTeleport;
        }
    }

    void Teleport()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerGotHit,fallingDamage);
        player.transform.position = _lastGroundPoint;
    }
}
