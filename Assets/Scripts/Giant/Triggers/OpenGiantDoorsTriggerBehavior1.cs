using UnityEngine;

namespace Giant.Triggers
{
    public class OpenGiantDoorsTrigger : MonoBehaviour
    {
        [SerializeField] private GiantFightManager giantFightManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                giantFightManager.OpenGiantDoors();
                gameObject.SetActive(false);
            }
        }
    }
}
