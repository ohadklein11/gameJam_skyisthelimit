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
        if(_playerMovement.climbing) {
            _animator.SetBool("isClimbing", true);
        } else {
            _animator.SetBool("isClimbing", false);
        }
        if (_rb.velocity.magnitude >= 0.1f) {
            _animator.SetBool("isMoving", true);
        } else {
            _animator.SetBool("isMoving", false);
        }
        if (_beansShooting._gunType == GunType.BeansGun) {
            _animator.SetBool("beans", true);
        } else {
            _animator.SetBool("beans", false);
        }
        if(_beansShooting.isLoading) {
            _animator.SetBool("isLoading", true);
        } else {
            _animator.SetBool("isLoading", false);
        }
    }
}
