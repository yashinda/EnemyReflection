using UnityEngine;

public class TriggerChase : MonoBehaviour
{
    public CharChase charChase;
    public Transform door;
    public AudioSource audioSourceDoor;
    public AudioClip audioClipDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            charChase.EnterChaseState();
            door.Rotate(0f, -90f, 0f);
            audioSourceDoor.PlayOneShot(audioClipDoor);
        }
    }
}
