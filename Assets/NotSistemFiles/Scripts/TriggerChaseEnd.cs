using UnityEngine;

public class TriggerChaseEnd : MonoBehaviour
{
    public CharChase charChase;
    public Transform door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            charChase.EnterIdleState();
            Quaternion rotationDoor = new Quaternion(0f, -180f, 0f, 0f);
            door.rotation = rotationDoor;
        }
    }
}
