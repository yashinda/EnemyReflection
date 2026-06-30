using UnityEngine;
using Random = UnityEngine.Random;

public enum PushType
{
    Ball,
    Cup,
    Mouse
}

public class InteractPush : Interactable
{
    private Rigidbody _rb;
    public AudioSource audioSource;
    public AudioClip clipBall;
    public AudioClip breakeClip;
    
    public PushType pushType;

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
            
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clipBall);

        ToggleUI(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mirror") && pushType == PushType.Ball)
        {
            collision.gameObject.SetActive(false);
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(breakeClip);
        }

        if (collision.gameObject.CompareTag("Floor") && pushType != PushType.Ball)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(breakeClip);
        }
    }
}
