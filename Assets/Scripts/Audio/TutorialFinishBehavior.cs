using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinishBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if player
        if (other.CompareTag("Player"))
        {
            AudioManager.StopCurrentBGM();
            AudioManager.PlayUpBackground();
            Destroy(gameObject);
        }
    }
}
