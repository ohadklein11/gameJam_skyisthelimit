using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;

    private CanvasGroup _canvasGroup;
    private void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerWin, OnPlayerWin);
        EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDead, OnPlayerLose);
        _canvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
    }

    private void OnPlayerLose(object arg0)
    {
        loseImage.SetActive(true);
        StartCoroutine(OnGameEnd(2f));
    }

    private void OnPlayerWin(object arg0)
    {
        winImage.SetActive(true);
        StartCoroutine(OnGameEnd(0));
    }

    IEnumerator OnGameEnd(float time)
    {
        yield return new WaitForSeconds(time);
        GameData.isGameStopped = true;
        gameOverScreen.SetActive(true);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0,
            "to", 1,
            "time", 3,
            "onupdate", "FadeScreenIn",
            "easetype", iTween.EaseType.easeOutSine));
            
    }
        
    void FadeScreenIn(float alpha)
    {
        _canvasGroup.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
