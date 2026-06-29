using System;
using UnityEngine;

public class CanvasWorldSpaceController : MonoBehaviour
{
    private Camera playerCamera;
    
    private void Start()
    {
        playerCamera = Camera.main;
    }
    
    private void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
    }
}
