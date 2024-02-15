using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public bool Grounded { get; private set; }
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _VineLayerMask;
    private GameObject _vineHeadPrefab;
    

    private Camera _mainCamera;

    [SerializeField] private float distanceToUpperPlatform = 20f;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Grounded = false;
        _mainCamera = Camera.main;
        _vineHeadPrefab= Resources.Load<GameObject>("Prefabs/Vines/VineHead");
    }

    void Update()
    {
        CheckGrounded();
    }
    
    private void CheckGrounded()
    {
        Vector3 position = transform.position;
        Vector3 localScale = transform.localScale;
        float distance = .4f * localScale.y;
        Bounds spriteBounds = _spriteRenderer.bounds;
        Vector3 BeanBottomCenter = position - new Vector3(0, (spriteBounds.max.y - spriteBounds.min.y) / 2, 0);
        RaycastHit2D hit = Physics2D.Raycast(BeanBottomCenter, Vector2.down, distance, _groundLayerMask);
        if (hit&& !Grounded)
        {
            GrowVine(hit);
            Grounded = true;
        }
    }
    
    private void GrowVine(RaycastHit2D bottomPlatform)
    {
        float vineWidth = _vineHeadPrefab.gameObject.transform.localScale.x;

        Vector3 position = transform.position + new Vector3(0,transform.localScale.y,0);
        // var hitUpperPlatform = Physics2D.Raycast(position, Vector2.up, distanceToUpperPlatform, _groundLayerMask);
        var hitLeft = Physics2D.Raycast(position, Vector2.left, vineWidth*0.67f, _VineLayerMask);
        var hitRight = Physics2D.Raycast(position, Vector2.right, vineWidth*0.67f, _VineLayerMask);

        if (!hitLeft && !hitRight)
        {
            // GameObject vine=Instantiate(_vinePrefab, new Vector3(transform.position
            //     .x,vineHeight+growthPositionY,0), Quaternion.identity);
            GameObject vine = BuildVine(bottomPlatform);
            vine.transform.SetParent(bottomPlatform.transform,true);

            // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Vine Grown");

        }
        Destroy(gameObject);

    }
    GameObject BuildVine(RaycastHit2D bottomPlatform)
    {
        float growthPositionY=bottomPlatform.transform.position.y+bottomPlatform.transform.localScale.y/2;
        float vineHeadHeight = _vineHeadPrefab.gameObject.transform.localScale.y/2;
        
        return Instantiate(_vineHeadPrefab, new Vector3(transform.position
            .x,bottomPlatform.point.y-vineHeadHeight,transform.position.z), Quaternion.identity);
    }

    
}
