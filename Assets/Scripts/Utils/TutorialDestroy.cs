using UnityEngine;

namespace Utils
{
    public class TutorialDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject tutorial;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                tutorial.SetActive(false);
            }
        }
    }
}
