using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject winTrigger;
    [SerializeField] private GameObject player;
    private Animator _playerAnimator;
    private BeansShooting _beansShootingScript;
    private static readonly int NoGunChange = Animator.StringToHash("noGunChange");

    
    // Start is called before the first frame update
    void Start()
    {
       EventManagerScript.Instance.StartListening(EventManagerScript.PlayerWin, OnPlayerWin); 
       EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, ActivateWinTrigger); 
       _playerAnimator = player.GetComponent<Animator>();
       _beansShootingScript = player.GetComponent<BeansShooting>();
    }

    private void ActivateWinTrigger(object arg0)
    {
        winTrigger.SetActive(true);
    }

    // Update is called once per frame
    void OnPlayerWin(object arg0)
    {
        AudioManager.StopCurrentBGM();
        AudioManager.PlayWinBackground();
        
        _playerAnimator.SetTrigger(NoGunChange);
        _beansShootingScript.canShoot = false;
        
    }
    
    
}
