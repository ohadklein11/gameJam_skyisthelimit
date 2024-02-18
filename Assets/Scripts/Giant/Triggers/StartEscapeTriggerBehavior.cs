using UnityEngine;

namespace Giant.Triggers
{
    public class StartEscapeTriggerBehavior : MonoBehaviour
    {
        [SerializeField] private GiantFightManager giantFightManager;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                giantFightManager.StartEscape();
                gameObject.SetActive(false);
            }
        }
    }
}
