using UnityEngine;

public class TriggerChaseEnd : MonoBehaviour
{
    public CharChase charChase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            charChase.EnterIdleState();
        }
    }
}
