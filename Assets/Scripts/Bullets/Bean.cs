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

    [SerializeField] private float distanceToUpperPlatform = 20f;
    [SerializeField] private float platformWidth = 5f;
    
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
        Debug.DrawRay(BeanBottomCenter, Vector3.down * distance, Color.red);
        if (hit&& !Grounded)
        {
            GrowVine();
            Grounded = true;
        }
    }
    
    private void GrowVine()
    {
        Vector3 position = transform.position + new Vector3(0,transform.localScale.y,0);
        var hit = Physics2D.Raycast(position, Vector2.up, distanceToUpperPlatform, _groundLayerMask);
        var hitLeft = Physics2D.Raycast(position, Vector2.left, platformWidth/2, _VineLayerMask);
        var hitRight = Physics2D.Raycast(position, Vector2.right, platformWidth/2, _VineLayerMask);

        if (hit&& !hitLeft && !hitRight)
        {
            Destroy(gameObject,1f);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Debug.Log("Vine Grown");

        }
    }
}
