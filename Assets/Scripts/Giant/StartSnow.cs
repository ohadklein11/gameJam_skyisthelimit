using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSnow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject snow;

    private void Update()
    {
        if (Mathf.Abs(player.position.x - snow.transform.position.x) < 50)
        {
            snow.SetActive(true);
            enabled = false;
        }
    }
}
