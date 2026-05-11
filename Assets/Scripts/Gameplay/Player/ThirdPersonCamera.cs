using UnityEngine;

namespace AdvancedRPG.Gameplay.Player
{
    public sealed class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2.2f, -5f);
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float minPitch = -25f;
        [SerializeField] private float maxPitch = 65f;

        private float yaw;
        private float pitch = 20f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            var rotation = Quaternion.Euler(pitch, yaw, 0f);
            transform.position = target.position + rotation * offset;
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }
}
