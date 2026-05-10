using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private GameObject magicProjectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float magicCooldown = 3f;

    private float lastMagicTime;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Физическая атака (ЛКМ)");
            MeleeAttack();
        }

        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Магическая атака (ПКМ)");
            MagicAttack();
        }
    }

    void MeleeAttack()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                Debug.Log($"Попал по {hit.collider.name} физической атакой");
                target.TakeDamage(new DamageData(stats.physicalDamage, DamageType.Physical));
            }
            else
            {
                Debug.Log("Физическая атака: цель без IDamageable");
            }
        }
        else
        {
            Debug.Log("Физическая атака: никого не задел");
        }
    }

    void MagicAttack()
    {
        if (Time.time < lastMagicTime + magicCooldown)
        {
            Debug.Log("Магия на кулдауне");
            return;
        }

        GameObject projectile = Instantiate(
            magicProjectilePrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Projectile proj = projectile.GetComponent<Projectile>();

        if (proj != null)
        {
            proj.Init(stats.magicalDamage);
        }

        Debug.Log("Выстрел магией!");
        lastMagicTime = Time.time;
    }
}