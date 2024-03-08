using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class TutorialBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject climbArrows;

        private void Start()
        {
            EventManagerScript.Instance.StartListening(EventManagerScript.FirstVine, arg0 =>
            {
                climbArrows.SetActive(true);
                foreach (var spriteRenderer in climbArrows.GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.color = new Color(1, 1, 1, 0);
                    spriteRenderer.DOFade(1f, 2f).SetEase(Ease.InSine);
                }
            });
        }
    }
}
