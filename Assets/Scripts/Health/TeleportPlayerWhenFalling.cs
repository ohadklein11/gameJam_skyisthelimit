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
    [SerializeField] private GameObject bossPlatform;
    [SerializeField] private GameObject playerHome;
    [SerializeField] private float fallingTimeUntillTeleport = 0.5f;
    private GameData _gameData;
    private PlayerMovement _playerMovement;
    private Vector3 _lastGroundPoint;
    private SpriteRenderer _playerSpriteRenderer;
    private bool _hitSolidGround;
    private GameObject _lastGround;
    private GameObject _currGround;
    private Collider2D _hit;

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
        var hit=CheckGroundBeneath(0.6f);

        _hitSolidGround = _playerMovement.grounded && !_playerMovement.falling && !_playerMovement.climbing && hit;
        // _hit =Physics2D.OverlapCircle(_playerMovement.groundCheck.position, _playerMovement.groundCheckRadius, LayerMask.GetMask("Ground"));
        // if (_hit)
        // {
        //     _currGround = _hit.gameObject;
        // }
        if (_gameData.isGiantFightOver)
            teleportOnDescend();
        else
            teleportOnAscend();
        // _lastGround = _currGround;
    }
    
    private bool CheckGroundBeneath(float distance)
    {
        Vector3 playerPos = player.transform.position;
        Vector3 spriteExtents = _playerSpriteRenderer.bounds.extents;
        var raycastOriginLeft =
            playerPos + new Vector3( -spriteExtents.x, -spriteExtents.y - .1f, 0);
        var raycastOriginRight =
            playerPos + new Vector3( spriteExtents.x, -spriteExtents.y - .1f, 0);
        var raycastOriginCenter =
            playerPos + new Vector3(0, -spriteExtents.y - .1f, 0);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.down, distance, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOriginRight, Vector2.down, distance, LayerMask.GetMask("Ground"));
        RaycastHit2D hitCenter = Physics2D.Raycast(raycastOriginCenter, Vector2.down, distance, LayerMask.GetMask("Ground"));
        return hitLeft && hitRight && hitCenter;
    }

    private void teleportOnAscend()
    {
        if (_hitSolidGround && Vector3.Distance(bossPlatform.transform.position,player.transform.position)<Vector3.Distance(bossPlatform.transform.position,_lastGroundPoint))
        {
            _lastGroundPoint = player.transform.position + new Vector3(0,1f,0);
            Debug.Log("GroundPointOnAscend"+_lastGroundPoint);

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
        var hitGroundBeneath = CheckGroundBeneath(10f);
        if (_hitSolidGround && Vector3.Distance(playerHome.transform.position,player.transform.position)<Vector3.Distance(playerHome.transform.position,_lastGroundPoint))
        {
            _lastGroundPoint = player.transform.position+new Vector3(0,1f,0);
            Debug.Log("GroundPointOnDescend"+_lastGroundPoint);  //too many logs

        }
        if (!hitGroundBeneath && _playerMovement.falling && !_playerMovement.jumping)
        {
            Debug.Log("no ground below and falling");
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
