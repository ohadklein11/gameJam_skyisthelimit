using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float giantFightZoomOut = 76f;
    [SerializeField] private CameraPath cameraPath;
    private float _giantFightZoomOutStartValue;
    [SerializeField] private bool _turnOffCameraPath=false;
    [SerializeField] private float zoomInOnGiant = 30f;
    [SerializeField] private GameObject follow;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform doorPivot;



    void Start()
    {
        _giantFightZoomOutStartValue =
            cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantDoorsOpen, SetGiantBattleCamera);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, SetGiantBattleCamera);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantAttitude, ZoomInOnGiant);

    }

    void Update()
    {
        if (_turnOffCameraPath)
        {
            cameraPath.EndCameraPath();
        }
    }
    public void StartCameraPath()
    {
        cameraPath.StartCameraPath();
    }

    void SetGiantBattleCamera(object arg0)
    {
        float startValue, endValue;
        if (arg0.ToString() == "start")
        {
            startValue = _giantFightZoomOutStartValue;
            endValue= giantFightZoomOut;
        }
        else
        {
            startValue = giantFightZoomOut;
            endValue = _giantFightZoomOutStartValue;
        }
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", startValue,
            "to", endValue,
            "time", 2,
            "onupdate", "UpdateGiantBattleCamera",
            "easetype", iTween.EaseType.linear));
        if (arg0.ToString() == "end")
        {
            
            ZoomInOnDoorOpen();
        }

    }

    void UpdateGiantBattleCamera(float value)
    {
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= value;
    }
    
    void ZoomIn(Transform transformToZoom, float fieldOfView)
    {
        follow.transform.position = player.transform.position;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = follow.transform;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = follow.transform;
        float fieldOfVIewBeforeZoom = cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;

        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= fieldOfView;

        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", transformToZoom.position, "time", 0.5f, "easetype", iTween.EaseType.linear));
        StartCoroutine(StartDelay(2f,fieldOfVIewBeforeZoom));

    }

    public void ZoomInOnDoorOpen()
    {
        float fieldOfView= doorPivot.GetComponentInChildren<BoxCollider2D>().bounds.size.y;
        ZoomIn(doorPivot, fieldOfView);
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
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView=fieldOfVIewBeforeZoom;

        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().LookAt = player.transform;
    }
}
