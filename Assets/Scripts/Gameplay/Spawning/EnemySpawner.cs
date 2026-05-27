using AdvancedRPG.Gameplay.Enemies;
using AdvancedRPG.Gameplay.Weapons;
using AdvancedRPG.Save;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedRPG.Gameplay.Spawning
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private EnemyBrain[] commonEnemyPrefabs;
        [SerializeField] private EnemyBrain rareEnemyPrefab;

        [Header("Weapons")]
        [SerializeField] private EnemyWeaponDefinition[] meleeWeapons;
        [SerializeField] private EnemyWeaponDefinition[] rangedWeapons;

        [Header("Spawn")]
        [SerializeField] private string spawnerId;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private int spawnCount = 4;
        [SerializeField] private float randomRadius = 12f;
        [SerializeField] private bool spawnOnStart = true;

        [Header("Rare Mob")]
        [Range(0f, 1f)]
        [SerializeField] private float rareChance = 0.1f;

        [Header("Links")]
        [SerializeField] private Transform target;
        [SerializeField] private MobKillCounter killCounter;

        private int spawnedCount;

        private void Start()
        {
            if (spawnOnStart)
                Spawn();
        }

        public void Spawn()
        {
            FindLinksIfNeeded();

            for (int i = 0; i < spawnCount; i++)
            {
                EnemyBrain prefab = ChoosePrefab();
                if (prefab == null)
                {
                    Debug.LogWarning($"{name}: no enemy prefabs assigned.");
                    return;
                }

                Vector3 position = GetSpawnPosition(i);
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                EnemyBrain enemy = Instantiate(prefab, position, rotation);
                enemy.Construct(target);
                AssignSavableId(enemy);

                EnemyWeaponSlot slot = enemy.GetComponent<EnemyWeaponSlot>();
                if (slot == null)
                    slot = enemy.gameObject.AddComponent<EnemyWeaponSlot>();

                EnemyWeaponDefinition weapon = ChooseWeapon();
                if (weapon != null)
                    slot.SetWeapon(weapon);

                killCounter?.RegisterEnemy(enemy);
            }
        }

        private void FindLinksIfNeeded()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    target = player.transform;
            }

            if (killCounter == null)
                killCounter = FindObjectOfType<MobKillCounter>();
        }

        private EnemyBrain ChoosePrefab()
        {
            if (rareEnemyPrefab != null && Random.value <= rareChance)
                return rareEnemyPrefab;

            if (commonEnemyPrefabs == null || commonEnemyPrefabs.Length == 0)
                return rareEnemyPrefab;

            return commonEnemyPrefabs[Random.Range(0, commonEnemyPrefabs.Length)];
        }

        private EnemyWeaponDefinition ChooseWeapon()
        {
            int meleeCount = meleeWeapons == null ? 0 : meleeWeapons.Length;
            int rangedCount = rangedWeapons == null ? 0 : rangedWeapons.Length;
            int total = meleeCount + rangedCount;

            if (total == 0)
                return null;

            int index = Random.Range(0, total);
            if (index < meleeCount)
                return meleeWeapons[index];

            return rangedWeapons[index - meleeCount];
        }

        private Vector3 GetSpawnPosition(int index)
        {
            Vector3 rawPosition;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                Transform point = spawnPoints[index % spawnPoints.Length];
                rawPosition = point == null ? transform.position : point.position;
            }
            else
            {
                Vector2 circle = Random.insideUnitCircle * randomRadius;
                rawPosition = transform.position + new Vector3(circle.x, 0f, circle.y);
            }

            if (NavMesh.SamplePosition(rawPosition, out NavMeshHit hit, 4f, NavMesh.AllAreas))
                return hit.position;

            return rawPosition;
        }

        private void AssignSavableId(EnemyBrain enemy)
        {
            SavableMob savableMob = enemy.GetComponent<SavableMob>();
            if (savableMob == null)
                savableMob = enemy.gameObject.AddComponent<SavableMob>();

            savableMob.SetId($"{GetSpawnerId()}_mob_{spawnedCount}");
            spawnedCount++;
        }

        private string GetSpawnerId()
        {
            if (!string.IsNullOrWhiteSpace(spawnerId))
                return spawnerId;

            string sceneName = gameObject.scene.IsValid() ? gameObject.scene.name : "Scene";
            return $"{sceneName}_{GetHierarchyPath(transform)}";
        }

        private static string GetHierarchyPath(Transform current)
        {
            string path = $"{current.name}_{current.GetSiblingIndex()}";
            while (current.parent != null)
            {
                current = current.parent;
                path = $"{current.name}_{current.GetSiblingIndex()}_{path}";
            }

            return path;
        }
    }
}
