using UnityEngine;

public class HealthMono : MonoBehaviour
{
    public float hp = 100;

    public void TakeDamage(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
            Destroy(gameObject);
    }
}