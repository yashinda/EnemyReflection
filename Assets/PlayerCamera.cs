using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.6f, -3f);

    [Header("Settings")]
    public float sensitivity = 2.0f;
    public float smoothTime = 0.05f;

    [Header("Rotation Limits")]
    public float minPitch = -35f;
    public float maxPitch = 70f;

    private float yaw;
    private float pitch;

    private Vector2 lookInput;

    private Vector3 currentVelocity;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );

        transform.rotation = rotation;
    }
}