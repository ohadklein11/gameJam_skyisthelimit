using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeansShooting : MonoBehaviour
{
    // [SerializeField] private float aimDistance = 20f;
    //
    // private Vector3 _faceRightPosition = new Vector3(-0.345892f, -0.250198f, 0);
    // private Quaternion _faceRightRotation = new Quaternion(0,0,-66f,0);
    // private Vector3 _faceLeftPosition = new Vector3(0.4262353f, -0.1692467f, 0);
    // private Quaternion _faceLeftRotation = new Quaternion(0,0,64,0);
    // [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    // private Transform _armTransform;

    void Awake()
    {
        // float w = transform.parent.transform.rotation.w;
        // _faceRightRotation.w= w;
        // _faceLeftRotation.w= w;
        // _armTransform = transform.parent.transform;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (_playerSpriteRenderer.flipX)
        // {
        //     Debug.Log("Facing left");
        //     _armTransform.position= _faceLeftPosition;
        //     _armTransform.rotation= _faceLeftRotation;
        //
        // }
        // else{
        //     Debug.Log("Facing Right");
        //
        //     _armTransform.position= _faceRightPosition;
        //     _armTransform.rotation= _faceRightRotation;
        // }
    }
}
