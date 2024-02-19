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

    [SerializeField] private float speed = 1f;
    private float timeLeft;
    private float _timeLeftToCurrentMovement;
    private int _currentPoint;
    private bool _isFollowingPath;
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
        follow.transform.position=Vector3.MoveTowards(follow.transform.position, _points[_currentPoint].position, speed*Time.deltaTime);
        if (Vector3.Distance(follow.transform.position, _points[_currentPoint].position) < 0.1f)
        {
            _currentPoint++;
        }
        else if (player.transform.position.x > follow.transform.position.x)
        {
            follow.transform.position = player.transform.position;
            if(player.transform.position.x > _points[_currentPoint].position.x)
            {
                _currentPoint++;
            }
        }
        if (_currentPoint>= _points.Length)
        {
            EndCameraPath();
            follow.transform.position = player.transform.position;
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
}
