using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject winTrigger;
    // Start is called before the first frame update
    void Start()
    {
       EventManagerScript.Instance.StartListening(EventManagerScript.PlayerWin, OnPlayerWin); 
       EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, ActivateWinTrigger); 

    }

    private void ActivateWinTrigger(object arg0)
    {
        winTrigger.SetActive(true);
    }

    // Update is called once per frame
    void OnPlayerWin(object arg0)
    {
        AudioManager.StopCurrentBGM();
        AudioManager.PlayWinBackground();
        SceneManager.LoadScene("WinScene");  // todo move to the same scene
    }
}
