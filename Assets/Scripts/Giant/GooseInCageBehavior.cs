using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GooseInCageBehavior : MonoBehaviour
{
    [SerializeField] private GiantFightManager giantFightManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            giantFightManager.TookGoose();
            gameObject.SetActive(false);
        }
    }
}
