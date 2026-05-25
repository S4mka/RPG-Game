using UnityEngine;
using UnityEngine.InputSystem;

namespace AdvancedRPG.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator animator;

        [Header("New Input System Actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference runAction;

        private CharacterController controller;
        private Vector3 verticalVelocity;
        private const float Gravity = -20f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();

            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            moveAction?.action.Enable();
            runAction?.action.Enable();
        }

        private void OnDisable()
        {
            moveAction?.action.Disable();
            runAction?.action.Disable();
        }

        private void Update()
        {
            var input = ReadMoveInput();
            var running = ReadRunInput();
            var speed = running ? runSpeed : walkSpeed;

            var move = CalculateCameraRelativeMove(input);
            RotateToMoveDirection(move);
            ApplyGravity();

            controller.Move((move * speed + verticalVelocity) * Time.deltaTime);
            bool isWalking = input.sqrMagnitude > 0.01f;
            animator.SetBool("Walk", isWalking);
        }

        private Vector2 ReadMoveInput()
        {
            if (moveAction == null || moveAction.action == null)
                return Vector2.zero;

            return Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1f);
        }

        private bool ReadRunInput()
        {
            return runAction != null && runAction.action != null && runAction.action.IsPressed();
        }

        private Vector3 CalculateCameraRelativeMove(Vector2 input)
        {
            if (cameraTransform == null)
                return new Vector3(input.x, 0f, input.y);

            var camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            var camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            return camRight * input.x + camForward * input.y;
        }

        private void RotateToMoveDirection(Vector3 move)
        {
            if (move.sqrMagnitude <= 0.001f)
                return;

            var targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (controller.isGrounded && verticalVelocity.y < 0f)
                verticalVelocity.y = -2f;

            verticalVelocity.y += Gravity * Time.deltaTime;
        }
    }
}
