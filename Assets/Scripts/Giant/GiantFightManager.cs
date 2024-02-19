using Giant;
using UnityEngine;
using UnityEngine.Serialization;

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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // temp to teleport to giant doors
        {
            _player.transform.position = _openGiantDoorsTrigger.transform.position + new Vector3(-2, 1, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.D)) // temp to teleport to end of temple
        {
            _player.transform.position = _stonesTrigger.transform.position + new Vector3(2, 1, 0);
        }
    }

    private void OpenDoor()
    {

        iTween.RotateTo(_doorPivot.gameObject, iTween.Hash("y", 180, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
        _doorCollider.enabled = false;
    }
    
    private void CloseDoor()
    {
        iTween.RotateTo(_doorPivot.gameObject, iTween.Hash("y", 90, "time", 2, "easetype", iTween.EaseType.easeOutCubic));
        _doorCollider.enabled = true;
    }

    public void StartGiantFight()
    {
        CloseDoor();
        giantBehavior.StartGiantFight();
    }
    
    public void EndGiantFight()
    {

        _giantSpriteRenderer.color = Color.green;  // temp
        _pathToGoose.gameObject.SetActive(true);
        _beansShooting.canShoot = false;
    }

    public void OpenGiantDoors()
    {
        
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.GiantDoorsOpen,"start");
        OpenDoor();
    }

    public void TookGoose()
    {
        _chairCollider.enabled = false;
        _pathToGoose.gameObject.SetActive(false);
        _pathFromGoose.gameObject.SetActive(true);
        _startEscapeTrigger.SetActive(true);
        _beansShooting.SetGunType(GunType.EggsGun);
    }
    
    public void StartEscape()
    {
        EventManagerScript.Instance.TriggerEvent(EventManagerScript.GiantFightEnd,"end");
        _chairCollider.enabled = true;
        _pathFromGoose.gameObject.SetActive(false);
        _giantSpriteRenderer.color = Color.red;  // temp
        OpenDoor();
        _stonesTrigger.SetActive(true);
        _beansShooting.canShoot = true;
    }
}
