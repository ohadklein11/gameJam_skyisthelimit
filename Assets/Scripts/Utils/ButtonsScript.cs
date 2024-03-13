using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField] private Restarter restarter;
    // Start is called before the first frame update
    
    public void RestartButton()
    {
        restarter.RestartGame();
    }   
    // Update is called once per frame
    public void QuitButton()
    {
        Application.Quit();
    }
}
