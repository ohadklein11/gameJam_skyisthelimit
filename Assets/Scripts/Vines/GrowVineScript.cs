using System;
using Player;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GrowVineScript : MonoBehaviour
{
    private static bool _firstVine = true;
    [SerializeField] private float vineGrowthSpeed = 1f;
    private GameObject _vineBodyPrefab;
    private Camera _mainCamera;
    private int _vineCount;
    private float _startY;
    private GameObject _lastStem;
    private float _headHeight;
    private float _stemHeight;
    private bool firstGrow = true;
    [SerializeField] private int giantStemCount;
    [SerializeField] private int regularStemCount;
    private bool _hitCeiling = false;
    private bool _growing;
    public bool growing => _growing;
    private const float Epsilon = .05f;
    

    void Awake()
    {
        _vineBodyPrefab = Resources.Load<GameObject>("Prefabs/Vines/VineBody");
        _mainCamera = Camera.main;
        _vineCount = ConfigureVineHeight() - 1;
        _startY = transform.position.y;
    }

    void Start()
    {
        _lastStem = transform.GetChild(0).gameObject;
        _headHeight = _lastStem.GetComponent<SpriteRenderer>().bounds.size.y;
        _stemHeight = _vineBodyPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        _growing = true;
        if (_firstVine)
        {
            _firstVine = false;
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.FirstVine, null);
        }
    }

    void Update()
    {
        if (!_growing || _hitCeiling)
        {
            return;
        }
        
        // check if vine has hit the ceiling
        if (!firstGrow)
        {
            var hit = Physics2D.Raycast(transform.position+new Vector3(0,.3f,0),Vector3.up,.3f,LayerMask.GetMask("Ceiling", "Ground"));
            if (hit)
            {
                _hitCeiling = true;
                return;
            }
        }

        Vector3 newPosition =
            new Vector3(transform.position.x, transform.position.y + vineGrowthSpeed * Time.deltaTime, transform.position.z);
        transform.position = newPosition;

        var curHeight = firstGrow ? _headHeight : _stemHeight;
        if (_lastStem.transform.position.y >= _startY + curHeight - Epsilon)
        {
            _vineCount--;
            if (_vineCount < 0)
            {
                _growing = false;
                return;
            }
            _lastStem = GrowNewStem(_lastStem.transform.position.y - curHeight/2);
            firstGrow = false;
        }
    }

    int ConfigureVineHeight()
    {
        if (GameData.Instance.IsGiantFight)
        {
            return giantStemCount;
        }
        else
        {
            int ran = Random.Range(0, 2);
            if (ran < 1)
                return regularStemCount - 1;
            return regularStemCount;
        }
    }

    GameObject GrowNewStem(float yTop)
    {
        var yPos = yTop - _stemHeight / 2;
        var position = transform.position;
        GameObject newStem = Instantiate(_vineBodyPrefab, 
            new Vector3(position.x, yPos, position.z), Quaternion.identity);
        newStem.transform.SetParent(transform.gameObject.transform, true);
        return newStem;
    }

    private void OnDestroy()
    {
        // remove player from children
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                var gameObj = child.gameObject;
                gameObj.GetComponent<PlayerMovement>().enabled = true;
                gameObj.GetComponent<BeansShooting>().enabled = true;
                gameObj.GetComponent<Collider2D>().enabled = true;
                gameObj.GetComponent<Animator>().enabled = true;
                gameObj.GetComponent<PlayerAnimation>().enabled = true;
                child.GetComponent<PlayerAnimation>().SwitchToClimbingAnimation(false);

                child.SetParent(null);
            }
        }
    }
}