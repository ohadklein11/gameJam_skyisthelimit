using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToStartPoint : MonoBehaviour
{
    [SerializeField] private Transform _playerStartPoint;
    [SerializeField] private GameObject _player;

    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd,DeActivateCollider);
    }

    void DeActivateCollider(object arg0)
    {
        GetComponent<Collider2D>().enabled = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.transform.position = _playerStartPoint.position;
        }
    }
}
