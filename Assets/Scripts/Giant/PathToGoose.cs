using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathToGoose : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    
    // when player touches the first point, the player will start following the path
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            iTween.MoveTo(other.gameObject, iTween.Hash("path", points, "time", 5, "easetype", iTween.EaseType.linear));
        }
    }

    private void OnDrawGizmos()
    {
        iTween.DrawPath(points, Color.green);
    }
}
