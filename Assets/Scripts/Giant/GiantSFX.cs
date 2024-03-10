using UnityEngine;

namespace Giant
{
    public class GiantSFX : MonoBehaviour
    {
        [SerializeField] private AudioSource audioRoar;
        [SerializeField] private AudioSource audioHit;
        [SerializeField] private AudioSource audioThrow;
        [SerializeField] private AudioSource audioCry;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem tearsLeft;
        [SerializeField] private ParticleSystem tearsRight;

        private bool _roar;
        private bool _hit;
        private bool _throw;
        private bool _cry;

        private bool _standRoar;

        // Start is called before the first frame update
        void Start()
        {
            audioCry.mute = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_standRoar) return;
            var state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("roar") || state.IsName("strong roar"))
            {
                if (!_roar)
                {
                    _roar = true;
                    var pitch = Random.Range(0.8f, 1.2f);
                    audioRoar.pitch = pitch;
                    audioRoar.Play();
                }
            }
            else
            {
                _roar = false;
            }
            if (state.IsName("hit") || state.IsName("small hit"))
            {
                if (!_hit)
                {
                    _hit = true;
                    audioHit.Play();
                }
            }
            else
            {
                _hit = false;
            }
            if (state.IsName("throw"))
            {
                if (!_throw)
                {
                    _throw = true;
                    audioThrow.Play();
                }
            }
            else
            {
                _throw = false;
            }
            if (state.IsName("cry"))
            {
                if (!_cry)
                {
                    _cry = true;
                    audioCry.mute = false;
                    tearsLeft.Play();
                    tearsRight.Play();
                }
            }
            else
            {
                _cry = false;
            }
            if (state.IsName("stand roar"))
            {
                if (!_standRoar)
                {
                    audioCry.mute = true;
                    tearsLeft.Stop();
                    tearsRight.Stop();
                    _standRoar = true;
                    audioRoar.loop = true;
                    audioRoar.Play();
                }
            }
        }
    }
}
