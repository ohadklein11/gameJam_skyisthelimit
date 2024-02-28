using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHealthFillerAfterGIantFigth : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject healthFillerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        EventManagerScript.Instance.StartListening(EventManagerScript.GiantFightEnd, SpawnHealthFiller);
    }

    // Update is called once per frame
    void SpawnHealthFiller(object obj)
    {
        Instantiate(healthFillerPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
}
