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
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private float nonGravityTime;
    private GameObject _vineHeadPrefab;
    private const float Epsilon = 0.3f;  // to deal with instantiating vines on slopes 
    private float _initialGravityScale;
    private CircleCollider2D _collider;  
    

    private Camera _mainCamera;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Grounded = false;
        _mainCamera = Camera.main;
        _vineHeadPrefab= Resources.Load<GameObject>("Prefabs/Vines/VineHead");
        _collider = GetComponent<CircleCollider2D>();
        _initialGravityScale= GetComponent<Rigidbody2D>().gravityScale;
    }

    void Start()
    {
        StartCoroutine(ApplyRigidbody());
    }


    IEnumerator ApplyRigidbody()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
        yield return new WaitForSeconds(nonGravityTime);
        GetComponent<Rigidbody2D>().gravityScale = _initialGravityScale;

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
        Vector3 BeanBottomCenter = position - new Vector3(0, (spriteBounds.max.y - spriteBounds.min.y) / 2);
        RaycastHit2D hit = Physics2D.Raycast(BeanBottomCenter, Vector2.down, distance, _groundLayerMask);
        if (hit&& !Grounded && hit.normal.y > 0.8f)
        {
            Debug.Log(hit.normal);
            GrowVine(hit);
            Grounded = true;
        }
    }
    
    private void GrowVine(RaycastHit2D bottomPlatform)
    {
        float vineWidth = _vineHeadPrefab.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        float vineHeight = _vineHeadPrefab.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        Vector3 upRayPosition = new Vector3(transform.position.x - vineWidth * 0.67f, _spriteRenderer.bounds.max.y + vineHeight, transform.position.z) ;
        Vector3 downRayPosition = new Vector3(transform.position.x - vineWidth * 0.67f, _spriteRenderer.bounds.min.y - vineHeight, transform.position.z) ;

        var hitUp = Physics2D.Raycast(upRayPosition, Vector2.right, vineWidth * 1.5f, _VineLayerMask);
        var hitDown = Physics2D.Raycast(downRayPosition, Vector2.right, vineWidth * 1.5f, _VineLayerMask);
        Debug.DrawRay(upRayPosition, Vector3.right * vineWidth * 1.5f, Color.red);
        Debug.DrawRay(downRayPosition, Vector3.right * vineWidth * 1.5f, Color.red);


        // if (!hitLeft && !hitRight)
        if (!hitUp && !hitDown)
        {
            GameObject vine = BuildVine(bottomPlatform); 
            vine.transform.SetParent(bottomPlatform.transform,true); 
            
            Debug.Log("Vine Grown");

        }
        Destroy(gameObject);

    }
    GameObject BuildVine(RaycastHit2D bottomPlatform)
    {
        float vineHeadHeight = _vineHeadPrefab.gameObject.GetComponentInChildren<SpriteRenderer>().bounds.size.y;
        float growthPositionY = bottomPlatform.point.y - vineHeadHeight / 2 - Epsilon;
        var position = transform.position;
        return Instantiate(_vineHeadPrefab, new Vector3(
            position.x,growthPositionY, position.z), Quaternion.identity);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
    }

}
