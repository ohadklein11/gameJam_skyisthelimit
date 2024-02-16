using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNotAroundCamera : MonoBehaviour
{
    private Camera _mainCamera;
    void Awake()
    {
        _mainCamera = Camera.main;
    }
    
    void Update()
    {

        // if camera is 1.5 screens on the left of the bean, destroy the bean
        if (transform.position.x < _mainCamera.transform.position.x - _mainCamera.orthographicSize * _mainCamera.aspect * 1.5f)
        {
            Destroy(gameObject);
        }
        // if camera is 1.5 screens on the right of the bean, destroy the bean
        if (transform.position.x > _mainCamera.transform.position.x + _mainCamera.orthographicSize * _mainCamera.aspect * 1.5f)
        {
            Destroy(gameObject);
        }
        // if camera is 1.5 screens above the bean, destroy the bean
        if (transform.position.y > _mainCamera.transform.position.y + _mainCamera.orthographicSize * 1.5f)
        {
            Destroy(gameObject);
        }

        
        
    }
}
