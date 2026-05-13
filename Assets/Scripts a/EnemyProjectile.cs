using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 15;
    public float lifeTime = 5f;

    private Transform target;
    private Vector3 direction;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (!other.isTrigger && !other.CompareTag("EnemyDal"))
        {
            Destroy(gameObject);
        }
    }
}