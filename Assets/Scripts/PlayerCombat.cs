using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageDealer
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float magicCooldown = 3f;

    private float lastMagicTime;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MeleeAttack();

        if (Input.GetMouseButtonDown(1))
            MagicAttack();
    }

    void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
                target.TakeDamage(new DamageData(stats.physicalDamage, DamageType.Physical));
        }
    }

    void MagicAttack()
    {
        if (Time.time < lastMagicTime + magicCooldown)
            return;

        Instantiate(magicProjectilePrefab, shootPoint.position, shootPoint.rotation);
        lastMagicTime = Time.time;
    }

    public DamageData GetDamage()
    {
        return new DamageData(stats.physicalDamage, DamageType.Physical);
    }
}