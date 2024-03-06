using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _stonesGenerator;
    [SerializeField] private GameObject tutorial;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManagerScript.Instance.TriggerEvent(EventManagerScript.LeavingGiantTemple,"end");

            tutorial.SetActive(false);
            _stonesGenerator.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
