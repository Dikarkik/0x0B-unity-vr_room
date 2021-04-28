using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private CameraRaycast cameraRaycastScript;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraRaycastScript = FindObjectOfType<CameraRaycast>();
    }

    private void OnCollisionEnter(Collision other)
    {
        cameraRaycastScript.DropObject();
    }
}
