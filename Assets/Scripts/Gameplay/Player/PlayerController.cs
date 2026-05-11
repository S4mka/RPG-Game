using UnityEngine;

namespace AdvancedRPG.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 7f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Animator animator;

        private CharacterController controller;
        private Vector3 velocity;
        private const float Gravity = -20f;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            input = Vector2.ClampMagnitude(input, 1f);
            var running = Input.GetKey(KeyCode.LeftShift);
            var speed = running ? runSpeed : walkSpeed;

            var camForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            var camRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            var move = camRight * input.x + camForward * input.y;

            if (move.sqrMagnitude > 0.001f)
            {
                var targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (controller.isGrounded && velocity.y < 0f) velocity.y = -2f;
            velocity.y += Gravity * Time.deltaTime;
            controller.Move((move * speed + velocity) * Time.deltaTime);

            animator?.SetFloat("Speed", input.magnitude * (running ? 2f : 1f));
        }
    }
}
