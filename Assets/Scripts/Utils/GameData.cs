using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class GameData: Singleton<GameData>
    {
        public bool IsGiantFight { get; private set; }
        public static bool isGameStopped=true;

        private int _curPlayerLifePoints;
        public int maxPlayerLifePoints = 100;
        public int GetPlayerHealth() => _curPlayerLifePoints;

        public void RestartPlayerHealth() => _curPlayerLifePoints = maxPlayerLifePoints;
        public bool isGiantFightOver=false;
        public bool escaping = false;
        public bool openedGiantDoors = false;
        

        private void Awake()
        {
            IsGiantFight = false;
            _curPlayerLifePoints = maxPlayerLifePoints;
        
        }

        

        

        public void SetPlayerHealth(int health)
        {
            _curPlayerLifePoints = health;
        }

        public void StartGiantFight()
        {
            IsGiantFight = true;
        }
        
        public void EndGiantFight()
        {
            IsGiantFight = false;
            isGiantFightOver = true;
        }

        private void OnRestart()
        {
            IsGiantFight = false;
            isGiantFightOver = false;
            escaping = false;
            openedGiantDoors = false;
            _curPlayerLifePoints = maxPlayerLifePoints;
            Time.timeScale = 1;
        }
        public static void Restart() => Instance.OnRestart();
        
        
    }
}