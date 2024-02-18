using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _shakeIntensity;
    
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        _shakeTimer = time;
        _shakeTimerTotal = time;
        _shakeIntensity= intensity;
    }
    
    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
                Mathf.Lerp(_shakeIntensity,0,1-(_shakeTimer/_shakeTimerTotal));
            }
        }
    }
}
