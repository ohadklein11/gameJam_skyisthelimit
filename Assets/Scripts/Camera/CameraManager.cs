using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float giantFightZoomOut = 76f;
    [SerializeField] private float afterGiantFightZoom;

    [SerializeField] private CameraPath cameraPath;
    private float _giantFightZoomOutStartValue;
    [SerializeField] private bool _turnOffCameraPath;
    [SerializeField] private float zoomInOnGiant = 30f;
    [SerializeField] private GameObject follow;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform doorPivot;
    [SerializeField] private StartCameraPath startCameraPath;
    [SerializeField] private GameObject gooseCage;
    private bool _firstOpen = true;
    [SerializeField] private ShaderManager shaderManager;
    [SerializeField] private CinemachineConfiner2D bossConfiner;
    private CinemachineFramingTransposer _transposer;


    void Start()
    {
        bossConfiner.enabled = false;
        startCameraPath.SetActivePath(_turnOffCameraPath);
        _giantFightZoomOutStartValue =
            cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantDoorsOpen, SetGiantBattleCamera);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, ZoomInOnDoorOpen);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantAttitude, ZoomInOnGiant);
        EventManagerScript.Instance.StartListening(EventManagerScript.LeavingGiantTemple, SetGiantBattleCamera);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerWin, OnPlayerWin);
        _transposer= cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void OnPlayerWin(object arg0)
    {
        // tween the VirtualCameraCinemachine's filef of view to 32
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView,
            "to", 30f,
            "time", 2,
            "onupdate", "UpdateGiantBattleCamera",
            "easetype", iTween.EaseType.easeOutSine));
    }


    public void StartCameraPath()
    {
        cameraPath.StartCameraPath();
    }

    void SetGiantBattleCamera(object arg0)
    {
        StartCoroutine(SetGiantBattleCameraCoroutine(arg0));
    }

    private IEnumerator SetGiantBattleCameraCoroutine(object arg0)
    {
        if (_firstOpen)
        {
            ZoomInOnGoose();
            yield return new WaitForSeconds(5f);
            shaderManager.OriginalToCold(0f);
        }
        float startValue, endValue;
        if (arg0.ToString() == "start")
        {
            startValue = _giantFightZoomOutStartValue;
            endValue= giantFightZoomOut;
            // StartCoroutine(EnableCinfiner());
            bossConfiner.m_Damping = 5f;
            bossConfiner.enabled = true;
            // iTween.ValueTo(gameObject, iTween.Hash(
            //     "from", 1.53f,
            //     "to", 3.85f,
            //     "time", 2,
            //     "onupdate", "UpdateTrackedObjectOffset",
            //     "easetype", iTween.EaseType.easeInOutSine));
            
        }
        else
        {
            startValue = giantFightZoomOut;
            endValue = afterGiantFightZoom;
            bossConfiner.enabled = false;
            bossConfiner.m_Damping = 0;
            cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0,0,0);
        }

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", startValue,
            "to", endValue,
            "time", 2,
            "onupdate", "UpdateGiantBattleCamera",
            "easetype", iTween.EaseType.easeInOutSine));
        _firstOpen = false;
    }

    IEnumerator EnableCinfiner()
    {
        yield return new WaitForSeconds(1.8f);
        cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().enabled = true;
        cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().m_Damping = 5f;
    }
    void UpdateGiantBattleCamera(float value)
    {
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= value;
    }
    void UpdateTrackedObjectOffset(float value)
    {
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(-0.4f,value,0);
    }
    
    void ZoomIn(Transform transformToZoom, float fieldOfView, float time=1f, float delay=1f)
    {
        follow.transform.position = player.transform.position;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
        float fieldOfVIewBeforeZoom = cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;

        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= fieldOfView;

        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", transformToZoom.position, "time", time, "easetype", iTween.EaseType.easeInOutSine));
        StartCoroutine(StartDelay(time + delay,fieldOfVIewBeforeZoom));

    }

    void ZoomInOnDoorOpen(object arg0)
    {
        float fieldOfView= 40f;
        ZoomIn(doorPivot, fieldOfView);
    }
    
    void ZoomInOnGiant(object arg0)
    {
        GameObject giant = (GameObject)arg0;
        // float fieldOfView= giant.GetComponent<PolygonCollider2D>().bounds.size.y;

        ZoomIn(giant.transform,zoomInOnGiant);
    }
    
    private void ZoomInOnGoose()
    {
        ZoomIn(gooseCage.transform, 30f, 3f, 2f);
    }

    
    IEnumerator StartDelay(float delay,float fieldOfVIewBeforeZoom)
    {
        yield return new WaitForSeconds(delay);
        ZoomOnPlayer(fieldOfVIewBeforeZoom);
    }

    void ZoomOnPlayer(float fieldOfVIewBeforeZoom)
    {
        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", player.transform.position, "time", 1f, "easetype", iTween.EaseType.easeInOutSine));
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView=fieldOfVIewBeforeZoom;

        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
    }

    private void FixedUpdate()
    {
        if (player.transform.position.x < -70)
        {
            float maxOffset = -72.56f + 70f;
            float currOffset= player.transform.position.x+70f;
            float offset = 0.21f;
            _transposer.m_ScreenX=Mathf.Min(0.5f, 0.5f - Mathf.Min(offset, (currOffset / maxOffset)*offset));
        }
        else
        {
            _transposer.m_ScreenX= 0.5f;
    
        }
    }
}
