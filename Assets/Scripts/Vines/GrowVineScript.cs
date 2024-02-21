using UnityEngine;
using Utils;

public class GrowVineScript : MonoBehaviour
{
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
    private bool _hitCeiling = false;
    private const float Epsilon = .05f;

    void Awake()
    {
        _vineBodyPrefab = Resources.Load<GameObject>("Prefabs/Vines/VineBody");
        _mainCamera = Camera.main;
        _vineCount = ConfigureVineHeight() - 1;
        _startY = transform.position.y;
        Debug.Log("vine created");
    }

    void Start()
    {
        _lastStem = transform.GetChild(0).gameObject;
        _headHeight = _lastStem.GetComponent<SpriteRenderer>().bounds.size.y;
        _stemHeight = _vineBodyPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        if (_hitCeiling)
        {
            return;
        }
        
        // check if vine has hit the ceiling
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, .3f, LayerMask.GetMask("Ceiling"));
        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ceiling"))
        {
            _hitCeiling = true;
            return;
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
                Destroy(GetComponent<GrowVineScript>());
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
        float cameraHeight = _mainCamera.orthographicSize * 2;
        float vineHeight = _vineBodyPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        int vineCount = (int)(cameraHeight / vineHeight);
        int randomVineCount = Random.Range(vineCount / 2, vineCount);
        return randomVineCount;
        
        
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
}