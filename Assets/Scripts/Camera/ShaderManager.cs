using System;
using UnityEngine;
using Utils;

public class ShaderManager : MonoBehaviour
{
    [SerializeField] private Color coldLightFilter;
    [SerializeField] private float coldLightTemperature;
    [SerializeField] private float coldLightIntensity;
    [SerializeField] private Color coldCameraBackground;
    [SerializeField] private Color coldFog;
    
    [SerializeField] private Color warmLightFilter;
    [SerializeField] private float warmLightTemperature;
    [SerializeField] private float warmLightIntensity;
    [SerializeField] private Color warmCameraBackground;
    [SerializeField] private Color warmFog;
    [SerializeField] private float warmFogDensity;
    
    [SerializeField] private Light directionalLight;
    [SerializeField] private Camera mainCamera;
    
    private Color _originalLightFilter;
    private float _originalLightTemperature;
    private float _originalLightIntensity;
    private Color _originalCameraBackground;
    private Color _originalFog;
    private float _originalFogDensity;
    
    private void Start()
    {
        _originalLightFilter = directionalLight.color;
        _originalLightTemperature = directionalLight.colorTemperature;
        _originalLightIntensity = directionalLight.intensity;
        _originalCameraBackground = mainCamera.backgroundColor;
        _originalFog = RenderSettings.fogColor;
        _originalFogDensity = RenderSettings.fogDensity;
    }

    private void Update()
    {
        if (!GameData.Instance.IsGiantFight && !GameData.Instance.isGiantFightOver)
        {  // COLD
            return;
        }
    }
}
