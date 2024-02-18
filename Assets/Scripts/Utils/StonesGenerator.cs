using System.Collections;
using System.Collections.Generic;
using Giant;
using UnityEngine;
using UnityEngine.Pool;


public class StonesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject throwable;

    private int _throwableObjectAmount = 10;
    private GameObject _player;
    private Camera _camera;
    private ObjectPool<GameObject> _throwablePool;


    // Start is called before the first frame update
    void Awake()
    {
        _camera = Camera.main;
        _player = GameObject.FindWithTag("Player");
        Vector3 playerPosition = _player.transform.position;

        _throwablePool = new ObjectPool<GameObject>(
            () =>
            {
                for (int i = 0; i < _throwableObjectAmount; i++)
                { 
                    Instantiate(throwable, _player.transform.position, Quaternion.identity);
                }
                return throwable;
            },
            o =>
            {
                o.SetActive(true);
                float spawnY = _camera.transform.position.y + _camera.orthographicSize +
                               throwable.GetComponent<SpriteRenderer>().bounds.size.y;
                o.transform.position = new Vector3(playerPosition.x, spawnY, playerPosition.z);
                o.GetComponent<GiantThrowableBehavior>().Init(_throwablePool, _player);
            }, o => o.SetActive(false));
    }

    // Update is called once per frame
    void Update()
    {
    }
}