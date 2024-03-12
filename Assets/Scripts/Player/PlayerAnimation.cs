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
    private bool _forceRunOnTruck = false;


    private Collider2D _playerCollider;
    private static readonly int Beans = Animator.StringToHash("beans");
    private static readonly int IsLoading = Animator.StringToHash("isLoading");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsMovingVertically = Animator.StringToHash("isMovingVertically");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private float _climbCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDead, PlayDeadAnimation);
        _playerCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _beansShooting = GetComponent<BeansShooting>();
        _animator.SetBool(Beans, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_climbCooldown > 0f) _climbCooldown -= Time.deltaTime;
        if (_forceRunOnTruck)
        {
            if (_beansShooting.isLoading != _animator.GetBool(IsLoading))
                _animator.SetBool(IsLoading, _beansShooting.isLoading);
            return;

        }
        
        if(_playerMovement.Climbing != _animator.GetBool(IsClimbing) && _climbCooldown <= 0f) {
            if (IsShootingBeans && _playerMovement.Climbing)
            {
                SwitchToClimbingAnimation(true);   
            }
            else if (IsShootingBeans && !_playerMovement.Climbing)
            {
                SwitchToClimbingAnimation(false); 
            }
            _animator.SetBool(IsClimbing, _playerMovement.Climbing);
            _climbCooldown = .2f;
        }
        
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f)!= _animator.GetBool(IsMoving)){
            _animator.SetBool(IsMoving, (Mathf.Abs(_rb.velocity.x) >= 0.1f));
        }
        if ((Mathf.Abs(_rb.velocity.y) >= 0.1f)!= _animator.GetBool(IsMovingVertically)){
            _animator.SetBool(IsMovingVertically, (Mathf.Abs(_rb.velocity.y) >= 0.1f));
        }

        if (_beansShooting.IsShootingBeans() != _animator.GetBool(Beans))
        {
            _animator.SetBool(Beans, _beansShooting.IsShootingBeans());
        }

        if(_beansShooting.isLoading != _animator.GetBool(IsLoading)) {
            _animator.SetBool(IsLoading, _beansShooting.isLoading);
        }
        if(_playerMovement.Jumping && !_animator.GetBool(IsJumping)) {
            _animator.SetBool(IsJumping, true);
        } 
        else if(!_playerMovement.Jumping && !_playerMovement.Falling && _animator.GetBool(IsJumping)) {
            _animator.SetBool(IsJumping, false);
        }
    }
    
    private IEnumerator ForceRunOnTruckCoroutine()
    {
        _forceRunOnTruck = true;
        _playerMovement.forceDirection=true;
        _animator.SetBool(IsJumping, false);
        _animator.SetBool(IsClimbing, false);
        _animator.SetBool(IsMovingVertically, false);
        _animator.SetBool(IsMoving, true);
        SwitchToClimbingAnimation(false);
        yield return new WaitForSeconds(4f);
        Debug.Log("Forcing run on truck");

        _playerMovement.forceDirection=false;
        _forceRunOnTruck = false;
        _animator.SetBool(IsMoving, (Mathf.Abs(_rb.velocity.y) >= 0.1f));

    }

    public void ForceRunOnTruck(int direction)
    {
        StartCoroutine(ForceRunOnTruckCoroutine());
        if (_playerMovement._facingDirection != direction) _playerMovement.Flip();
    }
    
    
    public void SwitchToClimbingAnimation(bool switchToClimb)
    {
        if (switchToClimb == _isClimbBeanSettings) return;
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
        else if(_playerMovement.Climbing)
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
        _animator.SetBool(IsClimbing,false);
        _animator.SetBool(Beans, false);
        _animator.SetBool(IsMoving, false);
        _animator.SetBool(IsLoading, false);
        _animator.SetBool(IsJumping, false); 
        _animator.Play("IdleEgg");
    }
}
