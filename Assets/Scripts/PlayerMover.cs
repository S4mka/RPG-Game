using UnityEngine;

public class PlayerMover
{
    private float moveSpeed;
    private float runSpeed;

    public PlayerMover()
    {
    }

    public PlayerMover(float moveSpeed, float runSpeed)
    {
        this.moveSpeed = moveSpeed;
        this.runSpeed = runSpeed;
    }

    public Vector3 Move(Vector3 current, Vector3 dir, bool run, float dt)
    {
        float speed = run ? runSpeed : moveSpeed;
        return current + dir * speed * dt;
    }
}