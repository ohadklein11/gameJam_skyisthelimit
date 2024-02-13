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
    [SerializeField] private GameObject _vinePrefab;

    [SerializeField] private float distanceToUpperPlatform = 20f;
    
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Grounded = false;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
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
        var hit = Physics2D.Raycast(BeanBottomCenter, Vector2.down, distance, _groundLayerMask);
        if (hit&& !Grounded)
        {
            GrowVine();
            Grounded = true;
        }
    }
    
    private void GrowVine()
    {
        float vineWidth = _vinePrefab.gameObject.transform.lossyScale.x;
        float vineHeight = _vinePrefab.gameObject.transform.lossyScale.y/2;

        Vector3 position = transform.position + new Vector3(0,transform.localScale.y,0);
        var hit = Physics2D.Raycast(position, Vector2.up, distanceToUpperPlatform, _groundLayerMask);
        var hitLeft = Physics2D.Raycast(position, Vector2.left, vineWidth*0.67f, _VineLayerMask);
        var hitRight = Physics2D.Raycast(position, Vector2.right, vineWidth*0.67f, _VineLayerMask);

        if (hit&& !hitLeft && !hitRight)
        {
            Instantiate(_vinePrefab, transform.position+new Vector3(0,vineHeight,0), Quaternion.identity);
            // GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Vine Grown");

        }
        Destroy(gameObject);

    }
}
