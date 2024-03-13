using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class Restarter : MonoBehaviour
    {
        [SerializeField] private KeyCode _restartKey = KeyCode.F1;
        private bool restart;
    
        //restarts the game to the initial state
    
        void Awake() {
            // DontDestroyOnLoad(this.gameObject);
            restart = false;
            Debug.Log("Restarter Start");
        }

        void Update() {
            if (Input.GetKeyDown(_restartKey) && !restart) {
                restart = true;  
                Debug.Log("Restarted");
                GameData.Restart();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Application.Quit();
            }
        }
    }
}
