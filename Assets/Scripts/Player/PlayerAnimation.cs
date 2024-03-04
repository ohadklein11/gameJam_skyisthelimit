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
    private bool IsShootingBeans => _beansShooting.IsShootingBeans();
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
    private bool _isClimbBeanSettings = false;


    private Collider2D _playerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDead, PlayDeadAnimation);
        _playerCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _beansShooting = GetComponent<BeansShooting>();
        _animator.SetBool("beans", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_playerMovement.climbing != _animator.GetBool("isClimbing")) {
            if (IsShootingBeans && _playerMovement.climbing)
            {
                SwitchToClimbingAnimation(true);   
            }
            else if (IsShootingBeans && !_playerMovement.climbing)
            {
                SwitchToClimbingAnimation(false); 
            }
            _animator.SetBool("isClimbing", _playerMovement.climbing);
        }
        
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f)!= _animator.GetBool("isMoving")){
            _animator.SetBool("isMoving", (Mathf.Abs(_rb.velocity.x) >= 0.1f));
        }
        if ((Mathf.Abs(_rb.velocity.y) >= 0.1f)!= _animator.GetBool("isMovingVertically")){
            _animator.SetBool("isMovingVertically", (Mathf.Abs(_rb.velocity.y) >= 0.1f));
        }

        if (_beansShooting.IsShootingBeans() != _animator.GetBool("beans"))
        {
            _animator.SetBool("beans", _beansShooting.IsShootingBeans());
        }

        if(_beansShooting.isLoading != _animator.GetBool("isLoading")) {
            _animator.SetBool("isLoading", _beansShooting.isLoading);
        }
        if(_playerMovement.jumping != _animator.GetBool("isJumping")) {
            _animator.SetBool("isJumping", _playerMovement.jumping);
        } 
    }
    
    public void SwitchToClimbingAnimation(bool switchToClimb)
    {
        if (switchToClimb == _isClimbBeanSettings) return;
        Debug.Log("Switching to climbing animation: "+switchToClimb);
        float offset = switchToClimb ? 1 : -1;
        _playerCollider.offset += new Vector2(_colliderXOffest, _colliderXYOffest)*offset;
        shootingPoint.transform.localPosition += new Vector3(_shootingXOffest, _shootingYOffest, 0)*offset;
        groundCheck.transform.localPosition += new Vector3(_groundCheckXOffest, _groundCheckYOffest, 0)*offset;
        loadingCanvas.transform.localPosition += new Vector3(_loadingXOffest, _loadingYOffest, 0)*offset;
        _isClimbBeanSettings = switchToClimb;
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

    public void PlayDeadAnimation(object arg0)
    {
        string type =_beansShooting.IsShootingBeans()? "Bean" : "Egg";
        _animator.Play("dead"+type);
    }

    public void SwitchToGooseAnimation()
    {
        Debug.Log("Switching to goose animation");
        _animator.SetBool("isClimbing",false);
        _animator.SetBool("beans", false);
        _animator.SetBool("isMoving", false);
        _animator.SetBool("isLoading", false);
        _animator.SetBool("isJumping", false); 
        _animator.Play("IdleEgg");
    }
}
