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

    
    // Start is called before the first frame update
    void Start()
    {
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
            _animator.SetBool("isClimbing", _playerMovement.climbing);
        }
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f)!= _animator.GetBool("isMoving")){
            _animator.SetBool("isMoving", (Mathf.Abs(_rb.velocity.x) >= 0.1f));
        }
        if(_beansShooting.isLoading != _animator.GetBool("isLoading")) {
            _animator.SetBool("isLoading", _beansShooting.isLoading);
        }
        if(_playerMovement.jumping != _animator.GetBool("isJumping")) {
            _animator.SetBool("isJumping", _playerMovement.jumping);
        } 
    }
    public void PlayShootingGooseAnimation()
    {
        if ((Mathf.Abs(_rb.velocity.x) >= 0.1f))
            _animator.Play("runEggShoot");
        else
        {
            _animator.Play("IdleEggShoot");
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
        _animator.Play("IdleEgg");
    }
}
