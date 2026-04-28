using UnityEngine;

public class PlayerModel
{
    public float HP { get; private set; }
    public float MaxHP { get; private set; }
    public float MP { get; private set; }

    public Vector3 Position;

    public PlayerModel(float hp, float mp)
    {
        MaxHP = hp;
        HP = hp;
        MP = mp;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP < 0) HP = 0;
    }

    public void Restore(float hp, float mp)
    {
        HP = hp;
        MP = mp;
    }
}