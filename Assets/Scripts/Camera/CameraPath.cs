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

    [SerializeField] private float time = 1f;
    private float timeLeft;
    private float _timeLeftToCurrentMovement;
    private int _currentPoint;
    private void Awake()
    {
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
        StartCameraPath();
    }

    void Update()
    {
        if (player.transform.position.x > follow.transform.position.x)
        {
            iTween.StopByName("anime"+(_currentPoint-1));
            follow.transform.position = player.transform.position;
            timeLeft = 0;
        }
        
        if (timeLeft <= 0)
        {
            if (_currentPoint < _points.Length - 1)
            {
                Debug.Log("Start itween fljdf");
                iTween.MoveTo(follow, iTween.Hash("position", _points[_currentPoint+1], "time", time, "easetype", iTween.EaseType.linear,"name","anime"+_currentPoint));
                _currentPoint++;
                timeLeft = time;
            }
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public void StartCameraPath()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
    }
}
