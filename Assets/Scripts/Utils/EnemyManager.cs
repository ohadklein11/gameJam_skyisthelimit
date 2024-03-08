using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class EnemyManager: MonoBehaviour
    {
        // init disabled enemies as a set of game objects
        private HashSet<GameObject> _disabledEnemies;
        
        
        Transform PlayerTransform => GameObject.FindWithTag("Player").transform;
        [SerializeField] private float distanceToEnable = 15;
        
        private void Start()
        {
            _disabledEnemies = new HashSet<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                _disabledEnemies.Add(transform.GetChild(i).gameObject);
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            // enable enemies close to player
            foreach (var enemy in _disabledEnemies)
            {
                if (Vector3.Distance(enemy.transform.position, PlayerTransform.position) < distanceToEnable)
                {
                    enemy.SetActive(true);
                    _disabledEnemies.Remove(enemy);
                    break;
                }
            }
        }
    }
}