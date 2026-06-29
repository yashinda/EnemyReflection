using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : Interactable
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _rb.angularDamping = 0.5f;
        _rb.linearDamping = 0.2f;
    }
    
    public override void Interact(PlayerInteraction player = null)
    {
        if (player == null)
            return;

        float pushForce = player.InteractionForce;
        
        Vector3 forceDirection = (transform.position - player.transform.position).normalized;
        
        forceDirection.y = 0;
        
        _rb.AddForce(forceDirection * pushForce, ForceMode.Impulse);
        
        _rb.AddTorque(Random.insideUnitSphere * pushForce, ForceMode.Impulse);
        
        ToggleUI(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mirror"))
            collision.gameObject.SetActive(false);
    }
}
