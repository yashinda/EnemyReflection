using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Максимально допустимый угол между взглядом игрока и объектом")]
    [SerializeField]
    private float maxInteractionAngle = 30.0f;
    
    [Header("Physics Interaction")]
    [Tooltip("Сила, с которой игрок толкает физические объекты")]
    [SerializeField] private float interactionForce = 5f;
    
    public float InteractionForce => interactionForce;
    
    private Interactable _currentInteractable;

    private void Update()
    {
        if (_currentInteractable == null)
            return;

        if (IsPlayerLookingAt(_currentInteractable))
        {
            _currentInteractable.ToggleUI(true);
        }
        else
        {
            _currentInteractable.ToggleUI(false);
        }
    }

    private bool IsPlayerLookingAt(Interactable target)
    {
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        
        Vector3 playerForward = transform.forward;
        
        float angle = Vector3.Angle(playerForward, directionToTarget);
        
        return angle <= maxInteractionAngle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            _currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (_currentInteractable == interactable)
            {
                _currentInteractable.ToggleUI(false);
                _currentInteractable = null;
            }
        }
    }

    private void OnInteract()
    {
        if (_currentInteractable == null) 
            return;
        
        if (IsPlayerLookingAt(_currentInteractable))
        {
            _currentInteractable.Interact(this);
        }
    }
}
