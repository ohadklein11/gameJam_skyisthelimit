using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class GameData: Singleton<GameData>
    {
        public bool IsGiantFight { get; private set; }
        public static bool isGameStopped=true;

        private int _curPlayerLifePoints;
        [SerializeField] private int maxPlayerLifePoints = 100;

        private void Awake()
        {
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerGotHit, PlayerGotHit);
            EventManagerScript.Instance.StartListening(EventManagerScript.HealthRecovery, HealthPointsFiller);

            IsGiantFight = false;
            _curPlayerLifePoints = maxPlayerLifePoints;
        
        }
        
        private void PlayerGotHit(object damage)
        {
            _curPlayerLifePoints -= (int)damage;
            Debug.Log("player took " + damage + " damage, " + _curPlayerLifePoints + " life points left");

            if (_curPlayerLifePoints <= 0)
            {
                Debug.Log("Player died");
                SceneManager.LoadScene("LostScene");
            }
        }
        private void HealthPointsFiller(object healthPoints)
        {
            _curPlayerLifePoints = Math.Min(maxPlayerLifePoints,_curPlayerLifePoints+(int)healthPoints);
            Debug.Log("player recovered " + healthPoints + " health points, " + _curPlayerLifePoints + " life points left");

        }
        public void StartGiantFight()
        {
            IsGiantFight = true;
        }
        
        public void EndGiantFight()
        {
            IsGiantFight = false;
        }

        private void OnRestart()
        {
            IsGiantFight = false;
            _curPlayerLifePoints = maxPlayerLifePoints;
        }
        public static void Restart() => Instance.OnRestart();
    }
}