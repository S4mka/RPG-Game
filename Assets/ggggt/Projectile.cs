using UnityEngine;

public class Projectile : MonoBehaviour
{
    private DamageData damage;

    public float speed = 10f;
    public float lifeTime = 5f;

    public void Init(DamageData damageData)
    {
        damage = damageData;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}