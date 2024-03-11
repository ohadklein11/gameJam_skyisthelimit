using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private int direction = 1;
    
    // when player touches the first point, the player will start following the path
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerAnimation>().ForceRunOnTruck(direction);
            // if we want player to control the movement, we can use PutOnPath instead
            iTween.MoveTo(other.gameObject, iTween.Hash("path", points, "time", 4, "easetype", iTween.EaseType.linear));
        }
    }

    private void OnDrawGizmos()
    {
        iTween.DrawPath(points, Color.green);
    }
}
