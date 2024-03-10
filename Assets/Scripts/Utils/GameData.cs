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
            _curPlayerLifePoints = maxPlayerLifePoints;
        }
        public static void Restart() => Instance.OnRestart();
        
        
    }
}