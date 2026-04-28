using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;

    private PlayerModel model;
    private PlayerMover mover;

    private void Start()
    {
        model = new PlayerModel(100, 50);
        mover = GameBootstrapperMB.Instance.PlayerMover;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir =
            cameraTransform.forward * v +
            cameraTransform.right * h;

        dir.y = 0;
        dir.Normalize();

        bool run = Input.GetKey(KeyCode.LeftShift);

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