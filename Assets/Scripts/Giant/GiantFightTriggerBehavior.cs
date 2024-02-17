using UnityEngine;

namespace Giant
{
    public class GiantFightTriggerBehavior : MonoBehaviour
    {
        [SerializeField] private GiantFightManager giantFightManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                giantFightManager.StartGiantFight();
                gameObject.SetActive(false);
            }
        }
    }
}
