using AdvancedRPG.Core;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class MeleeHitbox : MonoBehaviour
    {
        [SerializeField] private float damage = 25f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float activeSeconds = 0.2f;

        private Collider hitbox;
        private GameObject owner;

        private void Awake()
        {
            hitbox = GetComponent<Collider>();
            hitbox.isTrigger = true;
            hitbox.enabled = false;
        }

        public void Activate(GameObject attackOwner, float customDamage = -1f)
        {
            owner = attackOwner;
            if (customDamage >= 0f) damage = customDamage;
            hitbox.enabled = true;
            CancelInvoke(nameof(Deactivate));
            Invoke(nameof(Deactivate), activeSeconds);
        }

        private void Deactivate() => hitbox.enabled = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == owner) return;
            if (((1 << other.gameObject.layer) & targetMask) == 0) return;
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                ServiceLocator.Get<IDamageService>().Apply(target, new DamageData(damage, DamageType.Physical, owner));
                Deactivate();
            }
        }
    }
}
