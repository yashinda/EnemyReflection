using System;
using UnityEngine;

public class HandleDoor : MonoBehaviour
{
    public Transform door;
    public GameObject mirror;

    private float _angle = -20.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (door.localEulerAngles.y <= -150.0f || mirror.activeSelf)
            return;
        
        if (other.CompareTag("Player"))
        {
            door.Rotate(0f, _angle, 0f);
        }
    }
}
