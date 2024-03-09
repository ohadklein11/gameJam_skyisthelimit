using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Animator _playerAnimator;
    private BeansShooting _beansShootingScript;
    void Start()
    {
        _playerAnimator = player.GetComponent<Animator>();
        _beansShootingScript = player.GetComponent<BeansShooting>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerAnimator.SetTrigger("noGunChange");
            _beansShootingScript.canShoot = false;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerWin,null);
        }
    }
}
