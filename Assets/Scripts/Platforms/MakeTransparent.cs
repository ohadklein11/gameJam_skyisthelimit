using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private float fadeTime = 0.5f;
    
    private Material _material;
    private bool _isFaded = false;
    // private Material _defaultMaterial;
    [SerializeField] private Material transparentMaterial;

    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.transform.position;
        var cameraPos = _mainCamera.transform.position;
        // if this object is blocks the player, make it transparent
        var hit = Physics.Raycast(cameraPos, (playerPos-cameraPos).normalized, Vector3.Distance(playerPos,cameraPos), LayerMask.GetMask("Default"));
        if (hit)
        {
            // make object's surface type to transparent
            
            StartFade();
        }
        else
        {
            ResetFade();
        }
    }

    void StartFade()
    {
        Color color = _material.color;
        Color transColor = new Color(color.r, color.g, color.b, Mathf.Lerp( color.a, 0.5f, fadeTime*Time.deltaTime));
        _material.color = transColor;
    }
    
    void ResetFade()
    {
        Color color = _material.color;
        Color transColor = new Color(color.r, color.g, color.b, Mathf.Lerp( color.a, 1, fadeTime*Time.deltaTime));
        _material.color = transColor;
    }
}
