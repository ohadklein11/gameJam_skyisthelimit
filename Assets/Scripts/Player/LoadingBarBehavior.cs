using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarBehavior : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private Image loadingBarOutSide;
    [SerializeField] private Color shootingColor;
    [SerializeField] private Color cooldownColor;
    [SerializeField] private BeansShooting beansShooting;
    
    private bool _fullCharge = false;
    private int _direction = 1;
    private TweenerCore<Color,Color,ColorOptions> tween;

    void Update()
    {
        Vector3 localScale;
        var transform1 = transform;
        if ((int)Mathf.Sign(transform1.parent.localScale.x) != (int)Mathf.Sign(transform1.localScale.x))
        {
            var scale = transform1.localScale;
            localScale = new Vector3(-scale.x, (localScale = scale).y, localScale.z);
            scale = localScale;
            transform1.localScale = scale;
        }
        
        float percentage = 0f;
        if (beansShooting.isLoading)
        {
            percentage = beansShooting.GetShootingForcePercentage();
            loadingBar.color = shootingColor;
            
        } else if (beansShooting.IsOnCooldown)
        {
            percentage = beansShooting.GetCooldownPercentage();
            loadingBar.color = cooldownColor;
        }
        loadingBar.fillAmount = percentage;
        if (percentage >= 1f)
        {
            if (!_fullCharge)
            {
                _fullCharge = true;
                tween = loadingBarOutSide.DOColor(loadingBar.color, 0.3f).SetEase(Ease.InOutBounce);
            }
        }
        else
        {
            tween?.Kill();
            _fullCharge = false;
            loadingBarOutSide.color = Color.clear;
        }
    }
}
