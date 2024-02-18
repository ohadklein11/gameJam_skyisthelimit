using UnityEngine;
using Utils;

namespace Giant.Triggers
{
    public class GiantFightTriggerBehavior : MonoBehaviour
    {
        [SerializeField] private GiantFightManager giantFightManager;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GameData.Instance.StartGiantFight();
                giantFightManager.StartGiantFight();
                gameObject.SetActive(false);
            }
        }
    }
}
