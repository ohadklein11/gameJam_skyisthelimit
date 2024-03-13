using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class StartScript : MonoBehaviour
{
    public GameObject startPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image logo;

    [SerializeField] private CanvasGroup healthBar;
    [SerializeField] private CanvasGroup heightBar;
    
    // Start is called before the first frame update
    void Start()
    {
        var transform1 = logo.transform;
        var scale = transform1.localScale;
        transform1.localScale = new Vector3(0, 0, 0);
        GameData.isGameStopped = true;
        healthBar.gameObject.SetActive(false);
        heightBar.gameObject.SetActive(false);
        startPanel.SetActive(true);
        logo.DOFade(1, 3f).SetEase(Ease.OutSine);
        // tween the logo's size from size 0 to current size
        logo.transform.DOScale(scale, 3f).SetEase(Ease.OutCubic);
        

    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.anyKey && GameData.isGameStopped)
        {
            GameData.isGameStopped = false;
            healthBar.gameObject.SetActive(true);
            healthBar.DOFade(1f, 1f).SetEase(Ease.OutFlash);
            heightBar.gameObject.SetActive(true);
            heightBar.DOFade(1f, 1f).SetEase(Ease.OutFlash);
            startPanel.SetActive(false);
            audioSource.Play();
        }
    }
}
