using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowVineScript : MonoBehaviour
{
    [SerializeField] private float vineGrowthSpeed = 1f;
    private GameObject _vineBodyPrefab;
    private Camera _mainCamera;
    private int _vineCount;
    private float _startY;
    private GameObject _lastStem;

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
    }

    void Update()
    {
        Vector2 newPosition =
            new Vector2(transform.position.x, transform.position.y + vineGrowthSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (_lastStem.transform.position.y > _startY + _vineBodyPrefab.gameObject.transform.localScale.y)
        {
            _lastStem = GrowNewStem();
            _vineCount--;
            if (_vineCount <= 0)
            {
                Destroy(GetComponent<GrowVineScript>());
            }
        }
    }

    int ConfigureVineHeight()
    {
        float cameraHeight = _mainCamera.orthographicSize * 2;
        float vineHeight = _vineBodyPrefab.gameObject.transform.localScale.y;
        int vineCount = (int)(cameraHeight / vineHeight);
        int randomVineCount = Random.Range(vineCount / 2, vineCount);
        return randomVineCount;
    }

    GameObject GrowNewStem()
    {
        GameObject newStem = Instantiate(_vineBodyPrefab, new Vector3(transform.position
            .x, _startY, 0), Quaternion.identity);
        newStem.transform.SetParent(transform.gameObject.transform, false);
        return newStem;
    }
}