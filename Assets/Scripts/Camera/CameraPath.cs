using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPath : MonoBehaviour
{
    private Transform[] _points;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject pointsParent;
    [SerializeField] private GameObject follow;
    [SerializeField] private LayerMask _groundLayerMask;

    [SerializeField] private float speed = 1f;
    private float timeLeft;
    private float _timeLeftToCurrentMovement;
    private int _currentPoint;
    private bool _isFollowingPath;
    private Camera _mainCamera;
    private float _startFiledOfView;

    private void Awake()
    {
        _isFollowingPath = false;
        _points = new Transform[pointsParent.transform.childCount];
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = pointsParent.transform.GetChild(i);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _startFiledOfView= gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        _mainCamera = Camera.main;
        _currentPoint = 0;
    }

    void Update()
    {
        if (_isFollowingPath)
        {
            FollowingCameraPath();
        }
    }

    void FollowingCameraPath()
    {
        follow.transform.position = Vector3.MoveTowards(follow.transform.position, _points[_currentPoint].position,
            speed * Time.deltaTime);
        if (Vector3.Distance(follow.transform.position, _points[_currentPoint].position) < 0.1f)
        {
            _currentPoint++;
        }
        else if (player.transform.position.x > follow.transform.position.x)
        {
            // follow.transform.position = new Vector3(player.transform.position.x, follow.transform.position.y, follow.transform.position.z);

            float cameraHeight = _mainCamera.orthographicSize * 2;
            var hitGround = Physics2D.Raycast(player.transform.position, Vector2.down, cameraHeight, _groundLayerMask);
            float playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            if (hitGround)
            {
                follow.transform.position = new Vector3(player.transform.position.x, hitGround.point.y + playerHeight,
                    player.transform.position.z);
            }
            else
                follow.transform.position = player.transform.position;


            if (player.transform.position.x > _points[_currentPoint].position.x)
            {
                _currentPoint++;
            }
        }

        if (_currentPoint >= _points.Length)
        {
            EndCameraPath();
        }
    }

    public void StartCameraPath()
    {
        _isFollowingPath = true;

        GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
    }

    public void EndCameraPath()
    {
        _isFollowingPath = false;

        GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
    }
    
    
}