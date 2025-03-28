using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCamera : MonoBehaviour
{
    public Camera MapCamera;
    public Camera PlayerCamera;
    bool selectCamera = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            selectCamera = !selectCamera;
            UpdateCamera();
        }        
    }
    private void UpdateCamera()
    {
        if (MapCamera != null) MapCamera.enabled = !selectCamera;
        if (PlayerCamera != null) PlayerCamera.enabled = selectCamera;
    }
}
