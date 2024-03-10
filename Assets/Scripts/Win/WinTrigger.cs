using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.PlayerWin,null);
            gameObject.SetActive(false);
        }
    }
}
