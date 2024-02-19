using System.Collections;
using System.Collections.Generic;
using Giant;
using UnityEngine;
using UnityEngine.Pool;


public class StonesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject throwable;
    [SerializeField] private float timeToThrow = 2f;
    [SerializeField] private float velocityX = -15f;
    [SerializeField] private float velocityY = -2f;

    private float _timeToThrowLeft;
    private int _throwableObjectAmount = 10;
    
    private GameObject _player;
    private Camera _camera;
    private ObjectPool<GameObject> _throwablePool;


    // Start is called before the first frame update
    void Awake()
    {
        _timeToThrowLeft = timeToThrow;
        _camera = Camera.main;
        _player = GameObject.FindWithTag("Player");

        _throwablePool = new ObjectPool<GameObject>(
            () => Instantiate(throwable, _player.transform.position, Quaternion.identity),
            o =>
            {
                o.SetActive(true);
                float spawnY = _camera.transform.position.y + _camera.orthographicSize +
                               throwable.GetComponent<MeshRenderer>().bounds.size.y;
                float spawnYOffset= Random.Range(spawnY, spawnY+10);

                float spawnX = _camera.transform.position.x + _camera.orthographicSize * _camera.aspect;
                float spawnXOffset= Random.Range(spawnX+10, spawnX+15);
                o.GetComponent<FallingStoneBehaviour>().Init(_throwablePool);
                Vector3 playerPosition = _player.transform.position;
                o.transform.position = new Vector3(spawnXOffset, spawnYOffset, playerPosition.z);
                o.GetComponent<Rigidbody2D>().velocity= new Vector3(velocityX,velocityY,0);
                // o.GetComponentInParent<Rigidbody>().velocity = new Vector3(-10,-2,0);
            }, o =>o.SetActive(false), null,true, _throwableObjectAmount);
        
    }

    void Update()
    {
        if (_timeToThrowLeft<= 0)
        {
            _timeToThrowLeft = timeToThrow;
            _throwablePool.Get();
        }
        else
        {
            _timeToThrowLeft -= Time.deltaTime;
        }
    }
    
}