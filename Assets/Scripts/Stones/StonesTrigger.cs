using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _stonesGenerator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.LeavingGiantTemple,"end");

            _stonesGenerator.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
