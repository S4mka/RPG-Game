using AdvancedRPG.Core;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class MagicProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private LayerMask targetMask;

        private float damage;
        private GameObject owner;
        private Vector3 direction;

        public void Launch(GameObject projectileOwner, Vector3 dir, float amount, LayerMask mask)
        {
            owner = projectileOwner;
            direction = dir.normalized;
            damage = amount;
            targetMask = mask;
            Destroy(gameObject, lifeTime);
        }

        private void Update() => transform.position += direction * speed * Time.deltaTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == owner) return;
            if (((1 << other.gameObject.layer) & targetMask) == 0) return;
            if (other.TryGetComponent<IDamageable>(out var target))
                ServiceLocator.Get<IDamageService>().Apply(target, new DamageData(damage, DamageType.Magical, owner));
            Destroy(gameObject);
        }
    }
}
