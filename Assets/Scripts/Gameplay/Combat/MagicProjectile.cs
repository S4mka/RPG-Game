using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    [RequireComponent(typeof(Collider))]
    public class MagicProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 5f;

        private GameObject owner;
        private int damage;
        private DamageType damageType;
        private bool initialized;

        public void Init(GameObject newOwner, int newDamage, DamageType newDamageType)
        {
            owner = newOwner;
            damage = newDamage;
            damageType = newDamageType;
            initialized = true;

            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!initialized)
                return;

            if (owner != null && other.transform.root.gameObject == owner)
                return;

            IDamageable damageable = other.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(
                    new DamageData(
                        damage,
                        damageType,
                        owner));

                Destroy(gameObject);
                return;
            }

            if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}