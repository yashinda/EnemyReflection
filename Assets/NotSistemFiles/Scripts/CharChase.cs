using System;
using UnityEngine;

public class CharChase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelController.Instance.GameOver();
        }
    }
}
