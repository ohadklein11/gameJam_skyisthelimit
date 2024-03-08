using Cinemachine;
using DG.Tweening;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Giant
{
    public class GiantStrongRoarBehavior : StateMachineBehaviour
    {
        private PlayerMovement _playerMovement;
        [SerializeField] private float shakeDuration = 1f;
        [SerializeField] private float shakeStrength = 0.5f;
        private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private float cameraShakeDuration = 1f;
        [SerializeField] private float cameraShakeMagnitude = 2f;
        [SerializeField] private float _climbCooldown = 1f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.transform.DOShakePosition(shakeDuration, shakeStrength, 30, 90, false, false);
            _virtualCamera.GetComponent<CameraShake>().Shake(cameraShakeMagnitude, cameraShakeDuration);
            _playerMovement.StopClimbing(cooldown: _climbCooldown);
        }
	

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
        public void Init(PlayerMovement playerMovement1)
        {
            _playerMovement = playerMovement1;
            _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
    }
}
