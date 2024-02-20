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
    [SerializeField] private Transform _doorPivot;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float zoomInOnGiant = 30f;
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
        // if (player.transform.position.x > follow.transform.position.x)
        // {
        //     // iTween.StopByName("anime"+(_currentPoint-1));
        //     follow.transform.position = player.transform.position;
        //     timeLeft = 0;
        // }
        //
        // if (timeLeft <= 0)
        // {
        //     if (_currentPoint < _points.Length - 1)
        //     {
        //         Debug.Log("Start itween fljdf");
        //         // iTween.MoveTo(follow, iTween.Hash("position", _points[_currentPoint+1], "time", time, "easetype", iTween.EaseType.linear,"name","anime"+_currentPoint));
        //         _currentPoint++;
        //         timeLeft = time;
        //     }
        // }
        // else
        // {
        //     timeLeft -= Time.deltaTime;
        // }
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

        Debug.Log("Start of Camera path");
        GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
    }

    public void EndCameraPath()
    {
        _isFollowingPath = false;

        Debug.Log("End of Camera path");
        GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
    }
    
    public void ZoomIn(Transform transformToZoom, float fieldOfView)
    {
        follow.transform.position = player.transform.position;
        GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
        float fieldOfVIewBeforeZoom = gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;

        gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= fieldOfView;

        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", transformToZoom.position, "time", 0.5f, "easetype", iTween.EaseType.linear));
        StartCoroutine(StartDelay(2f,fieldOfVIewBeforeZoom));

    }

    public void ZoomInOnDoorOpen()
    {
        float fieldOfView= _doorPivot.GetComponentInChildren<BoxCollider2D>().bounds.size.y;
        ZoomIn(_doorPivot, fieldOfView);
    }
    
    public void ZoomInOnGiant(object arg0)
    {
        GameObject giant = (GameObject)arg0;
        // float fieldOfView= giant.GetComponent<PolygonCollider2D>().bounds.size.y;

        ZoomIn(giant.transform,zoomInOnGiant);
    }
    
    IEnumerator StartDelay(float delay,float fieldOfVIewBeforeZoom)
    {
        yield return new WaitForSeconds(delay);
        ZoomOnPlayer(fieldOfVIewBeforeZoom);
    }

    void ZoomOnPlayer(float fieldOfVIewBeforeZoom)
    {
        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", player.transform.position, "time", 0.5f, "easetype", iTween.EaseType.linear));
        gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView=fieldOfVIewBeforeZoom;

        GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
    }
}