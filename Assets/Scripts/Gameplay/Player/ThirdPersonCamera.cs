using UnityEngine;
using UnityEngine.InputSystem;

namespace AdvancedRPG.Gameplay.Player
{
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2.2f, -5f);
        [SerializeField] private float mouseSensitivity = 0.12f;
        [SerializeField] private float minPitch = -25f;
        [SerializeField] private float maxPitch = 65f;

        [Header("New Input System Actions")]
        [SerializeField] private InputActionReference lookAction;

        private float yaw;
        private float pitch = 20f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            lookAction?.action.Enable();
        }

        private void OnDisable()
        {
            lookAction?.action.Disable();
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            var lookInput = ReadLookInput();
            yaw += lookInput.x * mouseSensitivity;
            pitch -= lookInput.y * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            var rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.position = target.position + rotation * offset;
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }

        private Vector2 ReadLookInput()
        {
            if (lookAction == null || lookAction.action == null)
                return Vector2.zero;

            return lookAction.action.ReadValue<Vector2>();
        }
    }
}
