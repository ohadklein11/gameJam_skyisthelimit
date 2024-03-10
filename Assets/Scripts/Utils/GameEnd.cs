using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class GameEnd : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject restartButton;

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
        EventSystem.current.SetSelectedGameObject(restartButton);
        player.GetComponent<Rigidbody2D>().bodyType =RigidbodyType2D.Static ;
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0,
            "to", 1,
            "time", 3,
            "onupdate", "FadeScreenIn",
            "easetype", iTween.EaseType.easeOutSine));
        yield return new WaitForSeconds(3f);
        enemies.SetActive(false);
    }
        
    void FadeScreenIn(float alpha)
    {
        _canvasGroup.GetComponent<CanvasGroup>().alpha = alpha;
    }
}
