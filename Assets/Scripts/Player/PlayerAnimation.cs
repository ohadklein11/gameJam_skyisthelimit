using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{


    private Animator _animator;
    private Rigidbody2D _rb;
    private PlayerMovement _playerMovement;
    private BeansShooting _beansShooting;
    private bool _isShootingBeans;
    [SerializeField] private GameObject groundCheck;
    private float _groundCheckXOffest = 0.54f;
    private float _groundCheckYOffest = -0.27f;
    private float _colliderXOffest = 1.02f;
    private float _colliderXYOffest = -0.2f;
    [SerializeField] private GameObject shootingPoint;
    private float _shootingXOffest = 2.133f;
    private float _shootingYOffest = -3.995f;
    [SerializeField] private GameObject loadingCanvas;
    private float _loadingXOffest = 1.09f;
    private float _loadingYOffest = -0.39f;


    private Collider2D _playerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _beansShooting = GetComponent<BeansShooting>();
        _animator.SetBool("beans", true);
        _isShootingBeans= true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_playerMovement.climbing != _animator.GetBool("isClimbing")) {
            if (_isShootingBeans && _playerMovement.climbing)
            {
                // give offset to player collider
                _playerCollider.offset += new Vector2(_colliderXOffest, _colliderXYOffest);
                shootingPoint.transform.localPosition += new Vector3(_shootingXOffest, _shootingYOffest, 0);
                groundCheck.transform.localPosition += new Vector3(_groundCheckXOffest, _groundCheckYOffest, 0);
                loadingCanvas.transform.localPosition += new Vector3(_loadingXOffest, _loadingYOffest, 0);
            }
            else if (_isShootingBeans && !_playerMovement.climbing)
            {
                _playerCollider.offset -= new Vector2(_colliderXOffest, _colliderXYOffest);
                shootingPoint.transform.localPosition -= new Vector3(_shootingXOffest, _shootingYOffest, 0);
                groundCheck.transform.localPosition -= new Vector3(_groundCheckXOffest, _groundCheckYOffest, 0);
                loadingCanvas.transform.localPosition -= new Vector3(_loadingXOffest, _loadingYOffest, 0);

            }
            _animator.SetBool("isClimbing", _playerMovement.climbing);
        }
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f)!= _animator.GetBool("isMoving")){
            _animator.SetBool("isMoving", (Mathf.Abs(_rb.velocity.x) >= 0.1f));
        }
        if ((Mathf.Abs(_rb.velocity.y) >= 0.1f)!= _animator.GetBool("isMovingVertically")){
            _animator.SetBool("isMovingVertically", (Mathf.Abs(_rb.velocity.y) >= 0.1f));
        }
        if(_beansShooting.isLoading != _animator.GetBool("isLoading")) {
            _animator.SetBool("isLoading", _beansShooting.isLoading);
        }
        if(_playerMovement.jumping != _animator.GetBool("isJumping")) {
            _animator.SetBool("isJumping", _playerMovement.jumping);
        } 
    }
    /**
     shootingType: 0 for bean shooting, 1 for egg shooting
     */
    public void PlayShootingAnimation(int shootingType)
    {
        string animationName = shootingType == 0 ? "Bean" : "Egg";
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f))
            _animator.Play("run"+animationName+"Shoot");
        else if(_playerMovement.climbing)
        {
            _animator.Play("climb"+animationName+"Shoot");
        }
        else
        {
            _animator.Play("Idle"+animationName+"Shoot");

        }
    }

    public void SwitchToGooseAnimation()
    {
        Debug.Log("Switching to goose animation");
        _animator.SetBool("isClimbing",false);
        _animator.SetBool("beans", false);
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isLoading", false);
        _animator.SetBool("isJumping", false); 
        _isShootingBeans = false;
        _animator.Play("IdleEgg");
    }
}
