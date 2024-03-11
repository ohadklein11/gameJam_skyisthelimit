using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private KeyCode pauseKey;
    
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Press " + pauseKey + " to Continue";
    }

    // Update is called once per frame
    public void Update()
    {
        // check if pauseKey is pressed
        if (Input.GetKeyDown(pauseKey))
        {
            if (startPanel.activeSelf) return;
            pausePanel.SetActive(Time.timeScale != 0);
            Time.timeScale =1- Time.timeScale;
        }
        
        
    }
}
