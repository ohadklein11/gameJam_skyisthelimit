using System;
using DG.Tweening;
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
    [SerializeField] private Transform player;

    [SerializeField] private float yBottomLimitCold;
    [SerializeField] private float yTopLimit;
    [SerializeField] private float yBottomLimitWarm;
    
    private Color _originalLightFilter;
    private float _originalLightTemperature;
    private float _originalLightIntensity;
    private Color _originalCameraBackground;
    private Color _originalFog;
    private float _originalFogDensity;
    private bool _cold;
    private bool _warm;
    
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
        if (!GameData.Instance.openedGiantDoors && !GameData.Instance.isGiantFightOver)
        {  // COLD
            // lerp between original at the bottom to cold at the top
            var t = (player.transform.position.y - yBottomLimitCold) / (yTopLimit - yBottomLimitCold);
            if (t<0) t = 0;
            if (t>1) t = 1;
            directionalLight.color = Color.Lerp(_originalLightFilter, coldLightFilter, t);
            directionalLight.colorTemperature = Mathf.Lerp(_originalLightTemperature, coldLightTemperature, t);
            directionalLight.intensity = Mathf.Lerp(_originalLightIntensity, coldLightIntensity, t);
            mainCamera.backgroundColor = Color.Lerp(_originalCameraBackground, coldCameraBackground, t);
            RenderSettings.fogColor = Color.Lerp(_originalFog, coldFog, t);
            _cold = true;
        } else if (GameData.Instance.IsGiantFight && _cold)
        {
            ColdToOriginal(1f);
            _cold = false;
        } else if (GameData.Instance.escaping && !_warm)
        {  // WARM
            directionalLight.DOColor(warmLightFilter, 3f);
            DOTween.To(() => directionalLight.colorTemperature, x => directionalLight.colorTemperature = x, warmLightTemperature, 3f);
            directionalLight.DOIntensity(warmLightIntensity, 3f);
            mainCamera.DOColor(warmCameraBackground, 3f);
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, warmFog, 3f);
            DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, warmFogDensity, 3f).OnComplete(() => _warm = true);
        } else if (_warm)
        {
            if (GameData.Instance.escaping)
            {
                // lerp between warm at the bottom to original at the top
                var t = 1 - (player.transform.position.y - yBottomLimitWarm) / (yTopLimit - yBottomLimitWarm);
                if (t < 0) t = 0;
                if (t > 1) t = 1;
                RenderSettings.fogDensity = Mathf.Lerp(warmFogDensity, _originalFogDensity, t);
            }
            else
            {  // home - win
                directionalLight.DOColor(_originalLightFilter, 2f);
                DOTween.To(() => directionalLight.colorTemperature, x => directionalLight.colorTemperature = x, _originalLightTemperature, 2f);
                directionalLight.DOIntensity(_originalLightIntensity, 2f);
                mainCamera.DOColor(_originalCameraBackground, 2f);
                DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, _originalFog, 2f);
                DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, _originalFogDensity, 2f);
                _warm = false;
            }
        }
    }

    public void ColdToOriginal(float time)
    {
        directionalLight.DOColor(_originalLightFilter, time);
        DOTween.To(() => directionalLight.colorTemperature, x => directionalLight.colorTemperature = x, _originalLightTemperature, time);
        directionalLight.DOIntensity(_originalLightIntensity, time);
        mainCamera.DOColor(_originalCameraBackground, time);
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, _originalFog, time);
    }
    
    public void OriginalToCold(float time)
    {
        directionalLight.DOColor(coldLightFilter, time);
        DOTween.To(() => directionalLight.colorTemperature, x => directionalLight.colorTemperature = x, coldLightTemperature, time);
        directionalLight.DOIntensity(coldLightIntensity, time);
        mainCamera.DOColor(coldCameraBackground, time);
        DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, coldFog, time);
    }
}
