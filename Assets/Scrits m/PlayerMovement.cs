using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSmoothTime = 0.1f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundOffset = 0.2f;
    [SerializeField] private float groundRadius = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentRotationVelocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        PlayerHealth health = GetComponent<PlayerHealth>();

        if (health.isHit || health.currentHealth <= 0)
        {
            return; // Не двигаемся
        }
        GroundCheck();
        Move();
        ApplyGravity();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = transform.position + Vector3.down * groundOffset;
        isGrounded = Physics.CheckSphere(spherePosition, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
    }

    private void Move()
    {
        Vector2 input = ReadMovementInput();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentRotationVelocity, rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            bool isRunning = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
            float speed = isRunning ? runSpeed : walkSpeed;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }

    private Vector2 ReadMovementInput()
    {
        Vector2 input = Vector2.zero;

        
        if (Keyboard.current == null)
            return input;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            input.x -= 1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            input.y -= 1f;

        return input.normalized;
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}