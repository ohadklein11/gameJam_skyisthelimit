using UnityEngine;
using Utils;

public class StartScript : MonoBehaviour
{
    public GameObject startPanel;
    // Start is called before the first frame update
    void Start()
    {
        GameData.isGameStopped = true;
        startPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.anyKey && GameData.isGameStopped)
        {
            GameData.isGameStopped = false;
            startPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
