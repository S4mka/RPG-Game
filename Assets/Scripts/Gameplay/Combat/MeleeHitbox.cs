using System.Collections.Generic;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    [RequireComponent(typeof(Collider))]
    public class MeleeHitbox : MonoBehaviour
    {
        private readonly HashSet<IDamageable> damagedTargets = new HashSet<IDamageable>();

        [SerializeField] private Collider hitboxCollider;

        private GameObject owner;
        private int damage;
        private bool isActive;

        private void Awake()
        {
            if (hitboxCollider == null)
                hitboxCollider = GetComponent<Collider>();

            hitboxCollider.isTrigger = true;
            hitboxCollider.enabled = false;
        }

        public void Activate(GameObject newOwner, int newDamage)
        {
            owner = newOwner;
            damage = newDamage;

            damagedTargets.Clear();

            isActive = true;
            hitboxCollider.enabled = true;

            CancelInvoke(nameof(Deactivate));
            Invoke(nameof(Deactivate), 0.25f);
        }

        private void Deactivate()
        {
            isActive = false;

            if (hitboxCollider != null)
                hitboxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
                return;

            if (owner != null && other.transform.root.gameObject == owner)
                return;

            IDamageable damageable =
                other.GetComponentInParent<IDamageable>();

            if (damageable == null)
                return;

            if (damagedTargets.Contains(damageable))
                return;

            damagedTargets.Add(damageable);

            damageable.TakeDamage(
                new DamageData(
                    damage,
                    DamageType.Physical,
                    owner));
        }
    }
}