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
    
    



    void Start()
    {
        cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().enabled = false;
        startCameraPath.SetActivePath(_turnOffCameraPath);
        _giantFightZoomOutStartValue =
            cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantDoorsOpen, SetGiantBattleCamera);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, ZoomInOnDoorOpen);
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantAttitude, ZoomInOnGiant);
        EventManagerScript.Instance.StartListening(EventManagerScript.LeavingGiantTemple, SetGiantBattleCamera);


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
            cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().enabled = true;
            cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().m_Damping = 5f;
        }
        else
        {
            
            startValue = giantFightZoomOut;
            endValue = afterGiantFightZoom;
            cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().m_Damping = 0;
            cameraPath.gameObject.GetComponent<CinemachineConfiner2D>().enabled = false;
            cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0,0,0);
            
            
            
        }
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", startValue,
            "to", endValue,
            "time", 2,
            "onupdate", "UpdateGiantBattleCamera",
            "easetype", iTween.EaseType.easeInOutSine));

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

        iTween.MoveTo(follow.transform.gameObject, iTween.Hash("position", transformToZoom.position, "time", 1f, "easetype", iTween.EaseType.easeInOutSine));
        StartCoroutine(StartDelay(2f,fieldOfVIewBeforeZoom));

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
}
