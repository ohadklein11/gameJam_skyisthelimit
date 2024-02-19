using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraPath : MonoBehaviour
{
    [SerializeField] private CameraPath cameraPathManager;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraPathManager.enabled = true;
            // cameraPathManager.StartCameraPath();
            gameObject.SetActive(false);
        }
    }
}
