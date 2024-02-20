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

    void ZoomInOnDoorOpen()
    {
        cameraPath.ZoomInOnDoorOpen();
    }
    
    void ZoomInOnGiant(object arg0)
    {
        cameraPath.ZoomInOnGiant(arg0);
    }


    void UpdateGiantBattleCamera(float value)
    {
        cameraPath.gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView= value;
    }
}
