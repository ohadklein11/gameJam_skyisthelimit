using UnityEngine;

namespace Utils
{
    public class GameData: Singleton<GameData>
    {
        public bool IsGiantFight { get; private set; }
        private int _curPlayerLifePoints;
        [SerializeField] private int maxPlayerLifePoints = 100;

        private void Start()
        {
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerGotHit, PlayerGotHit);
            _curPlayerLifePoints = maxPlayerLifePoints;
        
        }
        
        private void PlayerGotHit(object damage)
        {
            _curPlayerLifePoints -= (int)damage;
            Debug.Log("player took " + damage + " damage, " + _curPlayerLifePoints + " life points left");

            if (_curPlayerLifePoints <= 0)
            {
                Debug.Log("Player died");
            }
        }
        public void StartGiantFight()
        {
            IsGiantFight = true;
        }
        
        public void EndGiantFight()
        {
            IsGiantFight = false;
        }
    }
}