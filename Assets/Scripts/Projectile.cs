using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float damage = 20f;

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
        var health = other.GetComponent<HealthMono>();

        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}