using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.7f, 0f);
    [SerializeField] private float distance = 5f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Smooth Follow")]
    [SerializeField] private float positionSmoothTime = 0.05f;

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("ThirdPersonCamera: Target is not assigned.");
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        HandleMouseInput();
        FollowTarget();
    }

    private void HandleMouseInput()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = target.position + offset;
        Vector3 desiredPosition = targetPosition - rotation * Vector3.forward * distance;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);
        transform.LookAt(targetPosition);
    }
}
