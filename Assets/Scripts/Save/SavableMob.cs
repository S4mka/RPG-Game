using System;
using AdvancedRPG.Gameplay.Combat;
using UnityEngine;

namespace AdvancedRPG.Save
{
    [RequireComponent(typeof(CharacterHealthView))]
    public sealed class SavableMob : MonoBehaviour
    {
        [SerializeField] private string id;
        private CharacterHealthView healthView;
        public string Id => id;
        private void Awake()
        {
            if (string.IsNullOrWhiteSpace(id)) id = Guid.NewGuid().ToString();
            healthView = GetComponent<CharacterHealthView>();
        }
        public MobSaveData Capture() => new MobSaveData { id = id, x = transform.position.x, y = transform.position.y, z = transform.position.z, hp = healthView.Health.Current, maxHp = healthView.Health.Max, dead = healthView.Health.IsDead };
        public void Restore(MobSaveData data)
        {
            transform.position = new Vector3(data.x, data.y, data.z);
            healthView.Restore(data.hp, data.maxHp);
            gameObject.SetActive(!data.dead);
        }
    }
}
