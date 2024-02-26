using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void RestartButton()
    {
        SceneManager.LoadScene("CodeScene");
    }
    // Update is called once per frame
    public void QuitButton()
    {
        Application.Quit();
    }
}
