using UnityEngine;

namespace Utils
{
    public class AnimatorStartDelay : MonoBehaviour
    {
        private Animator _animator;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.enabled = false;
            Invoke("PlayAnimation", Random.Range(0f, 3f));
            EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, DoubleSpeed);
        }

        private void DoubleSpeed(object arg0)
        {
            _animator.speed = 4;
        }

        void PlayAnimation()
        {
            _animator.enabled = true;
        }

    }
}
