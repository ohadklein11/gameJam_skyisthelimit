using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using Giant;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class GiantFightManager : Singleton<MonoBehaviour>
{
    [SerializeField] private GameObject _door;
    [SerializeField] private Transform _doorPivot;
    [SerializeField] private Collider2D _doorCollider;
    [SerializeField] private GiantBehavior giantBehavior;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _openGiantDoorsTrigger;
    [SerializeField] private GameObject _giantFightTrigger;
    [SerializeField] private Collider2D _chairCollider;  // to disable after taking goose
    [SerializeField] private GameObject _stonesTrigger;
    [SerializeField] private BezierPath _pathToGoose;
    [SerializeField] private BezierPath _pathFromGoose;
    [SerializeField] private GameObject _startEscapeTrigger;
    [SerializeField] private SpriteRenderer _giantSpriteRenderer;
    [SerializeField] private BeansShooting _beansShooting;
    [SerializeField] private GameObject _blockExit;
    [SerializeField] private GameObject EButton;
    [SerializeField] private GameObject backwardEnemies;
    [SerializeField] private GooseInCageBehavior _gooseInCage;
    [SerializeField] private AudioSource doorOpenSound;
    [SerializeField] private AudioSource doorCloseSound;
    [SerializeField] private CameraShake cameraShake;
    private bool _escaping;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // todo temp to teleport to giant doors
        {
            _player.transform.position = _openGiantDoorsTrigger.transform.position + new Vector3(-2, 1, 0);
        }

        if (_escaping)
        {
            var intensity = 3 - Mathf.Abs(_player.transform.position.x - giantBehavior.gameObject.transform.position.x) / 15f;
            if (intensity > 0)
            {
                cameraShake.Shake(intensity, 0.1f);
            }
         }
    }

    private void OpenDoor()
    {
        iTween.RotateTo(_doorPivot.gameObject, iTween.Hash("y", 180, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
        _doorCollider.enabled = false;
        _blockExit.SetActive(!_blockExit.activeSelf);
        var volume = Mathf.Abs(_player.transform.position.x - _doorPivot.position.x) < 10 ? 1 : 0.5f;
        doorOpenSound.volume = volume;
        doorOpenSound.Play();
    }
    
    private void CloseDoor()
    {
        iTween.RotateTo(_doorPivot.gameObject, iTween.Hash("y", -90, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
        _doorCollider.enabled = true;
        var volume = Mathf.Abs(_player.transform.position.x - _doorPivot.position.x) < 10 ? 1 : 0.5f;
        doorCloseSound.volume = volume;
        doorCloseSound.Play();
    }

    public void StartGiantFight()
    {
        CloseDoor();
        StartCoroutine(giantBehavior.StartGiantFight());
    }
    
    public void EndGiantFight()
    {
        giantBehavior.EndGiantFight();
        _beansShooting.canShoot = false;
        _pathToGoose.gameObject.SetActive(true);
        GameData.Instance.EndGiantFight();
        AudioManager.StopCurrentBGM();
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.GiantAttitude,_giantSpriteRenderer.gameObject);
    }

    public void OpenGiantDoors()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.GiantDoorsOpen,"start");
        _player.GetComponent<PlayerMovement>().StopMoving(5f);
        AudioManager.StopCurrentBGM();
        AudioManager.PlayBossBackground();
        _gooseInCage.audioGoose.Play(44100*3);
        OpenDoor();
    }

    public void TookGoose()
    {
        _chairCollider.enabled = false;
        _pathToGoose.gameObject.SetActive(false);
        _pathFromGoose.gameObject.SetActive(true);
        _startEscapeTrigger.SetActive(true);
        _beansShooting.SetGunType(GunType.EggsGun);
        _player.GetComponent<PlayerAnimation>().SwitchToGooseAnimation();
    }
    
    public void StartEscape()
    {
        giantBehavior.StartEscape();
        _chairCollider.enabled = true;
        _pathFromGoose.gameObject.SetActive(false);
        _stonesTrigger.SetActive(true);
        _beansShooting.canShoot = true;
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.GiantFightEnd,null);
        StartCoroutine(OpenGiantDoorsDelay());
        EButton.SetActive(true);
        EButton.GetComponent<SpriteRenderer>().DOFade(1, 1f);
        backwardEnemies.SetActive(true);
        AudioManager.PlayDownBackground();
        _escaping = true;
    }
    
    IEnumerator OpenGiantDoorsDelay()
    {
        yield return new WaitForSeconds(1f);
        OpenDoor();
    }

    public void HitGiant()
    {
        giantBehavior.HitGiant();
    }
}
