using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void OnMove(InputValue value)
    {
        playerController.moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
            playerController.jumpPressed = true;
    }

    void OnSprint(InputValue value)
    {
        if (value.isPressed)
            playerController.sprintPressed = !playerController.sprintPressed;
    }

    private void Start()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}