using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform model;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 6.5f;

    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float deceleration = 18f;

    [SerializeField] private float rotationSpeed = 12f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -25f;

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public Vector2 lookInput;
    [HideInInspector] public bool sprintPressed;
    [HideInInspector] public bool jumpPressed;

    Vector3 currentVelocity;
    float verticalVelocity;

    public float CurrentSpeed { get; private set; }

    public bool IsGrounded => controller.isGrounded;

    void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredDirection = forward * moveInput.y + right * moveInput.x;

        desiredDirection.Normalize();

        float targetSpeed = sprintPressed ? runSpeed : walkSpeed;

        if (moveInput.sqrMagnitude < 0.01f)
            targetSpeed = 0;

        float smooth = targetSpeed > CurrentSpeed ? acceleration : deceleration;

        CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, targetSpeed, smooth * Time.deltaTime);

        currentVelocity = desiredDirection * CurrentSpeed;

        HandleRotation(desiredDirection);

        HandleGravity();

        Vector3 velocity = currentVelocity + Vector3.up * verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
        
        UpdateAnimator();
    }

    void HandleRotation(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion target = Quaternion.LookRotation(direction);

        model.rotation = Quaternion.Slerp(model.rotation, target, rotationSpeed * Time.deltaTime);
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (jumpPressed)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                jumpPressed = false;
            }
        }

        verticalVelocity += gravity * Time.deltaTime;
    }
    
    private void UpdateAnimator()
    {
        // Безопасная проверка на случай, если аниматор забыли назначить в инспекторе
        if (animator == null) return;

        // Проверяем, нажимает ли игрок кнопки движения
        bool isMovingInput = moveInput.sqrMagnitude > 0.01f;

        // 1. Управляем корневым параметром Vert (Переход от Idle к Движению)
        // Если игрок движется, плавно стремимся к 1, если стоит — к 0
        float targetVert = isMovingInput ? 1f : 0f;
    
        // Считываем текущее значение из аниматора, чтобы сделать сглаживание независимым
        float currentVert = animator.GetFloat("Vert");
        float smoothedVert = Mathf.MoveTowards(currentVert, targetVert, acceleration * Time.deltaTime);
    
        animator.SetFloat("Vert", smoothedVert);

        // 2. Управляем вложенным параметром State (Смешивание Ходьба <-> Бег)
        float targetState = 0f;

        if (isMovingInput)
        {
            // Нормализуем текущую скорость в диапазон от 0 (хотьба) до 1 (бег)
            // Математика: (ТекущаяСкорость - МинСкорость) / (МаксСкорость - МинСкорость)
            targetState = Mathf.InverseLerp(walkSpeed, runSpeed, CurrentSpeed);
        }

        float currentState = animator.GetFloat("State");
        float smoothedState = Mathf.MoveTowards(currentState, targetState, acceleration * Time.deltaTime);

        animator.SetFloat("State", smoothedState);
    }
}