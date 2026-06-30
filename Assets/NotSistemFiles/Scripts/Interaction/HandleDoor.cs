using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandleDoor : MonoBehaviour
{
    public Transform door;
    public GameObject mirror;

    public float angle = -20.0f;
    public float maxAngle = -150.0f;

    public AudioSource audioSource;
    public AudioClip openDoorClip;

    private void OnTriggerEnter(Collider other)
    {
        if (door.localEulerAngles.y <= maxAngle || mirror.activeSelf)
            return;
        
        if (other.CompareTag("Player"))
        {
            door.Rotate(0f, angle, 0f);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(openDoorClip);
        }
    }
}
