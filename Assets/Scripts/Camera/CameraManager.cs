using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraPath cameraPath;
    // Start is called before the first frame update
    public void StartCameraPath()
    {
        cameraPath.StartCameraPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
