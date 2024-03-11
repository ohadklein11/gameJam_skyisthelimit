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
    // Start is called before the first frame update
    void Start()
    {
        GameData.isGameStopped = true;
        startPanel.SetActive(true);
        logo.DOFade(1, 3f).SetEase(Ease.OutSine);
        // tween the logo's size from size 0 to current size
        logo.transform.DOScale(new Vector3(1, 1, 1), 3f).SetEase(Ease.OutCubic);
        

    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.anyKey && GameData.isGameStopped)
        {
            GameData.isGameStopped = false;
            startPanel.SetActive(false);
            audioSource.Play();
        }
    }
}
