using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraPath : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;

    private bool _pathTurnedOff = true;
    
    public void SetActivePath(bool activate)
    {
        _pathTurnedOff = activate;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_pathTurnedOff)
                cameraManager.StartCameraPath();
            // cameraPathManager.StartCameraPath();
            gameObject.SetActive(false);
        }
    }
}
