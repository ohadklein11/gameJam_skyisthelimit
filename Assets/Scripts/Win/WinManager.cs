using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject winTrigger;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goose;
    [SerializeField] private GameObject playerHouseDoor;
    private Animator _playerAnimator;
    private BeansShooting _beansShootingScript;
    private static readonly int NoGunChange = Animator.StringToHash("noGunChange");
    [SerializeField] private AudioSource doorCloseSound;
    [SerializeField] private AudioSource gooseAudio;


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
        // put goose next to player
        goose.SetActive(true);
        // close the door
        iTween.RotateTo(playerHouseDoor, iTween.Hash("y", 180, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
        doorCloseSound.Play();
        playerHouseDoor.GetComponentInChildren<BoxCollider2D>().enabled = true;
        StartCoroutine(PlayGoose());
    }

    private IEnumerator PlayGoose()
    {
        yield return new WaitForSeconds(1.5f);
        gooseAudio.Play();
    }
}
