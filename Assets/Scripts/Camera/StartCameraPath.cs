using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraPath : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraManager.StartCameraPath();
            // cameraPathManager.StartCameraPath();
            gameObject.SetActive(false);
        }
    }
}
