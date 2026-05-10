using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;

    private PlayerModel model;
    private PlayerMover mover;

    private void Start()
    {
        model = new PlayerModel(100, 50);

        if (GameBootstrapperMB.Instance == null)
        {
            Debug.LogError("GameBootstrapperMB.Instance == null");
            enabled = false;
            return;
        }

        mover = GameBootstrapperMB.Instance.PlayerMover;

        if (mover == null)
        {
            Debug.LogError("PlayerMover == null");
            enabled = false;
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("cameraTransform is not assigned");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        if (Keyboard.current == null || mover == null || cameraTransform == null)
            return;

        float h = 0f;
        float v = 0f;

        if (Keyboard.current.aKey.isPressed) h -= 1f;
        if (Keyboard.current.dKey.isPressed) h += 1f;
        if (Keyboard.current.wKey.isPressed) v += 1f;
        if (Keyboard.current.sKey.isPressed) v -= 1f;

        Vector3 dir =
            cameraTransform.forward * v +
            cameraTransform.right * h;

        dir.y = 0f;
        dir.Normalize();

        bool run = Keyboard.current.leftShiftKey.isPressed;

        Vector3 newPos = mover.Move(
            transform.position,
            dir,
            run,
            Time.deltaTime
        );

        transform.position = newPos;

        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 10f
            );
        }

        model.Position = transform.position;
    }
}